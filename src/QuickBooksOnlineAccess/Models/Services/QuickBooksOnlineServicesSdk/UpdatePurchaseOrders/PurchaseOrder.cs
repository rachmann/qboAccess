using System.Collections.Generic;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders
{
	internal class PurchaseOrder
	{
		public QBInternalPurchaseOrderStatusEnum POStatus { get; set; }
		public string DocNumber { get; set; }
		public string Id { get; set; }
		public string SyncToken { get; set; }
		public IEnumerable< QBInternalPurchaseOrdeLineItem > LineItems { get; set; }
		public string VendorName { get; set; }
		public string VendorValue { get; set; }
	}

	internal enum QBInternalPurchaseOrderStatusEnum
	{
		Unknown,
		Open,
		Closed
	}
}