using LINQtoCSV;

namespace QuickBooksOnlineAccessTestsIntegration.TestEnvironment
{
	internal class FlatCsvLineAppKeys
	{
		public FlatCsvLineAppKeys()
		{
		}

		[ CsvColumn( Name = "RealmId", FieldIndex = 1 ) ]
		public string RealmId { get; set; }

		[ CsvColumn( Name = "DataSource", FieldIndex = 2 ) ]
		public string DataSource { get; set; }

		[ CsvColumn( Name = "AppToken", FieldIndex = 3 ) ]
		public string AppToken { get; set; }

		[ CsvColumn( Name = "OAuthAccessToken", FieldIndex = 4 ) ]
		public string OAuthAccessToken { get; set; }

		[ CsvColumn( Name = "OAuthAccessTokenSecret", FieldIndex = 5 ) ]
		public string OAuthAccessTokenSecret { get; set; }
	}
}