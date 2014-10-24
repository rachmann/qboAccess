namespace QuickBooksOnlineAccess.Models.GetOrders
{
	public class Order
	{
		public OrderType OrderType { get; set; }
		public string OrderId { get; set; }
	}

	public enum OrderType
	{
		Unknown,
		Invoice,
		SalesReceipt
	}
}