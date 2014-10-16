using System;

namespace QuickBooksAccess
{
	public class QuickBooksException : Exception
	{
		public QuickBooksException( string message, Exception exception )
			: base( message, exception )
		{
		}
	}
}