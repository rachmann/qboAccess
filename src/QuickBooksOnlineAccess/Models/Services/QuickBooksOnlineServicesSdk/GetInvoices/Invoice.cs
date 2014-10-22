using System;
using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices
{
	public class Invoice
	{
		public IEnumerable< InvoiceLine > Line { get; set; }
		public string Id { get; set; }
		public string DocNumber { get; set; }
		public string NotAvailable { get; set; }
		public decimal TotalAmt { get; set; }
		public string SyncToken { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string CountryCode { get; set; }
		public string PostalCode { get; set; }
		public string PostalCodeSuffix { get; set; }
		public DateTime ShipDate { get; set; }
		public decimal Deposit { get; set; }
		public string TrackingNum { get; set; }
	}
}