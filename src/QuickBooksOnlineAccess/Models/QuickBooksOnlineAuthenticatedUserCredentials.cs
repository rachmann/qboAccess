namespace QuickBooksOnlineAccess.Models
{
	public class QuickBooksOnlineAuthenticatedUserCredentials
	{
		public QuickBooksOnlineAuthenticatedUserCredentials(
			string realmId,
			string accessToken,
			string accessTokenSecret,
			int dataSource = 1
			)
		{
			this.RealmId = realmId;
			this.OAuthAccessToken = accessToken;
			this.OAuthAccessTokenSecret = accessTokenSecret;
			this.DataSource = dataSource;
		}

		public int DataSource { get; set; }
		public string OAuthAccessToken { get; set; }
		public string OAuthAccessTokenSecret { get; set; }
		public string RealmId { get; set; }
	}
}