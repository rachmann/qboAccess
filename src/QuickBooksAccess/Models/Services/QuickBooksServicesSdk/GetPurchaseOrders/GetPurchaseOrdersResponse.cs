using System.Collections.Generic;
using Intuit.Ipp.Data;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPurchaseOrders
{
	internal class GetPurchaseOrdersResponse
	{
		public GetPurchaseOrdersResponse( List< PurchaseOrder > purchaseOrders )
		{
			this.PurchaseOrders = purchaseOrders;
		}

		public List< PurchaseOrder > PurchaseOrders { get; set; }
	}
}