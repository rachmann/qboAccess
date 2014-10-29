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

		public async Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo )
		{
			try
			{
				dateFrom = dateFrom.ToUniversalTime();
				dateTo = dateTo.ToUniversalTime();

				var invoices = await this._quickBooksOnlineServiceSdk.GetInvoices( dateFrom, dateTo ).ConfigureAwait( false );
				var salesReceipts = await this._quickBooksOnlineServiceSdk.GetSalesReceipt( dateFrom, dateTo ).ConfigureAwait( false );

				var invoicesConverted = invoices.Invoices.ToQBOrder().ToList();
				var salesReceiptsConverted = salesReceipts.Orders.ToQBOrder().ToList();

				return invoicesConverted.Concat( salesReceiptsConverted );
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
			try
			{
				var invoices = await this._quickBooksOnlineServiceSdk.GetInvoices( docNumbers ).ConfigureAwait( false );
				var salesReceipts = await this._quickBooksOnlineServiceSdk.GetSalesReceipt( docNumbers ).ConfigureAwait( false );

				var invoicesConverted = invoices.Invoices.ToQBOrder().ToList();
				var salesReceiptsConverted = salesReceipts.Orders.ToQBOrder().ToList();

				return invoicesConverted.Concat( salesReceiptsConverted );
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

		public async Task< IEnumerable< Product > > GetProductsSimpleAsync()
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
			try
			{
				var itemsResponse = await this._quickBooksOnlineServiceSdk.GetTrackingItems().ConfigureAwait( false );
				return itemsResponse.Items.ToQBProduct();
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
			try
			{
				var response = await this._quickBooksOnlineServiceSdk.UpdateItemQuantityOnHand( products.ToQBInventoryItem().ToArray() ).ConfigureAwait( false );
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