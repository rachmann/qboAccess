using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts
{
	internal class GetSalesReceiptsResponse
	{
		public GetSalesReceiptsResponse( IEnumerable< SalesReceipt > ordersFilteredFromAndTo )
		{
			this.Orders = ordersFilteredFromAndTo;
		}

		public IEnumerable< SalesReceipt > Orders { get; set; }
	}
}