using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts
{
	internal class SaleReceipt
	{
		public string DocNumber { get; set; }
		public IEnumerable< Line > Line { get; set; }
		public string CustomerValue { get; set; }
		public string CustomerName { get; set; }
	}
}