using System;

namespace QuickBooksAccess
{
	public class QuickBooksAuthException : QuickBooksException
	{
		public QuickBooksAuthException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}