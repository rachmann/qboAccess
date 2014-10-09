using System;
using System.Linq;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.CreateOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.CreatePurchaseOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPurchaseOrders;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.UpdateInventory;

namespace QuickBooksAccess.Services
{
	internal class QuickBooksServiceSdk
	{
		private ServiceContext GetServiceContext( RestProfile profile )
		{
			var oauthValidator = new OAuthRequestValidator( profile.OAuthAccessToken, profile.OAuthAccessTokenSecret, this.ConsumerProfile.ConsumerKey, this.ConsumerProfile.ConsumerSecret );
			//return new ServiceContext(profile.OAuthAccessToken, consumerKey, IntuitServicesType.QBO, oauthValidator);
			return new ServiceContext( this.RestProfile.AppToken, this.RestProfile.CompanyId, IntuitServicesType.QBO, oauthValidator );
		}

		public ConsumerProfile ConsumerProfile { get; set; }

		public RestProfile RestProfile { get; set; }

		public QuickBooksServiceSdk( RestProfile restProfile, ConsumerProfile consumerProfile )
		{
			this.RestProfile = restProfile;
			this.ConsumerProfile = consumerProfile;
		}

		public UpdateInventoryResponse UpdateInventory()
		{
			///standart
			var serviceContext = this.GetServiceContext( this.RestProfile );
			var customerQueryService = new QueryService< Customer >( serviceContext );
			var itemQueryService = new QueryService< Item >( serviceContext );
			var vv = itemQueryService.Select( c => c ).ToList();

			//my0
			//var itemQueryService22 = new DataService( serviceContext );
			//var vv2 = itemQueryService22.Update( new Item()
			//{
			//	Name = "testSku1",
			//	QtyOnHand = 31,
			//	Id = "20",
			//	SyncToken = "2",
			//	QtyOnHandSpecified = true,
			//	Active = true,
			//	ActiveSpecified = true
			//});

			//batch
			var dataService = new DataService( serviceContext );
			var batch = dataService.CreateNewBatch(); // = new Intuit.Ipp.DataService.Batch(serviceContext, new SyncRestHandler(serviceContext));
			batch.Add( new Item()
			{
				Name = "testSku1",
				Type = ItemTypeEnum.Inventory,
				TypeSpecified = true,
				SyncToken = "2",
				QtyOnHand = 31,
				QtyOnHandSpecified = true
			}, "20", OperationEnum.update );
			batch.Execute();
			return new UpdateInventoryResponse( customerQueryService.Select( c => c ).ToList() );
			////my
			//var dataService = new DataService(serviceContext);
			//dataService.FindAll<Customer>();

			//my2
			//			OAuthRequestValidator oauth = new OAuthRequestValidator(profile.OAuthAccessToken, profile.OAuthAccessTokenSecret, _consumerKey, _consumerSecret);           

			//ServiceContext context = new ServiceContext(profile.OAuthAccessToken, _consumerKey, IntuitServicesType.QBO, oauth);

			//DataService service = new DataService(context);

			//var customer = new QueryService<Customer>(service).;

			//String query = select($(customer.getId()), $(customer.getGivenName())).generate();

			//QueryResult queryResult = service.executeQuery(query);

			//System.out.println("from query: "+((Customer)queryResult.getEntities().get(0)).getGivenName());      
		}

		#region PurchaseOrders
		public GetPurchaseOrdersResponse GetPurchseOrders( DateTime from, DateTime to )
		{
			var context = this.GetServiceContext( this.RestProfile );
			var queryService = new QueryService< PurchaseOrder >( context );
			var purchaseOrdersFilteredFrom = queryService.Where( x => x.MetaData.CreateTime >= from ).ToList();
			//todo: try to avoid additional filter with 'to', and inject it in first query
			var purchaseOrdersFilteredFromAndTo = purchaseOrdersFilteredFrom.Where( x => x.MetaData.CreateTime <= to ).ToList();
			return new GetPurchaseOrdersResponse( purchaseOrdersFilteredFromAndTo );
		}

		public CreatePurchaseOrdersResponse CreatePurchaseOrders( params PurchaseOrder[] purchaseOrders )
		{
			throw new Exception();
		}
		#endregion

		#region Orders
		public GetOrdersResponse GetOrders( DateTime from, DateTime to )
		{
			throw new Exception();
		}

		public CreateOrdersResponse CreateOrders( params SalesOrder[] orders )
		{
			throw new Exception();
		}
		#endregion
	}
}