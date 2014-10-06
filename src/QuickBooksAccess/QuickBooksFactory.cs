using System;
using QuickBooksAccess.Models;

namespace QuickBooksAccess
{
	public class QuickBooksFactory : IQuickBooksFactory
	{
		public IQuickBooksService CreateService( QuickBooksAuthenticatedUserCredentials userAuthCredentials )
		{
			//todo: replace me
			throw new NotImplementedException();
		}
	}
}