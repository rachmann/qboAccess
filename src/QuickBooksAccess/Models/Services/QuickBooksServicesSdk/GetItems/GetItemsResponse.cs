using System.Collections.Generic;
using Intuit.Ipp.Data;
using Payment = QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetPayments.Payment;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetItems
{
	internal class GetItemsResponse
	{
		public GetItemsResponse( List< Item > items )
		{
			Items = items;
		}

		public List< Item > Items { get; set; }
	}
}