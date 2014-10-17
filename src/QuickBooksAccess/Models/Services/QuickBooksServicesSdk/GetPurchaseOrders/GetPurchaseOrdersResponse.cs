using System.Collections.Generic;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPurchaseOrders
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