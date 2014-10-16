namespace QuickBooksAccess.Models
{
	public class QuickBooksAuthenticatedUserCredentials
	{
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