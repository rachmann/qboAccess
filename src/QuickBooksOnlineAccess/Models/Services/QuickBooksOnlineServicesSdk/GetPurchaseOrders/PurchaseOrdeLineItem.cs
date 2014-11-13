namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders
{
	internal class PurchaseOrdeLineItem
	{
		public decimal Amount { get; set; }
		public decimal Qty { get; set; }
		public string Id { get; set; }
		public string LineNum { get; set; }
		public decimal Rate { get; set; }
		public string ItemName { get; set; }
		public string ItemValue { get; set; }
	}
}