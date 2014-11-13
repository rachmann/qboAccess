using System;
using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders
{
	internal class PurchaseOrder
	{
		public string DocNumber { get; set; }
		public DateTime TnxDate { get; set; }
		public IEnumerable< PurchaseOrdeLineItem > LineItems { get; set; }
		public string SyncToken { get; set; }
		public string VendorName { get; set; }
		public string VendorValue { get; set; }
		public QBPurchaseOrderStatusEnum PoStatus { get; set; }
		public string Id { get; set; }
	}

	internal enum QBPurchaseOrderStatusEnum
	{
		Unknown,
		Open,
		Closed
	}
}