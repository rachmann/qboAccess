using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using QuickBooksOnlineAccess;
using QuickBooksOnlineAccess.Models;
using QuickBooksOnlineAccess.Models.CreatePurchaseOrders;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetVendors;

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
		public void FillPurchaseOrdersLineItemsById_ThereAreCorespondItemsForPurchaseOrderLineItems_PurchaseOrdersLineItemsFilled()
		{
			//A
			var products = new[]
			{
				new Product { Id = "1", Name = "testSku1" },
				new Product { Id = "2", Name = "testSku2" },
				new Product { Id = "3", Name = "testSku3" },
			};

			var purchaseOrders = new[]
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
			QuickBooksOnlineService.FillPurchaseOrdersLineItemsById( purchaseOrders, products );

			//A
			purchaseOrders.Should().OnlyContain( x => x.LineItems.All( y => y.Id == y.ItemName.Substring( y.ItemName.Length - 1 ) ) );
		}

		[ Test ]
		public void FillPurchaseOrdersVendorById_ThereAreCorespondVendorsForPurchaseOrder_PurchaseOrdersVendorsFilled()
		{
			//A
			var vendors = new[]
			{
				new Vendor { Id = "1", Name = "Vendor1" },
				new Vendor { Id = "2", Name = "Vendor2" },
				new Vendor { Id = "3", Name = "Vendor3" },
			};
			var purchaseOrders = new[]
			{
				new PurchaseOrder
				{
					VendorName = vendors[ 0 ].Name
				},
				new PurchaseOrder
				{
					VendorName = vendors[ 1 ].Name
				},
				new PurchaseOrder
				{
					VendorName = vendors[ 2 ].Name
				},
			};

			//A
			QuickBooksOnlineService.FillPurchaseOrdersByVendorId( purchaseOrders, vendors );

			//A
			purchaseOrders.Should().OnlyContain( x => x.VendorValue == x.VendorName.Substring( x.VendorName.Length - 1 ) );
		}

		[ Test ]
		public void GetOnlyPurchaseOrdersWithNotEmptyLineItemsId_ThereAreEmptyAndNotEmptyItemsInPurchaseOrder_ReturnedPurchaseOrdersOnlyWithNotEmptyLineItems()
		{
			//A
			var purchaseOrders = new[]
			{
				new PurchaseOrder
				{
					LineItems = new[]
					{
						new OrderLineItem { ItemName = "testSku1", Id = "1" },
						new OrderLineItem { ItemName = "testSku2" }
					}
				},
				new PurchaseOrder
				{
					LineItems = new[]
					{
						new OrderLineItem { ItemName = "testSku2" },
						new OrderLineItem { ItemName = "testSku3", Id = "3" }
					}
				},
				new PurchaseOrder
				{
					LineItems = new[]
					{
						new OrderLineItem { ItemName = "testSku2", Id = "2" },
						new OrderLineItem { ItemName = "testSku3", Id = "3" }
					}
				}
			};

			//A
			var filteredOrders = QuickBooksOnlineService.GetOnlyPurchaseOrdersWithNotEmptyLineItemsId( purchaseOrders );

			//A
			filteredOrders.Should().OnlyContain( x => x.LineItems.All( y => !string.IsNullOrWhiteSpace( y.Id ) && !string.IsNullOrWhiteSpace( y.ItemName ) ) );
		}

		[ Test ]
		public void GetOnlyPurchaseOrdersWithNotEmptyVendorId_ThereArePurchaseOrderWithEmptyAndNotEmptyVendor_ReturnedPurchaseOrdersOnlyWithNotEmptyVendor()
		{
			//A

			var purchaseOrders = new[]
			{
				new PurchaseOrder
				{
					VendorName = "vendor1",
					VendorValue = "1",
					DocNumber = "1"
				},
				new PurchaseOrder
				{
					VendorName = "vendor1",
					DocNumber = "2"
				},
				new PurchaseOrder
				{
					VendorName = "vendor1",
					DocNumber = "3"
				},
			};

			//A
			var filteredOrders = QuickBooksOnlineService.GetOnlyPurchaseOrdersWithNotEmptyVendorId( purchaseOrders );

			//A
			filteredOrders.Should().OnlyContain( y => y.DocNumber == "1" );
		}
	}
}