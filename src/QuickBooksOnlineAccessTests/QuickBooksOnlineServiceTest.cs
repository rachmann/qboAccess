﻿using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using QuickBooksOnlineAccess;
using QuickBooksOnlineAccess.Models;
using QuickBooksOnlineAccess.Models.CreatePurchaseOrders;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;

namespace QuickBooksOnlineAccessTests
{
	[ TestFixture ]
	public class QuickBooksOnlineServiceTest
	{
		private ConsumerProfile _consumerProfile;
		private RestProfile _restProfile;
		private QuickBooksOnlineService _quickBooksService;
		private QuickBooksOnlineAuthenticatedUserCredentials _quickBooksAuthenticatedUserCredentials;
		private QuickBooksOnlineNonAuthenticatedUserCredentials _quickBooksNonAuthenticatedUserCredentials;

		[ TestFixtureSetUp ]
		public void TestFixtureSetup()
		{
		}

		[ SetUp ]
		public void TestSetup()
		{
			this._consumerProfile = new ConsumerProfile() { ConsumerKey = "ConsumerKey", ConsumerSecret = "ConsumerSecret" };
			this._restProfile = new RestProfile() { AppToken = "AppToken", DataSource = 1, OAuthAccessToken = "OAuthAccessToken", OAuthAccessTokenSecret = "OAuthAccessTokenSecret", RealmId = "RealmId" };

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
		public void CreatePurchaseOrders_ServiceDontContainsTheSamePurchaseOrders_PurchaseOrdersCreated()
		{
			//A
			var products = new[]
			{
				new Product { Id = "1", Name = "testSku1" },
				new Product { Id = "2", Name = "testSku2" },
				new Product { Id = "3", Name = "testSku3" },
			};

			var purchaseOrder = new[]
			{
				new PurchaseOrder
				{
					LineItems = new[]
					{
						new OrderLineItem { ItemName = "testSku1" },
						new OrderLineItem { ItemName = "testSku2" }
					}
				},
				new PurchaseOrder
				{
					LineItems = new[]
					{
						new OrderLineItem { ItemName = "testSku2" },
						new OrderLineItem { ItemName = "testSku3" }
					}
				}
			};

			//A
			QuickBooksOnlineService.FillPurchaseOrdersLineItemsById( purchaseOrder, products );

			//A
			purchaseOrder.Should().OnlyContain( x => x.LineItems.All( y => y.Id == y.ItemName.Substring( y.ItemName.Length - 1 ) ) );
		}
	}
}