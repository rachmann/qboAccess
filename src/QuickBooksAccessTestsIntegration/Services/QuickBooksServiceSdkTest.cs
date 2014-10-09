﻿using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using QuickBooksAccess.Models.GetOrders;
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

		[Test]
		public void GetOrders_ServiceContainsOrders_OrdersReceived()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksServiceSdk.GetOrders(DateTime.Now.AddMonths(-1), DateTime.Now);

			//A
			getOrdersResponse.Orders.Count().Should().BeGreaterThan(0);
		}

		[Test]
		public void CreateOrders_ServiceDontContainsTheSameOrders_OrdersCreated()
		{
			//A

			//A
			var getOrdersResponse = this._quickBooksServiceSdk.CreateOrders(new Order);

			//A
			getOrdersResponse.Orders.Count().Should().BeGreaterThan(0);
		}
	}
}