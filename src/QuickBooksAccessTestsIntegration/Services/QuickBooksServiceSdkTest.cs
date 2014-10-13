﻿using System;
using System.Linq;
using FluentAssertions;
using Intuit.Ipp.Data;
using NUnit.Framework;
using QuickBooksAccess.Misc;
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
		public void UpdateItemQuantityOnHand_ServiceContainsItems_InfoReceived()
		{
			//A
			var ItemsSkus = new[] { "testSku2", "testSku4" };
			var items = this._quickBooksServiceSdk.GetItems( ItemsSkus );
			items.Items.ForEach( x => x.Qty++ );
			var inventoryItems = items.Items.Select( x => x.ToInventoryItem() ).ToArray();

			//A
			this._quickBooksServiceSdk.UpdateItemQuantityOnHand( inventoryItems );

			//A
			var updatedItems = this._quickBooksServiceSdk.GetItems( ItemsSkus ).Items.Select( x => x.ToInventoryItem() ).ToList();
			updatedItems.ForEach( x => x.SyncToken = "x" );
			inventoryItems.ToList().ForEach( x => x.SyncToken = "x" );
			updatedItems.ShouldBeEquivalentTo( inventoryItems );
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
		public void GetInvoices_ServiceContainsInvoices_InvoicesReceived()
		{
			//A

			//A
			var getSalesReceiptsResponse = this._quickBooksServiceSdk.GetInvoices( DateTime.Now.AddMonths( -1 ), DateTime.Now );

			//A
			getSalesReceiptsResponse.Invoices.Count.Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void GetSalesReceipts_ServiceContainsSalesReceipt_SalesReceiptReceived()
		{
			//A

			//A
			var getSalesReceiptsResponse = this._quickBooksServiceSdk.GetSalesReceipt( DateTime.Now.AddMonths( -1 ), DateTime.Now );

			//A
			getSalesReceiptsResponse.Orders.Count().Should().BeGreaterThan( 0 );
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