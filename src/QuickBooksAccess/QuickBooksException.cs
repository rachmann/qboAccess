using System;

namespace QuickBooksAccess
{
	public class QuickBooksException : Exception
	{
		protected QuickBooksException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}