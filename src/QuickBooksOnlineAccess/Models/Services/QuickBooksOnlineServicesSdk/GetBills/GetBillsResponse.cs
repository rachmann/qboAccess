using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetBills
{
	internal class GetBillsResponse
	{
		public GetBillsResponse( IEnumerable< Bill > billsFilteredFromAndTo )
		{
			this.Bills = billsFilteredFromAndTo;
		}

		public IEnumerable< Bill > Bills { get; set; }
	}
}