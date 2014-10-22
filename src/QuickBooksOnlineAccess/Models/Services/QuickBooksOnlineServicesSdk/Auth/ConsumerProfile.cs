namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.Auth
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

		public string ToJson()
		{
			var str = string.Format(
				"{{ConsumerKey:{0},ConsumerSecret:{1}}}",
				this.ConsumerKey,
				this.ConsumerSecret
				);

			return str;
		}
	}
}