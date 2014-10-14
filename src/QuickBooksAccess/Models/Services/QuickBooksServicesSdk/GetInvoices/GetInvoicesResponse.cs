using System.Collections.Generic;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetInvoices
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