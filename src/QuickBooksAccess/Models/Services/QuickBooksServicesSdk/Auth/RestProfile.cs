namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth
{
	public class RestProfile
	{
		private string _oAuthAccessToken { get; set; }
		private string _oAuthAccessTokenSecret { get; set; }
		private string _realmId { get; set; }
		private string _appToken { get; set; }
		private string _companyId { get; set; }
		private int _dataSource { get; set; }

		public string AppToken
		{
			get { return this._appToken; }
			set { this._appToken = value; }
		}

		public string CompanyId
		{
			get { return this._companyId; }
			set { this._companyId = value; }
		}

		public string RealmId
		{
			get { return this._realmId; }
			set { this._realmId = value; }
		}

		public string OAuthAccessToken
		{
			get { return this._oAuthAccessToken; }
			set { this._oAuthAccessToken = value; }
		}

		public string OAuthAccessTokenSecret
		{
			get { return this._oAuthAccessTokenSecret; }
			set { this._oAuthAccessTokenSecret = value; }
		}

		public int DataSource
		{
			get
			{
				object dataSource = this._dataSource;
				if( !dataSource.Equals( null ) )
					return ( int )dataSource;
				else
					return -1;
			}
			set { this._dataSource = value; }
		}
	}
}