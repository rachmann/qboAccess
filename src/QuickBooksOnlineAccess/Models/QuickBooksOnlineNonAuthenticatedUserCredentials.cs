using Intuit.Ipp.Core;

namespace QuickBooksOnlineAccess.Models
{
	public class QuickBooksOnlineNonAuthenticatedUserCredentials
	{
		public QuickBooksOnlineNonAuthenticatedUserCredentials(
			string appToken,
			string consumerKey,
			string consumerSecret,
			string callBackUrl,
			string oauthEndPoint,
			string getRequestTokenUrl,
			string getAccessTokenUrl,
			string authorizeUrl )
		{
			this.AppToken = appToken;
			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
			this.CallbackUrl = callBackUrl;
			this.OauthEndPoint = oauthEndPoint;
			this.GetRequestTokenUrl = getRequestTokenUrl;
			this.GetAccessTokenUrl = getAccessTokenUrl;
			this.AuthorizeUrl = authorizeUrl;
		}

		public QuickBooksOnlineNonAuthenticatedUserCredentials(
			string appToken,
			string consumerKey,
			string consumerSecret,
			string callBackUrl,
			string oauthEndPoint = "https://oauth.intuit.com/oauth/v1",
			string authorizeUrl = "https://workplace.intuit.com/Connect/Begin" )
			: this(
				appToken,
				consumerKey,
				consumerSecret,
				callBackUrl,
				oauthEndPoint,
				oauthEndPoint + "/get_request_token",
				oauthEndPoint + "/get_access_token",
				authorizeUrl
				)
		{
		}

		public string AppToken { get; private set; }
		public string ConsumerKey { get; private set; }
		public string ConsumerSecret { get; private set; }
		public string CallbackUrl { get; private set; }
		public string OauthEndPoint { get; private set; }
		public string GetRequestTokenUrl { get; private set; }
		public string GetAccessTokenUrl { get; private set; }
		public string AuthorizeUrl { get; private set; }

		public static int ParseQBDataSource( string str )
		{
			int res;
			switch( str.ToLower() )
			{
				case "qbo":
					res = ( int )IntuitServicesType.QBO;
					break;
				case "qbd":
					res = ( int )IntuitServicesType.QBD;
					break;
				case "ips":
					res = ( int )IntuitServicesType.IPS;
					break;
				default:
					res = ( int )IntuitServicesType.None;
					break;
			}

			return res;
		}
	}
}