using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders
{
	internal class GetPurchaseOrdersResponse
	{
		public GetPurchaseOrdersResponse( IEnumerable< PurchaseOrder > purchaseOrders )
		{
			this.PurchaseOrders = purchaseOrders;
		}

		public IEnumerable< PurchaseOrder > PurchaseOrders { get; set; }
	}
}