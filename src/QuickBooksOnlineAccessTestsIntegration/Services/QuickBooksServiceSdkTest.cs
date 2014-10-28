using System;
using System.Linq;
using FluentAssertions;
using Intuit.Ipp.Data;
using NUnit.Framework;
using QuickBooksOnlineAccess.Misc;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Services;
using QuickBooksOnlineAccessTestsIntegration.TestEnvironment;

namespace QuickBooksOnlineAccessTestsIntegration.Services
{
	[ TestFixture ]
	public class QuickBooksServiceSdkTest
	{
		private TestDataReader _testDataReader;
		private ConsumerProfile _consumerProfile;
		private RestProfile _restProfile;
		private QuickBooksOnlineServiceSdk _quickBooksOnlineServiceSdk;

		[ TestFixtureSetUp ]
		public void TestFixtureSetup()
		{
			this._testDataReader = new TestDataReader( @"..\..\Files\quickbooksOnline_consumerprofile.csv", @"..\..\Files\quickbooksOnline_restprofile.csv" );
		}

		[ SetUp ]
		public void TestSetup()
		{
			this._consumerProfile = this._testDataReader.ConsumerProfile;
			this._restProfile = this._testDataReader.RestProfile;
			this._quickBooksOnlineServiceSdk = new QuickBooksOnlineServiceSdk( this._restProfile, this._consumerProfile );
		}

		[ Test ]
		public void UpdateItemQuantityOnHand_ServiceContainsItems_InfoReceived()
		{
			//A
			var ItemsSkus = new[] { "testSku1", "testSku2", "testSku3", "testSku4", "testSku5" };
			var itemsTask = this._quickBooksOnlineServiceSdk.GetItems( ItemsSkus );
			itemsTask.Wait();
			var items = itemsTask.Result;
			items.Items.ForEach( x => x.Qty++ );
			var inventoryItems = items.Items.Select( x => x.ToInventoryItem() ).ToArray();

			//A
			var updateItemQuantityOnHandTask = this._quickBooksOnlineServiceSdk.UpdateItemQuantityOnHand( inventoryItems );
			updateItemQuantityOnHandTask.Wait();

			//A
			var updatedItemsTask = this._quickBooksOnlineServiceSdk.GetItems( ItemsSkus );
			updatedItemsTask.Wait();
			var updatedItems = updatedItemsTask.Result.Items.Select( x => x.ToInventoryItem() ).ToList();
			updatedItems.ForEach( x => x.SyncToken = "x" );
			inventoryItems.ToList().ForEach( x => x.SyncToken = "x" );
			updatedItems.ShouldBeEquivalentTo( inventoryItems );
		}

		[ Test ]
		public void GetPurchaseOrders_ServiceContainsPurchaseOrders_PurchaseOrdersReceived()
		{
			//A

			//A
			var getPurchaseOrdersResponseTask = this._quickBooksOnlineServiceSdk.GetPurchseOrders( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			var getPurchaseOrdersResponse = getPurchaseOrdersResponseTask.Result;

			//A
			getPurchaseOrdersResponse.PurchaseOrders.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void GetBills_ServiceContainsBills_BillsReceived()
		{
			//A

			//A
			var getPurchaseOrdersResponseTask = this._quickBooksOnlineServiceSdk.GetBills( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			getPurchaseOrdersResponseTask.Wait();
			var getPurchaseOrdersResponse = getPurchaseOrdersResponseTask.Result;

			//A
			getPurchaseOrdersResponse.Bills.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void CreatePurchaseOrders_ServiceDontContainsTheSamePurchaseOrders_PurchaseOrdersCreated()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksOnlineServiceSdk.CreatePurchaseOrders( new PurchaseOrder[ 0 ] );

			//A
		}

		[ Test ]
		public void GetInvoices_ServiceContainsInvoices_InvoicesReceived()
		{
			//A

			//A
			var getSalesReceiptsResponseTask = this._quickBooksOnlineServiceSdk.GetInvoices( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			var getSalesReceiptsResponse = getSalesReceiptsResponseTask.Result;

			//A
			getSalesReceiptsResponse.Invoices.Count.Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void GetPayments_ServiceContainsPayments_InvoicesReceived()
		{
			//A

			//A
			var paymentsResponseTask = this._quickBooksOnlineServiceSdk.GetPayments( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			var paymentsResponse = paymentsResponseTask.Result;

			//A
			paymentsResponse.Payments.Count.Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void GetSalesReceipts_ServiceContainsSalesReceipt_SalesReceiptReceived()
		{
			//A

			//A
			var getSalesReceiptsResponseTask = this._quickBooksOnlineServiceSdk.GetSalesReceipt( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			var getSalesReceiptsResponse = getSalesReceiptsResponseTask.Result;

			//A
			getSalesReceiptsResponse.Orders.Count().Should().BeGreaterThan( 0 );
		}

		[Test]
		public void GetTrackingItems_ServiceContainsTrackingItems_ItemsReceived()
		{
			//A

			//A
			var getSalesReceiptsResponseTask = this._quickBooksOnlineServiceSdk.GetTrackingItems();
			var getSalesReceiptsResponse = getSalesReceiptsResponseTask.Result;

			//A
			getSalesReceiptsResponse.Items.Count().Should().BeGreaterThan(0);
		}

		[ Test ]
		public void CreateOrders_ServiceDontContainsTheSameOrders_OrdersCreated()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksOnlineServiceSdk.CreateOrders( new SalesOrder[ 0 ] );

			//A
		}
	}
}