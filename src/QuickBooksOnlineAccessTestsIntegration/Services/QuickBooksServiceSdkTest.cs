using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using QuickBooksOnlineAccess.Misc;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders;
using QuickBooksOnlineAccess.Services;
using QuickBooksOnlineAccessTestsIntegration.TestEnvironment;
using PurchaseOrder = Intuit.Ipp.Data.PurchaseOrder;

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
		public void UpdatePurchaseOrders_ServiceContainsPurchaseOrders_PurchaseOrdersUpdated()
		{
			//A
			//#1
			//todo: create instead of pull
			var getPurchaseOrders1 = this._quickBooksOnlineServiceSdk.GetPurchseOrders( DateTime.Now.AddMonths( -1 ), DateTime.Now );

			var ordersToUpdate1 = getPurchaseOrders1.Result.PurchaseOrders.Select( x => x.ToQBInternalPurchaseOrder() ).ToList();
			ordersToUpdate1.ForEach( x => x.POStatus = QBInternalPurchaseOrderStatusEnum.Closed );

			var updatePurchaseOrders1 = this._quickBooksOnlineServiceSdk.UpdatePurchaseOrders( ordersToUpdate1.ToArray() );
			updatePurchaseOrders1.Wait();

			//#2
			var getPurchaseOrders2 = this._quickBooksOnlineServiceSdk.GetPurchseOrders( DateTime.Now.AddMonths( -1 ), DateTime.Now.AddMinutes( 5 ) );

			var ordersToUpdate2 = getPurchaseOrders2.Result.PurchaseOrders.Select( x => x.ToQBInternalPurchaseOrder() ).ToList();
			ordersToUpdate2.ForEach( x => x.POStatus = QBInternalPurchaseOrderStatusEnum.Open );

			//A
			var updatePurchaseOrders2 = this._quickBooksOnlineServiceSdk.UpdatePurchaseOrders( ordersToUpdate2.ToArray() );
			updatePurchaseOrders2.Wait();

			var getPurchaseOrders3 = this._quickBooksOnlineServiceSdk.GetPurchseOrders( DateTime.Now.AddMonths( -1 ), DateTime.Now.AddMinutes( 5 ) );
			//A
			getPurchaseOrders1.Result.PurchaseOrders.Should().HaveCount( x => x > 0 );
			getPurchaseOrders2.Result.PurchaseOrders.Should().HaveSameCount( getPurchaseOrders1.Result.PurchaseOrders );
			getPurchaseOrders3.Result.PurchaseOrders.Should().HaveSameCount( getPurchaseOrders2.Result.PurchaseOrders );
			getPurchaseOrders2.Result.PurchaseOrders.Should().OnlyContain( x => x.PoStatus == QBPurchaseOrderStatusEnum.Closed );
			getPurchaseOrders3.Result.PurchaseOrders.Should().OnlyContain( x => x.PoStatus == QBPurchaseOrderStatusEnum.Open );
		}

		[ Test ]
		public void CreateOrder_ServiceDontContainsSuchOrder_OrderCreated()
		{
			//A
			var invoice = new Invoicek
			{
				DocNumber = "1-1-5-54-28400-101",
				Line = new List< Linek >
				{
					new Linek()
					{
						Qty = 3,
						ItemValue = "21",
						ItemName = "testSku1",
						Amount = 35.0m,
					}
				},
				CustomerValue = "3",
				CustomerName = "Francine"
			};

			//A
			var getPurchaseOrders2 = this._quickBooksOnlineServiceSdk.CreateOrders( invoice );
			getPurchaseOrders2.Wait();

			//A
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

		[ Test ]
		public void GetInvoices_GettingByIdsServiceContainsInvoices_InvoicesReceived()
		{
			//A
			var invoicesTask = this._quickBooksOnlineServiceSdk.GetInvoices( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			invoicesTask.Wait();

			var realInvoicesIds = invoicesTask.Result.Invoices.Select( x => x.DocNumber ).ToList();
			var fakeInvoices = new List< string >() { "1000" };

			//A
			var filteredInvoicesTask = this._quickBooksOnlineServiceSdk.GetInvoices( realInvoicesIds.Concat( fakeInvoices ).ToArray() );
			var filteredInvoicesResponse = filteredInvoicesTask.Result;

			//A
			realInvoicesIds.Count.Should().BeGreaterThan( 0 );
			filteredInvoicesResponse.Invoices.Count.Should().Be( realInvoicesIds.Count );
		}

		[ Test ]
		public void GetSalesReceipts_GettingByIdsServiceContainsSalesReceipt_SalesReceiptReceived()
		{
			//A
			var getAllSalesReceiptsResponseTask = this._quickBooksOnlineServiceSdk.GetSalesReceipt( DateTime.Now.AddMonths( -1 ), DateTime.Now );
			var realReceiptsIds = getAllSalesReceiptsResponseTask.Result.Orders.Select( x => x.DocNumber ).ToList();
			var fakeIds = new List< string >() { "1000" };

			//A
			var getSalesReceiptsResponseTask = this._quickBooksOnlineServiceSdk.GetSalesReceipt( realReceiptsIds.Concat( fakeIds ).ToArray() );
			var getSalesReceiptsResponse = getSalesReceiptsResponseTask.Result;

			//A
			realReceiptsIds.Count.Should().BeGreaterThan( 0 );
			getSalesReceiptsResponse.Orders.Count().Should().Be( realReceiptsIds.Count );
		}

		[ Test ]
		public void GetTrackingItems_ServiceContainsTrackingItems_ItemsReceived()
		{
			//A

			//A
			var getSalesReceiptsResponseTask = this._quickBooksOnlineServiceSdk.GetTrackingItems();
			var getSalesReceiptsResponse = getSalesReceiptsResponseTask.Result;

			//A
			getSalesReceiptsResponse.Items.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void CreateOrders_ServiceDontContainsTheSameOrders_OrdersCreated()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksOnlineServiceSdk.CreateOrders();

			//A
		}
	}
}