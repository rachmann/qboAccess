using System.Linq;
using LINQtoCSV;
using QuickBooksAccess.Misc;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.Auth;

namespace QuickBooksAccessTestsIntegration.TestEnvironment
{
	public class TestDataReader
	{
		private readonly string _consumerKeysFilePath;
		private readonly string _appKeysFilePath;
		private FlatCsvLineConsumerKeys _flatCsvLineConsumerKeys;
		private FlatCsvLineAppKeys _flatCsvLineAppKeys;

		public ConsumerProfile ConsumerProfile
		{
			get
			{
				return new ConsumerProfile()
				{
					ConsumerKey = this._flatCsvLineConsumerKeys.ConsumerKey,
					ConsumerSecret = this._flatCsvLineConsumerKeys.ConsumerSecretKey,
				};
			}
		}

		public RestProfile RestProfile
		{
			get
			{
				return new RestProfile()
				{
					AppToken = this._flatCsvLineAppKeys.AppToken,
					CompanyId = this._flatCsvLineAppKeys.CompanyId,
					DataSource = this._flatCsvLineAppKeys.DataSource.ToIntOrDefault(),
					OAuthAccessToken = this._flatCsvLineAppKeys.OAuthAccessToken,
					OAuthAccessTokenSecret = this._flatCsvLineAppKeys.OAuthAccessTokenSecret,
					RealmId = this._flatCsvLineAppKeys.RealmId
				};
			}
		}

		public TestDataReader( string consumerKeysFilePath, string appKeysFilePath )
		{
			this._consumerKeysFilePath = consumerKeysFilePath;
			this._appKeysFilePath = appKeysFilePath;

			this.ReadData();
		}

		public void ReadData()
		{
			var cc = new CsvContext();

			this._flatCsvLineConsumerKeys = Enumerable.FirstOrDefault( cc.Read< FlatCsvLineConsumerKeys >( this._consumerKeysFilePath, new CsvFileDescription { FirstLineHasColumnNames = true } ) );
			this._flatCsvLineAppKeys = Enumerable.FirstOrDefault( cc.Read< FlatCsvLineAppKeys >( this._appKeysFilePath, new CsvFileDescription { FirstLineHasColumnNames = true } ) );
		}
	}
}