using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments
{
	public class Payment
	{
		public string Id { get; set; }
		public string DocNumber { get; set; }
		public decimal TotalAmt { get; set; }
		public string SyncToken { get; set; }
		public IEnumerable< Line > Line { get; set; }
	}
}