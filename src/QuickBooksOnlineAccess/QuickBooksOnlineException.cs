using System;

namespace QuickBooksOnlineAccess
{
	public class QuickBooksOnlineException : Exception
	{
		public QuickBooksOnlineException( string message, Exception exception )
			: base( message, exception )
		{
		}
	}
}