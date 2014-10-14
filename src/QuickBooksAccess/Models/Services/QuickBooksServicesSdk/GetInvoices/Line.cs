namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetInvoices
{
	public class InvoiceLine
	{
		public string Id { get; set; }
		public decimal Amount { get; set; }
		public string Description { get; set; }
		public string LineNum { get; set; }
		public decimal Qty { get; set; }
	}
}