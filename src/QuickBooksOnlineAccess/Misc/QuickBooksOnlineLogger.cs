using System;
using Netco.Logging;

namespace QuickBooksOnlineAccess.Misc
{
	internal class QuickBooksOnlineLogger
	{
		public static ILogger Log()
		{
			return NetcoLogger.GetLogger( "QuickBooksOnlineLogger" );
		}

		public static void LogTraceException( Exception exception )
		{
			Log().Trace( exception, "[qbo] An exception occured." );
		}

		public static void LogTraceStarted( string info )
		{
			Log().Trace( "[qbo] Start call:{0}.", info );
		}

		public static void LogTraceEnded( string info )
		{
			Log().Trace( "[qbo] End call:{0}.", info );
		}

		public static void LogTrace( string info )
		{
			Log().Trace( "[qbo] Trace info:{0}.", info );
		}
	}
}