using System.Collections.Generic;
using System.Linq;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;

namespace QuickBooksAccess.Services
{
	internal class QuickBooksServiceSdk
	{
		public RestProfile profile { get; set; }
		private string _consumerKey { get; set; }
		public string _consumerSecret { get; set; }


		public UpdateInventoryResponse UpdateInventory()
		{
			ServiceContext serviceContext = getServiceContext(profile);
			QueryService<Customer> customerQueryService = new QueryService<Customer>(serviceContext);
			return new UpdateInventoryResponse(customerQueryService.Select(c => c).ToList());
		}

		private ServiceContext getServiceContext(RestProfile profile)
		{
			var consumerKey = _consumerKey;
			var consumerSecret = _consumerSecret;
			OAuthRequestValidator oauthValidator = new OAuthRequestValidator(profile.OAuthAccessToken, profile.OAuthAccessTokenSecret, consumerKey, consumerSecret);
			return new ServiceContext(profile.RealmId, (IntuitServicesType)profile.DataSource, oauthValidator);
		}
	}

	internal class UpdateInventoryResponse
	{
		public UpdateInventoryResponse( List< Customer > toList )
		{
		}
	}


	public class RestProfile
	{
		public RestProfile() { }

		//public static RestProfile GetRestProfile(string username)
		//{
		//	return Create(username) as RestProfile;
		//}

		//public static RestProfile GetRestProfile()
		//{
		//	return Create(Membership.GetUser().UserName) as RestProfile;
		//}

		private string _oAuthAccessToken { get; set; }
		private string _oAuthAccessTokenSecret { get; set; }
		private string _realmId { get; set; }
		private int _dataSource { get; set; }

		public string RealmId
		{
			get { return _realmId as string; }
			set { _realmId = value; }
		}

		public string OAuthAccessToken
		{
			get { return _oAuthAccessToken as string; }
			set { _oAuthAccessToken = value; }
		}

		public string OAuthAccessTokenSecret
		{
			get { return _oAuthAccessTokenSecret as string; }
			set { _oAuthAccessTokenSecret = value; }
		}

		public int DataSource
		{
			get { object dataSource = _dataSource; if (!dataSource.Equals(null)) { return (int)dataSource; } else { return -1; } }
			set { _dataSource = value; }
		}
	}
}