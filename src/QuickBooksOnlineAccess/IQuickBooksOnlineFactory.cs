using QuickBooksOnlineAccess.Models;

namespace QuickBooksOnlineAccess
{
	public interface IQuickBooksOnlineFactory
	{
		IQuickBooksOnlineService CreateService( QuickBooksOnlineAuthenticatedUserCredentials userAuthCredentials );
	}
}