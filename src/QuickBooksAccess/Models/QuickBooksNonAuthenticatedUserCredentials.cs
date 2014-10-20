namespace QuickBooksAccess.Models
{
	public class QuickBooksNonAuthenticatedUserCredentials
	{
		public QuickBooksNonAuthenticatedUserCredentials(
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

		public QuickBooksNonAuthenticatedUserCredentials(
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
	}
}