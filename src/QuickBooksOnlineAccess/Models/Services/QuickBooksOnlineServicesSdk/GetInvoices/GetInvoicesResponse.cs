using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices
{
	internal class GetInvoicesResponse
	{
		public GetInvoicesResponse( List< Invoice > invoicesFilteredFromAndTo )
		{
			this.Invoices = invoicesFilteredFromAndTo;
		}

		public List< Invoice > Invoices { get; set; }
	}
}