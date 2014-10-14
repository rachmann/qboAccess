using System.Collections.Generic;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPayments
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