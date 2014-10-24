namespace QuickBooksOnlineAccess.Models
{
	public class QuickBooksOnlineAuthenticatedUserCredentials
	{
		public QuickBooksOnlineAuthenticatedUserCredentials(
			string realmId,
			string accessToken,
			string consumerTokenSecret )
		{
			this.RealmId = realmId;
			this.OAuthAccessToken = accessToken;
			this.OAuthAccessTokenSecret = this.OAuthAccessTokenSecret;
		}

		public string AppToken { get; set; }
		public string CompanyId { get; set; }
		public int DataSource { get; set; }
		public string OAuthAccessToken { get; set; }
		public string OAuthAccessTokenSecret { get; set; }
		public string RealmId { get; set; }
		public string ConsumerSecret { get; set; }
		public string ConsumerKey { get; set; }
	}
}