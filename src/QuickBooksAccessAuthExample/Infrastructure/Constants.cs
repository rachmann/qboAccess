using System.Configuration;

namespace QuickBooksAccessAuthExample.Infrastructure
{
	public class Constants
	{
		public static class OauthEndPoints
		{
			public static string IdFedOAuthBaseUrl = ConfigurationManager.AppSettings["Intuit_OAuth_BaseUrl"] != null ?
				ConfigurationManager.AppSettings["Intuit_OAuth_BaseUrl"].ToString() : "https://oauth.intuit.com/oauth/v1";


			public static string UrlRequestToken = ConfigurationManager.AppSettings["Url_Request_Token"] != null ?
				ConfigurationManager.AppSettings["Url_Request_Token"].ToString() : "/get_request_token";


			public static string UrlAccessToken = ConfigurationManager.AppSettings["Url_Access_Token"] != null ?
				ConfigurationManager.AppSettings["Url_Access_Token"].ToString() : "/get_access_token";

			public static string AuthorizeUrl = ConfigurationManager.AppSettings["Intuit_Workplace_AuthorizeUrl"] != null ?
				ConfigurationManager.AppSettings["Intuit_Workplace_AuthorizeUrl"].ToString() : "https://workplace.intuit.com/Connect/Begin";
        

		}
	}
}