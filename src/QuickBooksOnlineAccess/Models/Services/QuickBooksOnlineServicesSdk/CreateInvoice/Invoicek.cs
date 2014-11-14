using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice
{
	internal class Invoicek
	{
		public string DocNumber { get; set; }
		public IEnumerable<Linek> Line { get; set; }
	}
}