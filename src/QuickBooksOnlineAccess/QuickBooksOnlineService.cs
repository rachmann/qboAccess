using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QuickBooksOnlineAccess.Misc;
using QuickBooksOnlineAccess.Models;
using QuickBooksOnlineAccess.Models.GetOrders;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.GetPurchaseOrders;
using QuickBooksOnlineAccess.Models.Ping;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetVendors;
using QuickBooksOnlineAccess.Models.UpdateInventory;
using QuickBooksOnlineAccess.Services;

namespace QuickBooksOnlineAccess
{
	public class QuickBooksOnlineService : IQuickBooksOnlineService
	{
		private readonly QuickBooksOnlineServiceSdk _quickBooksOnlineServiceSdk;
		private readonly RestProfile _restProfile;
		private readonly ConsumerProfile _consumerProfile;
		public Func< string > AdditionalLogInfo { get; set; }

		public QuickBooksOnlineService( QuickBooksOnlineAuthenticatedUserCredentials quickBooksAuthenticatedUserCredentials, QuickBooksOnlineNonAuthenticatedUserCredentials quickBooksNonAuthenticatedUserCredentials )
		{
			this._restProfile = new RestProfile()
			{
				AppToken = quickBooksNonAuthenticatedUserCredentials.AppToken,
				DataSource = quickBooksAuthenticatedUserCredentials.DataSource,
				OAuthAccessToken = quickBooksAuthenticatedUserCredentials.OAuthAccessToken,
				OAuthAccessTokenSecret = quickBooksAuthenticatedUserCredentials.OAuthAccessTokenSecret,
				RealmId = quickBooksAuthenticatedUserCredentials.RealmId,
			};

			this._consumerProfile = new ConsumerProfile()
			{
				ConsumerKey = quickBooksNonAuthenticatedUserCredentials.ConsumerKey,
				ConsumerSecret = quickBooksNonAuthenticatedUserCredentials.ConsumerSecret,
			};

			this._quickBooksOnlineServiceSdk = new QuickBooksOnlineServiceSdk( this._restProfile, this._consumerProfile );
		}

		public async Task< PingInfo > Ping()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task< IEnumerable< PurchaseOrder > > GetPurchaseOrdersOrdersAsync( DateTime dateFrom, DateTime dateTo )
		{
			var methodParameters = string.Format( "{{dateFrom:{0},dateTo:{1}}}", dateFrom, dateTo );
			var mark = Guid.NewGuid().ToString();
			try
			{
				QuickBooksOnlineLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var getPurchaseOrdersResponse = await this._quickBooksOnlineServiceSdk.GetPurchseOrders( dateFrom, dateTo ).ConfigureAwait( false );

				var result = getPurchaseOrdersResponse.PurchaseOrders.ToQBPurchaseOrder().ToList();

				QuickBooksOnlineLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : result.ToJson() ) );

				return result;
			}
			catch( Exception exception )
			{
				var ebayException = new QuickBooksOnlineException( string.Format( "Error. Was called:{0}", this.CreateMethodCallInfo( methodParameters, mark ) ), exception );
				LogTraceException( ebayException.Message, ebayException );
				throw ebayException;
			}
		}

		public async Task CreatePurchaseOrdersOrdersAsync( params Models.CreatePurchaseOrders.PurchaseOrder[] purchaseOrders )
		{
			var methodParameters = string.Format( "{{purchaseOrders:{0}}}", purchaseOrders.ToJson() );
			var mark = Guid.NewGuid().ToString();
			try
			{
				QuickBooksOnlineLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				if( purchaseOrders == null || !purchaseOrders.Any() )
					return;

				var getItemsResponse = await this._quickBooksOnlineServiceSdk.GetItems();
				var items = getItemsResponse.Items;
				FillPurchaseOrdersLineItemsById( purchaseOrders, items.ToQBProduct() );
				//var ordersWithExistingLineItems = GetPurchaseOrdersWithExistingLineItems( purchaseOrders, items );

				var getVendorsResponse = await this._quickBooksOnlineServiceSdk.GetVendors();
				var vendors = getVendorsResponse.Vendors;
				var ordersWithExistingVendor = this.GetPurchaseOrdersWithExistingVendor( purchaseOrders, vendors );
				var createPurchaseOrdersResponse = await this._quickBooksOnlineServiceSdk.CreatePurchaseOrders( ordersWithExistingVendor.Select( x => x.ToQBPurchaseOrder() ).ToArray() ).ConfigureAwait( false );

				QuickBooksOnlineLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark ) );
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		private IEnumerable< Models.CreatePurchaseOrders.PurchaseOrder > GetPurchaseOrdersWithExistingVendor( IEnumerable< Models.CreatePurchaseOrders.PurchaseOrder > purchaseOrders, IEnumerable< Vendor > vendors )
		{
			var ordersToCreate = new List< Models.CreatePurchaseOrders.PurchaseOrder >();
			var vendorsList = vendors as IList< Vendor > ?? vendors.ToList();
			foreach( var purchaseOrder in purchaseOrders )
			{
				var vendor = vendorsList.FirstOrDefault( x => x.Name == purchaseOrder.VendorName );
				if( vendor != null )
				{
					purchaseOrder.VendorValue = vendor.Id;
					ordersToCreate.Add( purchaseOrder );
				}
			}
			return ordersToCreate;
		}

		internal static void FillPurchaseOrdersLineItemsById( IEnumerable< Models.CreatePurchaseOrders.PurchaseOrder > purchaseOrders, IEnumerable< Product > items )
		{
			var itemsList = items as IList< Product > ?? items.ToList();
			foreach( var purchaseOrder in purchaseOrders )
			{
				foreach( var lineItem in purchaseOrder.LineItems.ToList() )
				{
					var itemMayBe = itemsList.FirstOrDefault( item => item.Name == lineItem.ItemName );
					if( itemMayBe != null )
						lineItem.Id = itemMayBe.Id;
				}
			}
		}

		public async Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo )
		{
			var methodParameters = string.Format( "{{dateFrom:{0},dateTo:{1}}}", dateFrom, dateTo );
			var mark = Guid.NewGuid().ToString();
			try
			{
				QuickBooksOnlineLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				dateFrom = dateFrom.ToUniversalTime();
				dateTo = dateTo.ToUniversalTime();

				var invoices = await this._quickBooksOnlineServiceSdk.GetInvoices( dateFrom, dateTo ).ConfigureAwait( false );
				var salesReceipts = await this._quickBooksOnlineServiceSdk.GetSalesReceipt( dateFrom, dateTo ).ConfigureAwait( false );

				var invoicesConverted = invoices.Invoices.ToQBOrder().ToList();
				var salesReceiptsConverted = salesReceipts.Orders.ToQBOrder().ToList();

				var result = invoicesConverted.Concat( salesReceiptsConverted );

				QuickBooksOnlineLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : result.ToJson() ) );

				return result;
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task< IEnumerable< Order > > GetOrdersAsync( params string[] docNumbers )
		{
			var methodParameters = string.Format( "{{docNumbers:{0}}}", docNumbers.ToJson() );
			var mark = Guid.NewGuid().ToString();

			try
			{
				QuickBooksOnlineLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var invoices = await this._quickBooksOnlineServiceSdk.GetInvoices( docNumbers ).ConfigureAwait( false );
				var salesReceipts = await this._quickBooksOnlineServiceSdk.GetSalesReceipt( docNumbers ).ConfigureAwait( false );

				var invoicesConverted = invoices.Invoices.ToQBOrder().ToList();
				var salesReceiptsConverted = salesReceipts.Orders.ToQBOrder().ToList();

				var result = invoicesConverted.Concat( salesReceiptsConverted );

				QuickBooksOnlineLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : result.ToJson() ) );

				return result;
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task< IEnumerable< Order > > GetOrdersAsync()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task< IEnumerable< Product > > GetProductsAsync()
		{
			var methodParameters = string.Format( "{{{0}}}", PredefinedValues.NotAvailable );
			var mark = Guid.NewGuid().ToString();
			try
			{
				QuickBooksOnlineLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var itemsResponse = await this._quickBooksOnlineServiceSdk.GetTrackingItems().ConfigureAwait( false );

				var result = itemsResponse.Items.ToQBProduct();

				QuickBooksOnlineLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : result.ToJson() ) );

				return result;
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task UpdateInventoryAsync( IEnumerable< Inventory > products )
		{
			var methodParameters = string.Format( "{{products:{0}}}", products.ToJson() );
			var mark = Guid.NewGuid().ToString();
			try
			{
				QuickBooksOnlineLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				if( products == null || !products.Any() )
					return;

				var response = await this._quickBooksOnlineServiceSdk.UpdateItemQuantityOnHand( products.ToQBInventoryItem().ToArray() ).ConfigureAwait( false );

				QuickBooksOnlineLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : PredefinedValues.NotAvailable ) );
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		private string CreateMethodCallInfo( string methodParameters = "", string mark = "", string errors = "", string methodResult = "", string additionalInfo = "", [ CallerMemberName ] string memberName = "" )
		{
			var restInfo = this._quickBooksOnlineServiceSdk.ToJson();
			var str = string.Format(
				"{{MethodName:{0}, ConnectionInfo:{1}, MethodParameters:{2}, Mark:{3}{4}{5}{6}}}",
				memberName,
				restInfo,
				methodParameters,
				mark,
				string.IsNullOrWhiteSpace( errors ) ? string.Empty : ", Errors:" + errors,
				string.IsNullOrWhiteSpace( methodResult ) ? string.Empty : ", Result:" + methodResult,
				string.IsNullOrWhiteSpace( additionalInfo ) ? string.Empty : ", " + additionalInfo
				);
			return str;
		}

		private static void LogTraceException( string message, QuickBooksOnlineException ebayException )
		{
			QuickBooksOnlineLogger.Log().Trace( ebayException, message );
		}
	}
}