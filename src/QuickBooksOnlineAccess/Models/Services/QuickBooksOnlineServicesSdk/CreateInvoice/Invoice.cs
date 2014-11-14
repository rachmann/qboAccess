using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice
{
	internal class Invoice
	{
		public string DocNumber { get; set; }
		public IEnumerable< Line > Line { get; set; }
		public string CustomerValue { get; set; }
		public string CustomerName { get; set; }
	}
}