using System;

namespace QuickBooksOnlineAccess
{
	public class QuickBooksOnlineAuthException : QuickBooksOnlineException
	{
		public QuickBooksOnlineAuthException( string message, Exception exception )
			: base( message, exception )
		{
		}
	}
}