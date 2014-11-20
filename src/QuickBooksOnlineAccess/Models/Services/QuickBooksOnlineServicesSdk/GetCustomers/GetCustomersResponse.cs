using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetCustomers
{
	internal class GetCustomersResponse
	{
		public GetCustomersResponse( IEnumerable< Customer > customers )
		{
			this.Customers = customers;
		}

		public IEnumerable< Customer > Customers { get; set; }
	}
}