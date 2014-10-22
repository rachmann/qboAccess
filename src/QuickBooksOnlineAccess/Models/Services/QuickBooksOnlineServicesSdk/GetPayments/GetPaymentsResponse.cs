using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments
{
	internal class GetPaymentsResponse
	{
		public GetPaymentsResponse( List< Payment > itemsConvertedToQbAccessItems )
		{
			this.Payments = itemsConvertedToQbAccessItems;
		}

		public List< Payment > Payments { get; set; }
	}
}