namespace QuickBooksOnlineAccess.Models.GetPurchaseOrders
{
	public class OrderLineItem
	{
		public string Id { get; set; }
		public decimal Qty { get; set; }
		public decimal Amount { get; set; }
		public string ItemName { get; set; }
		public string LineNum { get; set; }
		public decimal Rate { get; set; }
	}
}