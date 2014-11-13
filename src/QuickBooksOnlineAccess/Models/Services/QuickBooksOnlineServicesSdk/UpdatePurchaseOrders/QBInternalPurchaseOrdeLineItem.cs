namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders
{
	internal class QBInternalPurchaseOrdeLineItem
	{
		public string Id { get; set; }
		public decimal Qty { get; set; }
		public decimal Rate { get; set; }
		public string LineNum { get; set; }
		public decimal Amount { get; set; }
		public string ItemName { get; set; }
		public string ItemValue { get; set; }
	}
}