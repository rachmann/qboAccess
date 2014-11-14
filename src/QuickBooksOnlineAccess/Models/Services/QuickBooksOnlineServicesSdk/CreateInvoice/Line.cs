namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice
{
	public class Line
	{
		public int Qty { get; set; }
		public string ItemValue { get; set; }
		public string ItemName { get; set; }
		public decimal Amount { get; set; }
	}
}