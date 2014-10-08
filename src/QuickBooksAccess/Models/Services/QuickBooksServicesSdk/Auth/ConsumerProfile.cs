namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth
{
	public class ConsumerProfile
	{
		private string _consumerKey { get; set; }
		private string _consumerSecret { get; set; }

		public string ConsumerKey
		{
			get { return this._consumerKey; }
			set { this._consumerKey = value; }
		}

		public string ConsumerSecret
		{
			get { return this._consumerSecret; }
			set { this._consumerSecret = value; }
		}
	}
}