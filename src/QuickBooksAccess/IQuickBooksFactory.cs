using QuickBooksAccess.Models;

namespace QuickBooksAccess
{
	public interface IQuickBooksFactory
	{
		IQuickBooksService CreateService( QuickBooksAuthenticatedUserCredentials userAuthCredentials );
	}
}