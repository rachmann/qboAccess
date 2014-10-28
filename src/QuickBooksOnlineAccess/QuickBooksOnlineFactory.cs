using QuickBooksOnlineAccess.Models;

namespace QuickBooksOnlineAccess
{
	public class QuickBooksOnlineFactory : IQuickBooksOnlineFactory
	{
		private readonly QuickBooksOnlineNonAuthenticatedUserCredentials _nonAuthenticatedQuickBooksOnlineNonAuthenticatedUserCredentials;

		public IQuickBooksOnlineService CreateService( QuickBooksOnlineAuthenticatedUserCredentials userAuthCredentials )
		{
			return new QuickBooksOnlineService( userAuthCredentials, this._nonAuthenticatedQuickBooksOnlineNonAuthenticatedUserCredentials );
		}

		public QuickBooksOnlineFactory( QuickBooksOnlineNonAuthenticatedUserCredentials quickBooksOnlineNonAuthenticatedUserCredentials )
		{
			this._nonAuthenticatedQuickBooksOnlineNonAuthenticatedUserCredentials = quickBooksOnlineNonAuthenticatedUserCredentials;
		}
	}
}