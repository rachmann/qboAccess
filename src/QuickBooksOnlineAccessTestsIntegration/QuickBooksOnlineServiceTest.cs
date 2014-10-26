using System;
using NUnit.Framework;
using QuickBooksOnlineAccess;
using QuickBooksOnlineAccess.Models;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Models.UpdateInventory;
using QuickBooksOnlineAccess.Services;
using QuickBooksOnlineAccessTestsIntegration.TestEnvironment;

namespace QuickBooksOnlineAccessTestsIntegration
{
	[ TestFixture ]
	public class QuickBooksOnlineServiceTest
	{
		private TestDataReader _testDataReader;
		private ConsumerProfile _consumerProfile;
		private RestProfile _restProfile;
		private QuickBooksOnlineServiceSdk _quickBooksOnlineServiceSdk;
		private QuickBooksOnlineService _quickBooksService;
		private QuickBooksOnlineAuthenticatedUserCredentials _quickBooksAuthenticatedUserCredentials;
		private QuickBooksOnlineNonAuthenticatedUserCredentials _quickBooksNonAuthenticatedUserCredentials;

		[ TestFixtureSetUp ]
		public void TestFixtureSetup()
		{
			this._testDataReader = new TestDataReader( @"..\..\Files\quickBooksOnline_consumerprofile.csv", @"..\..\Files\quickBooksOnline_restprofile.csv" );
		}

		[ SetUp ]
		public void TestSetup()
		{
			this._consumerProfile = this._testDataReader.ConsumerProfile;
			this._restProfile = this._testDataReader.RestProfile;
			this._quickBooksOnlineServiceSdk = new QuickBooksOnlineServiceSdk( this._restProfile, this._consumerProfile );

			this._quickBooksAuthenticatedUserCredentials = new QuickBooksOnlineAuthenticatedUserCredentials(
				this._restProfile.RealmId,
				this._restProfile.OAuthAccessToken,
				this._restProfile.OAuthAccessTokenSecret,
				this._restProfile.DataSource );

			this._quickBooksNonAuthenticatedUserCredentials = new QuickBooksOnlineNonAuthenticatedUserCredentials(
				this._restProfile.AppToken,
				this._consumerProfile.ConsumerKey,
				this._consumerProfile.ConsumerSecret,
				"http://localhost:27286/home/Callback" );

			this._quickBooksService = new QuickBooksOnlineService( this._quickBooksAuthenticatedUserCredentials, this._quickBooksNonAuthenticatedUserCredentials );
		}

		[ Test ]
		public void test()
		{
			//A

			//A
			this._quickBooksService.UpdateInventoryAsync( new ArraySegment< Inventory >() ).Wait();
			//A
		}
	}
}