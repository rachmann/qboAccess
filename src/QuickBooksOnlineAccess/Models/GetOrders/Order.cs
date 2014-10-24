using System;
using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.GetOrders
{
	public class Order
	{
		public OrderType OrderType { get; set; }
		public string OrderId { get; set; }
		public string Currency { get; set; }
		public string DocNumber { get; set; }
		public string ShipCity { get; set; }
		public string ShipCountry { get; set; }
		public string ShipCountryCode { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipPostalCodeSuffix { get; set; }
		public decimal Deposit { get; set; }
		public string TrackingNum { get; set; }
		public string SyncToken { get; set; }
		public DateTime ShipDate { get; set; }
		public decimal TotalAmt { get; set; }
		public IEnumerable< OrderLine > Line { get; set; }
		public DateTime CreateTime { get; set; }
	}

	public enum OrderType
	{
		Unknown,
		Invoice,
		SalesReceipt
	}
}