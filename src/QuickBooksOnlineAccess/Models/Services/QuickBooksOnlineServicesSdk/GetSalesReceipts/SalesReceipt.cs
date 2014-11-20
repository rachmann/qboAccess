using System;
using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts
{
	internal class SalesReceipt
	{
		public string ShipCity { get; set; }
		public string ShipCountry { get; set; }
		public string ShipCountryCode { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipPostalCodeSuffix { get; set; }
		public DateTime ShipDate { get; set; }
		public string TrackingNum { get; set; }
		public string Id { get; set; }
		public string DocNumber { get; set; }
		public string Currency { get; set; }
		public decimal TotalAmt { get; set; }
		public string SyncToken { get; set; }
		public decimal Balance { get; set; }
		public string PONumber { get; set; }
		public List< SalesReceiptLine > Line { get; set; }
		public DateTime CreateTime { get; set; }
		public string CustomerName { get; set; }
		public string CustomerValue { get; set; }
	}
}