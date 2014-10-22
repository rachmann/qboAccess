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
			this._quickBooksAuthenticatedUserCredentials = new QuickBooksOnlineAuthenticatedUserCredentials()
			{
				ConsumerKey = this._consumerProfile.ConsumerKey,
				ConsumerSecret = this._consumerProfile.ConsumerSecret,
				AppToken = this._restProfile.AppToken,
				CompanyId = this._restProfile.CompanyId,
				OAuthAccessToken = this._restProfile.OAuthAccessToken,
				OAuthAccessTokenSecret = this._restProfile.OAuthAccessTokenSecret,
				RealmId = this._restProfile.RealmId,
				DataSource = this._restProfile.DataSource,
			};
			this._quickBooksService = new QuickBooksOnlineService( this._quickBooksAuthenticatedUserCredentials );
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