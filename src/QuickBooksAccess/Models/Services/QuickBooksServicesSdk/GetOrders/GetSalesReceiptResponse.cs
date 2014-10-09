using System.Collections.Generic;
using Intuit.Ipp.Data;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetOrders
{
	internal class GetSalesReceiptResponse
	{
		public GetSalesReceiptResponse( List< SalesOrder > orders )
		{
			this.Orders = orders;
		}

		public GetSalesReceiptResponse( List< SalesTransaction > ordersFilteredFromAndTo )
		{
			this.Orders = ordersFilteredFromAndTo;
		}

		public GetSalesReceiptResponse( List< SalesReceipt > ordersFilteredFromAndTo )
		{
			this.Orders = ordersFilteredFromAndTo;
		}

		public IEnumerable< object > Orders { get; set; }
	}
}