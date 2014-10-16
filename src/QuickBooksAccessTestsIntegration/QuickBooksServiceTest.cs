using System;
using NUnit.Framework;
using QuickBooksAccess;
using QuickBooksAccess.Models;
using QuickBooksAccess.Models.PutInventory;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;
using QuickBooksAccess.Services;
using QuickBooksAccessTestsIntegration.TestEnvironment;

namespace QuickBooksAccessTestsIntegration
{
	[ TestFixture ]
	public class QuickBooksServiceTest
	{
		private TestDataReader _testDataReader;
		private ConsumerProfile _consumerProfile;
		private RestProfile _restProfile;
		private QuickBooksServiceSdk _quickBooksServiceSdk;
		private QuickBooksService _quickBooksService;
		private QuickBooksAuthenticatedUserCredentials _quickBooksAuthenticatedUserCredentials;

		[ TestFixtureSetUp ]
		public void TestFixtureSetup()
		{
			this._testDataReader = new TestDataReader( @"..\..\Files\quickbooks_consumerprofile.csv", @"..\..\Files\quickbooks_restprofile.csv" );
		}

		[ SetUp ]
		public void TestSetup()
		{
			this._consumerProfile = this._testDataReader.ConsumerProfile;
			this._restProfile = this._testDataReader.RestProfile;
			this._quickBooksServiceSdk = new QuickBooksServiceSdk( this._restProfile, this._consumerProfile );
			this._quickBooksAuthenticatedUserCredentials = new QuickBooksAuthenticatedUserCredentials()
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
			this._quickBooksService = new QuickBooksService( this._quickBooksAuthenticatedUserCredentials );
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