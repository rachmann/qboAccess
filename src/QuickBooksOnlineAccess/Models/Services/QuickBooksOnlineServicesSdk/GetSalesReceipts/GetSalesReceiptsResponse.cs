using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts
{
	internal class GetSalesReceiptsResponse
	{
		public GetSalesReceiptsResponse( IEnumerable< SalesReceipt > salesReceipts )
		{
			this.SaleReceipts = salesReceipts;
		}

		public IEnumerable< SalesReceipt > SaleReceipts { get; set; }
	}
}