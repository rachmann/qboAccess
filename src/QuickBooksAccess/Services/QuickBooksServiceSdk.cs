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
		private readonly OAuthRequestValidator _requestValidator;
		private readonly ServiceContext _serviceContext;
		private readonly DataService _dataService;
		private readonly QueryService< Item > _queryServiceItem;
		private readonly QueryService< Account > _queryServiceAccount;
		private readonly QueryService< PurchaseOrder > _queryServicePurchaseOrder;
		private readonly QueryService< SalesReceipt > _queryServiceSalesReceipt;
		private readonly QueryService< Invoice > _queryServiceInvoice;

		public ConsumerProfile ConsumerProfile { get; set; }

		public RestProfile RestProfile { get; set; }

		public QuickBooksServiceSdk( RestProfile restProfile, ConsumerProfile consumerProfile )
		{
			this.RestProfile = restProfile;
			this.ConsumerProfile = consumerProfile;
			this._requestValidator = new OAuthRequestValidator( this.RestProfile.OAuthAccessToken, this.RestProfile.OAuthAccessTokenSecret, this.ConsumerProfile.ConsumerKey, this.ConsumerProfile.ConsumerSecret );
			this._serviceContext = new ServiceContext( this.RestProfile.AppToken, this.RestProfile.CompanyId, IntuitServicesType.QBO, this._requestValidator );
			this._dataService = new DataService( this._serviceContext );
			this._queryServiceItem = new QueryService< Item >( this._serviceContext );
			this._queryServiceAccount = new QueryService< Account >( this._serviceContext );
			this._queryServicePurchaseOrder = new QueryService< PurchaseOrder >( this._serviceContext );
			this._queryServiceSalesReceipt = new QueryService< SalesReceipt >( this._serviceContext );
			this._queryServiceInvoice = new QueryService< Invoice >( this._serviceContext );
		}

		public UpdateInventoryResponse UpdateItemQuantityOnHand( params InventoryItem[] inventoryItems )
		{
			//get items
			var items = this._queryServiceItem.Where( x => x.Type == ItemTypeEnum.Inventory ).ToList();
			var skus = inventoryItems.Select( x => x.Sku ).ToArray();

			var itemsCollections = new ConcurrentBag< IEnumerable< Item > >();
			var items2 = this._queryServiceItem.Where( x => x.Name.In( skus ) ).DoWithPagesAsync(
				1,
				y => ( Task.Factory.StartNew( () => itemsCollections.Add( y ) ) ) );
			items2.Wait();
			var itemsCollectionsMany = itemsCollections.SelectMany( x => x ).ToList();

			var itemsQuery = this._queryServiceItem.Where( x => x.Name.In( skus ) ).ToIdsQuery();

			var itemsQueryBatch = this._dataService.CreateNewBatch();
			itemsQueryBatch.Add( itemsQuery, "bID1" );
			itemsQueryBatch.Execute();
			var queryResponse = itemsQueryBatch[ "bID1" ];
			var customers = queryResponse.Entities.Cast< Item >().ToList();

			var batch = this._dataService.CreateNewBatch();
			var accounts = this._queryServiceAccount.Where( x => x.Name == "Cost of Goods Sold" ).ToList();
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
			var purchaseOrdersFilteredFrom = this._queryServicePurchaseOrder.Where( x => x.MetaData.CreateTime >= from ).ToList();
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
			var ordersFilteredFrom = this._queryServiceSalesReceipt.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
			//todo: try to avoid additional filter with 'to', and inject it in first query
			var ordersFilteredFromAndTo = ordersFilteredFrom.Where( x => x.MetaData.LastUpdatedTime <= to ).ToList();
			return new GetSalesReceiptsResponse( ordersFilteredFromAndTo );
		}

		public GetInvoicesResponse GetInvoices( DateTime from, DateTime to )
		{
			var invoicesFilteredFrom = this._queryServiceInvoice.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
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
			var items = this._queryServiceItem.Where( x => x.Name.In( skus ) ).ToList();
			var itemsConvertedToQBAccessItems = items.Select( y => y.ToQBAccessItem() ).ToList();
			return new GetItemsResponse( itemsConvertedToQBAccessItems );
		}
	}
}