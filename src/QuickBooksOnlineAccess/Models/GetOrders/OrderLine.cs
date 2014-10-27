namespace QuickBooksOnlineAccess.Models.GetOrders
{
	public class OrderLine
	{
		public decimal Amount { get; set; }
		public string Description { get; set; }
		public string Id { get; set; }
		public string LineNum { get; set; }
		public decimal Qty { get; set; }
		public string Sku { get; set; }
	}
}