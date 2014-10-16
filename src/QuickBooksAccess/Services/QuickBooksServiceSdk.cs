using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using QuickBooksAccess.Misc;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.CreateOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.CreatePurchaseOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetInvoices;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetItems;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPayments;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPurchaseOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetSalesReceipts;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.UpdateInventory;
using Invoice = Intuit.Ipp.Data.Invoice;
using Item = Intuit.Ipp.Data.Item;
using Payment = Intuit.Ipp.Data.Payment;
using Task = System.Threading.Tasks.Task;

namespace QuickBooksAccess.Services
{
	internal class QuickBooksServiceSdk
	{
		private readonly OAuthRequestValidator _requestValidator;
		private readonly ServiceContext _serviceContext;
		private readonly DataService _dataService;
		private readonly QueryService< Item > _queryServiceItem;
		private readonly QueryService< Payment > _queryServicePayment;
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
			this._queryServicePayment = new QueryService< Payment >( this._serviceContext );
			this._queryServiceAccount = new QueryService< Account >( this._serviceContext );
			this._queryServicePurchaseOrder = new QueryService< PurchaseOrder >( this._serviceContext );
			this._queryServiceSalesReceipt = new QueryService< SalesReceipt >( this._serviceContext );
			this._queryServiceInvoice = new QueryService< Invoice >( this._serviceContext );
		}

		#region Items
		public async Task< UpdateItemQuantityOnHandResponse > UpdateItemQuantityOnHand( params InventoryItem[] inventoryItems )
		{
			return await Task.Factory.StartNew( () =>
			{
				var batch = this._dataService.CreateNewBatch();

				foreach( var item in inventoryItems )
				{
					batch.Add( new Item()
					{
						Name = item.Sku,
						Id = item.Id,
						SyncToken = item.SyncToken,
						QtyOnHand = item.QtyOnHand,
						QtyOnHandSpecified = true,
						ExpenseAccountRef = new ReferenceType { Value = item.ExpenseAccRefValue, name = item.ExpenseAccRefName, type = item.ExpenseAccRefType },
						IncomeAccountRef = new ReferenceType { Value = item.IncomeAccRefValue, name = item.IncomeAccRefName, type = item.IncomeAccRefType }
					}, item.Id, OperationEnum.update );
				}

				batch.Execute();
				return new UpdateItemQuantityOnHandResponse( new List< Customer >() );
			} ).ConfigureAwait( false );
		}

		public async Task< GetItemsResponse > GetItems( params string[] skus )
		{
			// simle query
			//var items = this._queryServiceItem.Where( x => x.Name.In( skus ) ).ToList();
			//var itemsConvertedToQBAccessItems = items.Select( y => y.ToQBAccessItem() ).ToList();
			//return new GetItemsResponse( itemsConvertedToQBAccessItems );

			// query with pages
			//var itemsCollections = new ConcurrentBag<IEnumerable<Item>>();
			//var getItemsWithPagesAsync = this._queryServiceItem.Where(x => x.Name.In(skus)).DoWithPagesAsync(
			//	1,
			//	y => (Task.Factory.StartNew(() => itemsCollections.Add(y))));
			//getItemsWithPagesAsync.Wait();
			//var queredItems = itemsCollections.SelectMany(x => x).ToList();

			// batch query with

			return await Task.Factory.StartNew( () =>
			{
				var itemsQuery = this._queryServiceItem.Where( x => x.Name.In( skus ) ).ToIdsQuery();
				var itemsQueryBatch = this._dataService.CreateNewBatch();
				itemsQueryBatch.Add( itemsQuery, "bID1" );
				itemsQueryBatch.Execute();
				var queryResponse = itemsQueryBatch[ "bID1" ];
				var items = queryResponse.Entities.Cast< Item >().ToList();
				var itemsConvertedToQbAccessItems = items.Select( x => x.ToQBAccessItem() ).ToList();
				return new GetItemsResponse( itemsConvertedToQbAccessItems );
			} ).ConfigureAwait( false );
		}
		#endregion

		#region PurchaseOrders
		public async Task< GetPurchaseOrdersResponse > GetPurchseOrders( DateTime from, DateTime to )
		{
			return await Task.Factory.StartNew( () =>
			{
				var purchaseOrdersFilteredFrom = this._queryServicePurchaseOrder.Where( x => x.MetaData.CreateTime >= from ).ToList();
				//todo: try to avoid additional filter with 'to', and inject it in first query
				var purchaseOrdersFilteredFromAndTo = purchaseOrdersFilteredFrom.Where( x => x.MetaData.CreateTime <= to ).ToList();
				return new GetPurchaseOrdersResponse( purchaseOrdersFilteredFromAndTo );
			} ).ConfigureAwait( false );
		}

		public async Task< CreatePurchaseOrdersResponse > CreatePurchaseOrders( params PurchaseOrder[] purchaseOrders )
		{
			return await Task.Factory.StartNew( () =>
			{
				throw new Exception();
				return new CreatePurchaseOrdersResponse();
			} ).ConfigureAwait( false );
		}
		#endregion

		#region Orders
		public async Task< GetSalesReceiptsResponse > GetSalesReceipt( DateTime from, DateTime to )
		{
			return await Task.Factory.StartNew( () =>
			{
				var ordersFilteredFrom = this._queryServiceSalesReceipt.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
				//todo: try to avoid additional filter with 'to', and inject it in first query
				var ordersFilteredFromAndTo = ordersFilteredFrom.Where( x => x.MetaData.LastUpdatedTime <= to ).ToList();
				return new GetSalesReceiptsResponse( ordersFilteredFromAndTo );
			} ).ConfigureAwait( false );
		}

		public async Task< GetInvoicesResponse > GetInvoices( DateTime from, DateTime to )
		{
			return await Task.Factory.StartNew( () =>
			{
				var invoicesFilteredFrom = this._queryServiceInvoice.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
				//todo: try to avoid additional filter with 'to', and inject it in first query
				var invoicesFilteredFromAndTo = invoicesFilteredFrom.Where( x => x.MetaData.LastUpdatedTime <= to ).ToList();
				var invoicesConverted = invoicesFilteredFromAndTo.Select( x => x.ToQBAccessInvoice() ).ToList();
				return new GetInvoicesResponse( invoicesConverted );
			} ).ConfigureAwait( false );
		}

		public async Task< CreateOrdersResponse > CreateOrders( params SalesOrder[] orders )
		{
			return await Task.Factory.StartNew( () =>
			{
				throw new Exception();
				return new CreateOrdersResponse();
			} ).ConfigureAwait( false );
		}

		public async Task< GetPaymentsResponse > GetPayments( DateTime lastUpdateTimeFrom, DateTime lastUpdateTimeTo )
		{
			return await Task.Factory.StartNew( () =>
			{
				//var itemsQuery = this._queryServicePayment.Where( x => x.MetaData.LastUpdatedTime >= lastUpdateTimeFrom && x.MetaData.LastUpdatedTime <= lastUpdateTimeTo ).ToIdsQuery();
				var itemsQuery = this._queryServicePayment.Where( x => x.MetaData.LastUpdatedTime >= lastUpdateTimeFrom ).ToIdsQuery();
				var itemsQueryBatch = this._dataService.CreateNewBatch();
				itemsQueryBatch.Add( itemsQuery, "bID1" );
				itemsQueryBatch.Execute();
				var queryResponse = itemsQueryBatch[ "bID1" ];
				var items = queryResponse.Entities.Cast< Payment >().ToList();
				var itemsConvertedToQbAccessItems = items.Select( x => x.ToQBAccessPayment() ).ToList();
				return new GetPaymentsResponse( itemsConvertedToQbAccessItems );
			} ).ConfigureAwait( false );
		}
		#endregion
	}
}