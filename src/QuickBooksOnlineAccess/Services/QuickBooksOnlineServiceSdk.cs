using System;
using System.Linq;
using System.Threading.Tasks;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using QuickBooksOnlineAccess.Misc;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreatePurchaseOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetBills;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetItems;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdateItemQuantityOnHand;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders;
using Bill = Intuit.Ipp.Data.Bill;
using Invoice = Intuit.Ipp.Data.Invoice;
using Item = Intuit.Ipp.Data.Item;
using Payment = Intuit.Ipp.Data.Payment;
using PurchaseOrder = Intuit.Ipp.Data.PurchaseOrder;
using SalesReceipt = Intuit.Ipp.Data.SalesReceipt;
using Task = System.Threading.Tasks.Task;

namespace QuickBooksOnlineAccess.Services
{
	internal class QuickBooksOnlineServiceSdk
	{
		private readonly OAuthRequestValidator _requestValidator;
		private readonly ServiceContext _serviceContext;
		private readonly DataService _dataService;
		private readonly QueryService< Item > _queryServiceItem;
		private readonly QueryService< Payment > _queryServicePayment;
		private readonly QueryService< Account > _queryServiceAccount;
		private readonly QueryService< PurchaseOrder > _queryServicePurchaseOrder;
		private readonly QueryService< Bill > _queryServiceBill;
		private readonly QueryService< SalesReceipt > _queryServiceSalesReceipt;
		private readonly QueryService< Invoice > _queryServiceInvoice;
		private readonly QueryService< Customer > _queryServiceCustomer;

		public ConsumerProfile ConsumerProfile { get; set; }

		public RestProfile RestProfile { get; set; }

		public QuickBooksOnlineServiceSdk( RestProfile restProfile, ConsumerProfile consumerProfile )
		{
			this.RestProfile = restProfile;
			this.ConsumerProfile = consumerProfile;
			this._requestValidator = new OAuthRequestValidator( this.RestProfile.OAuthAccessToken, this.RestProfile.OAuthAccessTokenSecret, this.ConsumerProfile.ConsumerKey, this.ConsumerProfile.ConsumerSecret );
			this._serviceContext = new ServiceContext( this.RestProfile.AppToken, this.RestProfile.RealmId, IntuitServicesType.QBO, this._requestValidator );
			this._dataService = new DataService( this._serviceContext );
			this._queryServiceItem = new QueryService< Item >( this._serviceContext );
			this._queryServicePayment = new QueryService< Payment >( this._serviceContext );
			this._queryServiceAccount = new QueryService< Account >( this._serviceContext );
			this._queryServicePurchaseOrder = new QueryService< PurchaseOrder >( this._serviceContext );
			this._queryServiceBill = new QueryService< Bill >( this._serviceContext );
			this._queryServiceSalesReceipt = new QueryService< SalesReceipt >( this._serviceContext );
			this._queryServiceInvoice = new QueryService< Invoice >( this._serviceContext );
			this._queryServiceCustomer = new QueryService< Customer >( this._serviceContext );
		}

		#region Items
		public async Task< UpdateItemQuantityOnHandResponse > UpdateItemQuantityOnHand( params InventoryItem[] inventoryItems )
		{
			return await Task.Factory.StartNew( () =>
			{
				if( inventoryItems == null || inventoryItems.Length == 0 )
					return new UpdateItemQuantityOnHandResponse();

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
				return new UpdateItemQuantityOnHandResponse();
			} ).ConfigureAwait( false );
		}

		public async Task< GetItemsResponse > GetItems( params string[] skus )
		{
			var itemsQuery = string.Format( "Select * FROM Item  WHERE   Name IN ({0})", string.Join( ",", skus.Select( x => "'" + x + "'" ) ) );

			return await Task.Factory.StartNew( () =>
			{
				var itemsQueryBatch = this._dataService.CreateNewBatch();
				itemsQueryBatch.Add( itemsQuery, "bID1" );
				itemsQueryBatch.Execute();
				var queryResponse = itemsQueryBatch[ "bID1" ];
				var items = queryResponse.Entities.Cast< Item >().ToList();
				var itemsConvertedToQbAccessItems = items.Select( x => x.ToQBAccessItem() ).ToList();
				return new GetItemsResponse( itemsConvertedToQbAccessItems );
			} ).ConfigureAwait( false );
		}

		public async Task< GetItemsResponse > GetTrackingItems()
		{
			return await Task.Factory.StartNew( () =>
			{
				var items = this._queryServiceItem.Where( x => x.Type == ItemTypeEnum.Inventory ).ToList();
				return new GetItemsResponse( items.Select( x => x.ToQBAccessItem() ).ToList() );
			} ).ConfigureAwait( false );
		}
		#endregion

		#region PurchaseOrders
		public async Task< GetBillsResponse > GetBills( DateTime from, DateTime to )
		{
			return await Task.Factory.StartNew( () =>
			{
				var billsFilteredFrom = this._queryServiceBill.Where( x => x.MetaData.CreateTime >= from ).ToList();
				var billsFilteredFromAndTo = billsFilteredFrom.Where( x => x.MetaData.CreateTime <= to ).ToList();
				var billsFilteredFromAndToConverted = billsFilteredFromAndTo.Select( x => x.ToQBBill() ).ToList();
				return new GetBillsResponse( billsFilteredFromAndToConverted );
			} ).ConfigureAwait( false );
		}

		public async Task< GetPurchaseOrdersResponse > GetPurchseOrders( DateTime from, DateTime to )
		{
			var itemsQuery = string.Format( "Select * FROM PurchaseOrder  WHERE MetaData.LastUpdatedTime >= '{0}' && MetaData.LastUpdatedTime <= '{1}'", from.ToStringUtcIso8601(), to.ToStringUtcIso8601() );

			return await Task.Factory.StartNew( () =>
			{
				var itemsQueryBatch = this._dataService.CreateNewBatch();
				itemsQueryBatch.Add( itemsQuery, "bID1" );
				itemsQueryBatch.Execute();
				var queryResponse = itemsQueryBatch[ "bID1" ];
				var items = queryResponse.Entities.Cast< PurchaseOrder >().ToList();
				var itemsConvertedToQbAccessItems = items.Select( x => x.ToQBServicePurchaseOrder() ).ToList();
				return new GetPurchaseOrdersResponse( itemsConvertedToQbAccessItems );
			} ).ConfigureAwait( false );

			//return await Task.Factory.StartNew( () =>
			//{
			//	var purchaseOrdersFilteredFrom = this._queryServicePurchaseOrder.Where( x => x.MetaData.LastUpdatedTime >= from ).ToList();
			//	var purchaseOrdersFilteredFromAndTo = purchaseOrdersFilteredFrom.Where( x => x.MetaData.LastUpdatedTime <= to ).ToList();
			//	return new GetPurchaseOrdersResponse( purchaseOrdersFilteredFromAndTo.Select( x => x.ToQBServicePurchaseOrder() ) );
			//} ).ConfigureAwait( false );
		}

		public async Task< CreatePurchaseOrdersResponse > CreatePurchaseOrders( params PurchaseOrder[] purchaseOrders )
		{
			return await Task.Factory.StartNew( () =>
			{
				if( purchaseOrders == null || purchaseOrders.Length == 0 )
					return new CreatePurchaseOrdersResponse();

				//todo: implemet
				throw new NotImplementedException();
			} ).ConfigureAwait( false );
		}

		public async Task< UpdatePurchaseOrdersResponse > UpdatePurchaseOrders( params Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders.PurchaseOrder[] purchaseOrders )
		{
			return await Task.Factory.StartNew( () =>
			{
				if( purchaseOrders == null || purchaseOrders.Length == 0 )
					return new UpdatePurchaseOrdersResponse();

				var batch = this._dataService.CreateNewBatch();

				foreach( var item in purchaseOrders )
				{
					batch.Add( item.ToIppPurchaseOrder(), item.Id, OperationEnum.update );
				}

				batch.Execute();
				return new UpdatePurchaseOrdersResponse();
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
				var ordersFilteredFromAndTo = ordersFilteredFrom.Where( x => x.MetaData.LastUpdatedTime.ToUniversalTime() <= to ).ToList();
				return new GetSalesReceiptsResponse( ordersFilteredFromAndTo.Select( x => x.ToQBSalesReceipt() ) );
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

		public async Task< GetSalesReceiptsResponse > GetSalesReceipt( params string[] docNumbers )
		{
			var itemsQuery = string.Format( "Select * FROM SalesReceipt WHERE DocNumber IN ({0})", string.Join( ",", docNumbers.Select( x => "'" + x + "'" ) ) );

			return await Task.Factory.StartNew( () =>
			{
				var itemsQueryBatch = this._dataService.CreateNewBatch();
				itemsQueryBatch.Add( itemsQuery, "bID1" );
				itemsQueryBatch.Execute();
				var queryResponse = itemsQueryBatch[ "bID1" ];
				var items = queryResponse.Entities.Cast< SalesReceipt >().ToList();
				var itemsConvertedToQbAccessItems = items.Select( x => x.ToQBSalesReceipt() ).ToList();
				return new GetSalesReceiptsResponse( itemsConvertedToQbAccessItems );

				//var salesReceipts = this._queryServiceSalesReceipt.Where( x => x.DocNumber.In( docNumbers ) ).ToList();
				//return new GetSalesReceiptsResponse( salesReceipts.Select( x => x.ToQBSalesReceipt() ) );
			} ).ConfigureAwait( false );
		}

		public async Task< GetInvoicesResponse > GetInvoices( params string[] docNumbers )
		{
			var itemsQuery = string.Format( "Select * FROM Invoice WHERE DocNumber IN ({0})", string.Join( ",", docNumbers.Select( x => "'" + x + "'" ) ) );

			return await Task.Factory.StartNew( () =>
			{
				var itemsQueryBatch = this._dataService.CreateNewBatch();
				itemsQueryBatch.Add( itemsQuery, "bID1" );
				itemsQueryBatch.Execute();
				var queryResponse = itemsQueryBatch[ "bID1" ];
				var items = queryResponse.Entities.Cast< Invoice >().ToList();
				var itemsConvertedToQbAccessItems = items.Select( x => x.ToQBAccessInvoice() ).ToList();
				return new GetInvoicesResponse( itemsConvertedToQbAccessItems );

				//var invoices = this._queryServiceInvoice.Where( x => x.DocNumber.In( docNumbers ) ).ToList();
				//var invoicesConverted = invoices.Select( x => x.ToQBAccessInvoice() ).ToList();
				//return new GetInvoicesResponse( invoicesConverted );
			} ).ConfigureAwait( false );
		}

		public async Task< CreateInvoicesResponse > CreateInvoices( params Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice.Invoice[] orders )
		{
			return await Task.Factory.StartNew( () =>
			{
				if( orders == null || orders.Length == 0 )
					return new CreateInvoicesResponse();

				var invoicesForBatch = orders.ToList().Select( x => x.ToIppInvoice() ).ToDictionary( x => Guid.NewGuid().ToString() );

				var batch = this._dataService.CreateNewBatch();
				foreach( var invoiceKey in invoicesForBatch.Keys )
				{
					batch.Add( invoicesForBatch[ invoiceKey ], invoiceKey, OperationEnum.create );
				}

				batch.Execute();
				return new CreateInvoicesResponse();
			} ).ConfigureAwait( false );
		}

		public async Task< GetPaymentsResponse > GetPayments( DateTime lastUpdateTimeFrom, DateTime lastUpdateTimeTo )
		{
			return await Task.Factory.StartNew( () =>
			{
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

		public string ToJson()
		{
			var res = string.Format( "{{RestProfile:{0},ConsumerProfile:{1}}}", this.RestProfile.ToJson(), this.ConsumerProfile.ToJson() );
			return res;
		}
	}
}