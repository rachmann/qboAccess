namespace QuickBooksOnlineAccess.Models.UpdateInventory
{
	public class Inventory
	{
		public string ProductId { get; set; }
		public long Quantity { get; set; }
		public string NameOrSku { get; set; }
	}
}