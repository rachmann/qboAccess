using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Netco.Logging;
using Netco.Logging.NLogIntegration;
using NUnit.Framework;
using QuickBooksOnlineAccess;
using QuickBooksOnlineAccess.Models;
using QuickBooksOnlineAccess.Models.CreateOrders;
using QuickBooksOnlineAccess.Models.CreatePurchaseOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccessTestsIntegration.TestEnvironment;
using OrderLineItem = QuickBooksOnlineAccess.Models.CreatePurchaseOrders.OrderLineItem;

namespace QuickBooksOnlineAccessTestsIntegration
{
	[ TestFixture ]
	public class QuickBooksOnlineServiceTest
	{
		private TestDataReader _testDataReader;
		private ConsumerProfile _consumerProfile;
		private RestProfile _restProfile;
		//private QuickBooksOnlineServiceSdk _quickBooksOnlineServiceSdk;
		private QuickBooksOnlineService _quickBooksService;
		private QuickBooksOnlineAuthenticatedUserCredentials _quickBooksAuthenticatedUserCredentials;
		private QuickBooksOnlineNonAuthenticatedUserCredentials _quickBooksNonAuthenticatedUserCredentials;

		[ TestFixtureSetUp ]
		public void TestFixtureSetup()
		{
			this._testDataReader = new TestDataReader( @"..\..\Files\quickBooksOnline_consumerprofile.csv", @"..\..\Files\quickBooksOnline_restprofile.csv" );
			NetcoLogger.LoggerFactory = new NLogLoggerFactory();
		}

		[ SetUp ]
		public void TestSetup()
		{
			this._consumerProfile = this._testDataReader.ConsumerProfile;
			this._restProfile = this._testDataReader.RestProfile;
			//this._quickBooksOnlineServiceSdk = new QuickBooksOnlineServiceSdk( this._restProfile, this._consumerProfile );

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
			var invoicesTask = this._quickBooksService.GetOrdersAsync( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			invoicesTask.Wait();

			var realInvoicesIds = invoicesTask.Result.Select( x => x.DocNumber ).ToList();
			var fakeInvoices = new List< string >() { "1000" };

			//A
			var filteredInvoicesTask = this._quickBooksService.GetOrdersAsync( realInvoicesIds.Concat( fakeInvoices ).ToArray() );
			var filteredInvoicesResponse = filteredInvoicesTask.Result;

			//A
			realInvoicesIds.Count.Should().BeGreaterThan( 0 );
			filteredInvoicesResponse.Count().Should().Be( realInvoicesIds.Count );
		}

		[ Test ]
		public void CreatePurchaseOrders_ServiceDontContainsTheSamePurchaseOrders_PurchaseOrdersCreated()
		{
			//A
			var purchaseOrders = new PurchaseOrder[]
			{
				new PurchaseOrder
				{
					DocNumber = "123-103",
					PoStatus = PoStatusEnum.Open,
					VendorName = "Shoes Supplier",
					LineItems = new List< OrderLineItem >
					{
						new OrderLineItem() { ItemName = "testSku1", Qty = 3, Rate = 1.1m, },
						new OrderLineItem() { ItemName = "testSku2", Qty = 4, Rate = 1.11m, }
					},
				}
			};

			//A
			var createOrdersResponse = this._quickBooksService.CreatePurchaseOrdersOrdersAsync( purchaseOrders );
			createOrdersResponse.Wait();

			//A
			var getOrdersResponse = this._quickBooksService.GetPurchaseOrdersOrdersAsync( DateTime.UtcNow.AddMinutes( -5 ), DateTime.UtcNow.AddMinutes( 5 ) );
			getOrdersResponse.Result.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void CreateOrder_ServiceDontContainsSuchOrder_SaleReceiptCreated()
		{
			//A
			var receipt = new Order
			{
				DocNumber = "1-1-5-54-28400-105",
				LineItems = new List< QuickBooksOnlineAccess.Models.CreateOrders.OrderLineItem >
				{
					new QuickBooksOnlineAccess.Models.CreateOrders.OrderLineItem()
					{
						Qty = 3,
						ItemName = "testSku1",
						Rate = 12.3m,
					}
				},
				CustomerName = "Mrs Francine Smith",
				OrderStatus = OrderStatusEnum.Paid,
				TnxDate = new DateTime( 2014, 11, 15 ),
			};

			//A
			var createSaleReceipts = this._quickBooksService.CreateOrdersAsync( receipt );
			createSaleReceipts.Wait();

			//A
			var getSaleRceipts = this._quickBooksService.GetOrdersAsync( receipt.DocNumber );
			var salesReceiptsResponse = getSaleRceipts.Result;

			//A
			salesReceiptsResponse.ToList().Where( x => x.DocNumber == receipt.DocNumber ).ToList().Count.Should().BeGreaterThan( 0 );
		}
	}
}