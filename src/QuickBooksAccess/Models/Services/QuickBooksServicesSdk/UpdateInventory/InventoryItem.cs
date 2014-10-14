namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.UpdateInventory
{
	internal class InventoryItem
	{
		public string Sku { get; set; }
		public decimal QtyOnHand { get; set; }
		public string Id { get; set; }
		public string SyncToken { get; set; }
	}
}