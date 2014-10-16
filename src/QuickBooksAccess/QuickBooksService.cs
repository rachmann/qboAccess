using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QuickBooksAccess.Misc;
using QuickBooksAccess.Models;
using QuickBooksAccess.Models.GetOrders;
using QuickBooksAccess.Models.GetProducts;
using QuickBooksAccess.Models.Ping;
using QuickBooksAccess.Models.PutInventory;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;
using QuickBooksAccess.Services;

namespace QuickBooksAccess
{
	public class QuickBooksService : IQuickBooksService
	{
		private QuickBooksServiceSdk quickBooksServiceSdk;
		private RestProfile _restProfile;
		private ConsumerProfile _consumerProfile;

		public QuickBooksService( QuickBooksAuthenticatedUserCredentials quickBooksAuthenticatedUserCredentials )
		{
			this._restProfile = new RestProfile()
			{
				AppToken = quickBooksAuthenticatedUserCredentials.AppToken,
				CompanyId = quickBooksAuthenticatedUserCredentials.CompanyId,
				DataSource = quickBooksAuthenticatedUserCredentials.DataSource,
				OAuthAccessToken = quickBooksAuthenticatedUserCredentials.OAuthAccessToken,
				OAuthAccessTokenSecret = quickBooksAuthenticatedUserCredentials.OAuthAccessTokenSecret,
				RealmId = quickBooksAuthenticatedUserCredentials.RealmId,
			};

			this._consumerProfile = new ConsumerProfile()
			{
				ConsumerKey = quickBooksAuthenticatedUserCredentials.ConsumerKey,
				ConsumerSecret = quickBooksAuthenticatedUserCredentials.ConsumerSecret,
			};

			this.quickBooksServiceSdk = new QuickBooksServiceSdk( this._restProfile, this._consumerProfile );
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
				var quickBooksException = new QuickBooksException( this.CreateMethodCallInfo(), exception );
				QuickBooksLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo )
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksException( this.CreateMethodCallInfo(), exception );
				QuickBooksLogger.LogTraceException( quickBooksException );
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
				var quickBooksException = new QuickBooksException( this.CreateMethodCallInfo(), exception );
				QuickBooksLogger.LogTraceException( quickBooksException );
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
				var quickBooksException = new QuickBooksException( this.CreateMethodCallInfo(), exception );
				QuickBooksLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task< IEnumerable< Product > > GetProductsAsync()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksException( this.CreateMethodCallInfo(), exception );
				QuickBooksLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		public async Task UpdateInventoryAsync( IEnumerable< Inventory > products )
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksException( this.CreateMethodCallInfo(), exception );
				QuickBooksLogger.LogTraceException( quickBooksException );
				throw quickBooksException;
			}
		}

		private string CreateMethodCallInfo( string additionalInfo = "", [ CallerMemberName ] string memberName = "" )
		{
			var str = string.Format(
				"MethodName:{0}, ConsumerProfile:{1}, RestProfile:{2}, AdditionalInfo:{3}",
				memberName,
				this._consumerProfile.ToJson(),
				this._restProfile.ToJson(),
				string.IsNullOrWhiteSpace( additionalInfo ) ? PredefinedValues.NotAvailable : additionalInfo
				);
			return str;
		}
	}
}