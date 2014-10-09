using System;
using System.Linq;
using FluentAssertions;
using Intuit.Ipp.Data;
using NUnit.Framework;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;
using QuickBooksAccess.Services;
using QuickBooksAccessTestsIntegration.TestEnvironment;

namespace QuickBooksAccessTestsIntegration.Services
{
	[ TestFixture ]
	public class QuickBooksServiceSdkTest
	{
		private TestDataReader _testDataReader;
		private ConsumerProfile _consumerProfile;
		private RestProfile _restProfile;
		private QuickBooksServiceSdk _quickBooksServiceSdk;

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
		}

		[ Test ]
		public void getSomeInfo_ServiceContainsInfo_InfoReceived()
		{
			//A
			//A
			this._quickBooksServiceSdk.UpdateInventory();
			//A
		}

		[ Test ]
		public void GetPurchaseOrders_ServiceContainsPurchaseOrders_PurchaseOrdersReceived()
		{
			//A

			//A
			var getPurchaseOrdersResponse = this._quickBooksServiceSdk.GetPurchseOrders( DateTime.Now.AddMonths( -1 ), DateTime.Now );

			//A
			getPurchaseOrdersResponse.PurchaseOrders.Count.Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void CreatePurchaseOrders_ServiceDontContainsTheSamePurchaseOrders_PurchaseOrdersCreated()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksServiceSdk.CreatePurchaseOrders( new PurchaseOrder[ 0 ] );

			//A
		}

		[ Test ]
		public void GetSalesReceipt_ServiceContainsSalesReceipt_SalesReceiptReceived()
		{
			//A

			//A
			var getSalesReceiptResponse = this._quickBooksServiceSdk.GetSalesReceipt( DateTime.Now.AddMonths( -1 ), DateTime.Now );

			//A
			getSalesReceiptResponse.Orders.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void CreateOrders_ServiceDontContainsTheSameOrders_OrdersCreated()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksServiceSdk.CreateOrders( new SalesOrder[ 0 ] );

			//A
		}
	}
}