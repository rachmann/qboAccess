using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QuickBooksOnlineAccess.Misc;
using QuickBooksOnlineAccess.Models;
using QuickBooksOnlineAccess.Models.GetOrders;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.Ping;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Models.UpdateInventory;
using QuickBooksOnlineAccess.Services;

namespace QuickBooksOnlineAccess
{
	public class QuickBooksOnlineService : IQuickBooksOnlineService
	{
		private QuickBooksOnlineServiceSdk _quickBooksOnlineServiceSdk;
		private readonly RestProfile _restProfile;
		private readonly ConsumerProfile _consumerProfile;

		public QuickBooksOnlineService( QuickBooksOnlineAuthenticatedUserCredentials quickBooksAuthenticatedUserCredentials )
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

		public async Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo )
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

		public async Task UpdateInventoryAsync( IEnumerable< Inventory > products )
		{
			try
			{
			}
			catch( Exception exception )
			{
				var quickBooksException = new QuickBooksOnlineException( this.CreateMethodCallInfo(), exception );
				QuickBooksOnlineLogger.LogTraceException( quickBooksException );
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