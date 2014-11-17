namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreatePurchaseOrders
{
	internal class PurchaseOrdeLineItem
	{
		//public decimal Amount { get; set; }
		public decimal Qty { get; set; }
		//public string Id { get; set; }
		//public string LineNum { get; set; }
		public decimal UnitPrice { get; set; }
		public string ItemName { get; set; }
		public string ItemValue { get; set; }
	}
}