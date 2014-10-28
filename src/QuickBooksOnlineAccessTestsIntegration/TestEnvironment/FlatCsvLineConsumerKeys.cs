using LINQtoCSV;

namespace QuickBooksOnlineAccessTestsIntegration.TestEnvironment
{
	internal class FlatCsvLineConsumerKeys
	{
		public FlatCsvLineConsumerKeys()
		{
		}

		[ CsvColumn( Name = "ConsumerKey", FieldIndex = 1 ) ]
		public string ConsumerKey { get; set; }

		[ CsvColumn( Name = "ConsumerSecret", FieldIndex = 2 ) ]
		public string ConsumerSecretKey { get; set; }
	}
}