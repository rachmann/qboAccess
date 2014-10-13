using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Netco.Extensions;
using QuickBooksAccess.Misc;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.CreateOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.CreatePurchaseOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetInvoices;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetItems;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPurchaseOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetSalesReceipts;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.UpdateInventory;
using Item = Intuit.Ipp.Data.Item;
using Task = System.Threading.Tasks.Task;

namespace QuickBooksAccess.Services
{
	internal class QuickBooksServiceSdk
	{
		private ServiceContext GetServiceContext( RestProfile profile )
		{
			var oauthValidator = new OAuthRequestValidator( profile.OAuthAccessToken, profile.OAuthAccessTokenSecret, this.ConsumerProfile.ConsumerKey, this.ConsumerProfile.ConsumerSecret );
			//return new ServiceContext(profile.OAuthAccessToken, consumerKey, IntuitServicesType.QBO, oauthValidator);
			return new ServiceContext( this.RestProfile.AppToken, this.RestProfile.CompanyId, IntuitServicesType.QBO, oauthValidator );
		}

		public ConsumerProfile ConsumerProfile { get; set; }

		public RestProfile RestProfile { get; set; }

		public QuickBooksServiceSdk( RestProfile restProfile, ConsumerProfile consumerProfile )
		{
			this.RestProfile = restProfile;
			this.ConsumerProfile = consumerProfile;
		}

		public UpdateInventoryResponse UpdateItemQuantityOnHand( params InventoryItem[] inventoryItems )
		{
			//environment
			var oauthValidator = new OAuthRequestValidator( this.RestProfile.OAuthAccessToken, this.RestProfile.OAuthAccessTokenSecret, this.ConsumerProfile.ConsumerKey, this.ConsumerProfile.ConsumerSecret );
			var serviceContext = new ServiceContext( this.RestProfile.AppToken, this.RestProfile.CompanyId, IntuitServicesType.QBO, oauthValidator );
			var dataService = new DataService( serviceContext );
			var queryService = new QueryService< Item >( serviceContext );
			var queryServiceAccount = new QueryService< Account >( serviceContext );

			//get items
			var items = queryService.Where( x => x.Type == ItemTypeEnum.Inventory ).ToList();
			var skus = inventoryItems.Select( x => x.Sku ).ToArray();
			var itemsCollections = new ConcurrentBag< IEnumerable< Item > >();
			var items2 = queryService.Where( x => x.Name.In( skus ) ).DoWithPagesAsync(
				1,
				y => ( Task.Factory.StartNew( () => itemsCollections.Add( y ) ) ) );
			items2.Wait();
			var itemsCollectionsMany = itemsCollections.SelectMany(x => x).ToList();

			var itemsQuery = queryService.Where( x => x.Name.In( skus ) ).ToIdsQuery();

			var itemsQueryBatch = dataService.CreateNewBatch();
			itemsQueryBatch.Add( itemsQuery, "bID1" );
			itemsQueryBatch.Execute();
			var queryResponse = itemsQueryBatch[ "bID1" ];
			var customers = queryResponse.Entities.Cast< Item >().ToList();

			var batch = dataService.CreateNewBatch();
			var accounts = queryServiceAccount.Where( x => x.Name == "Cost of Goods Sold" ).ToList();
			var accReference = accounts.FirstOrDefault();
			var expenseAccountRef = new ReferenceType { type = accReference.AccountType.ToString(), name = accReference.Name, Value = accReference.Id };
			foreach( var item in items )
			{
				batch.Add( new Item()
				{
					Name = item.Name,
					Id = item.Id,
					//Type = ItemTypeEnum.Inventory,
					//TypeSpecified = true,
					SyncToken = item.SyncToken,
					QtyOnHand = item.QtyOnHand + 1,
					QtyOnHandSpecified = true,
					ExpenseAccountRef = expenseAccountRef,
				
				}, item.Id, OperationEnum.update );
			}

			batch.Execute();
			return new UpdateInventoryResponse( new List< Customer >() );
		}

		#region PurchaseOrders
		public GetPurchaseOrdersResponse GetPurchseOrders( DateTime from, DateTime to )
		{
			var context = this.GetServiceContext( this.RestProfile );
			var queryService = new QueryService< PurchaseOrder >( context );
			var purchaseOrdersFilteredFrom = queryService.Where( x => x.MetaData.CreateTime >= from ).ToList();
			//todo: try to avoid additional filter with 'to', and inject it in first query
			var purchaseOrdersFilteredFromAndTo = purchaseOrdersFilteredFrom.Where( x => x.MetaData.CreateTime <= to ).ToList();
			return new GetPurchaseOrdersResponse( purchaseOrdersFilteredFromAndTo );
		}

		public CreatePurchaseOrdersResponse CreatePurchaseOrders( params PurchaseOrder[] purchaseOrders )
		{
			throw new Exception();
		}
		#endregion

		#region Orders
		public GetSalesReceiptsResponse GetSalesReceipt( DateTime from, DateTime to )
		{
			var context = this.GetServiceContext( this.RestProfile );
			var queryService = new QueryService< SalesReceipt >( context );
			var ordersFilteredFrom = queryService.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
			//todo: try to avoid additional filter with 'to', and inject it in first query
			var ordersFilteredFromAndTo = ordersFilteredFrom.Where( x => x.MetaData.LastUpdatedTime <= to ).ToList();
			return new GetSalesReceiptsResponse( ordersFilteredFromAndTo );
		}

		public GetInvoicesResponse GetInvoices( DateTime from, DateTime to )
		{
			var context = this.GetServiceContext( this.RestProfile );
			var queryService = new QueryService< Invoice >( context );
			var invoicesFilteredFrom = queryService.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
			//todo: try to avoid additional filter with 'to', and inject it in first query
			var invoicesFilteredFromAndTo = invoicesFilteredFrom.Where( x => x.MetaData.LastUpdatedTime <= to ).ToList();
			return new GetInvoicesResponse( invoicesFilteredFromAndTo );
		}

		public CreateOrdersResponse CreateOrders( params SalesOrder[] orders )
		{
			throw new Exception();
		}
		#endregion

		public GetItemsResponse GetItems( params string[] skus )
		{
			var context = this.GetServiceContext( this.RestProfile );
			var queryService = new QueryService< Item >( context );
			var items = queryService.Where( x => x.Name.In( skus ) ).ToList();
			var itemsConvertedToQBAccessItems = items.Select( y => y.ToQBAccessItem() ).ToList();
			return new GetItemsResponse( itemsConvertedToQBAccessItems );
		}
	}
}