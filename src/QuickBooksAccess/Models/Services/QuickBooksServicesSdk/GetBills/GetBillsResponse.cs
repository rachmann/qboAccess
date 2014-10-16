using System.Collections.Generic;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetBills
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