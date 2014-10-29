using System;
using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices
{
	public class Invoice
	{
		public IEnumerable< InvoiceLine > Line { get; set; }
		public string Id { get; set; }
		public string DocNumber { get; set; }
		public string Currency { get; set; }
		public decimal TotalAmt { get; set; }
		public string SyncToken { get; set; }
		public string ShipCity { get; set; }
		public string ShipCountry { get; set; }
		public string ShipCountryCode { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipPostalCodeSuffix { get; set; }
		public DateTime ShipDate { get; set; }
		public decimal Deposit { get; set; }
		public string TrackingNum { get; set; }
		public DateTime CreateTime { get; set; }
		public decimal Balance { get; set; }
	}
}