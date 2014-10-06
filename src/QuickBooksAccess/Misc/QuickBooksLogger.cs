using Netco.Logging;

namespace QuickBooksAccess.Misc
{
	internal class QuickBooksLogger
	{
		public static ILogger Log()
		{
			return NetcoLogger.GetLogger( "MagentoLogger" );
		}
	}
}