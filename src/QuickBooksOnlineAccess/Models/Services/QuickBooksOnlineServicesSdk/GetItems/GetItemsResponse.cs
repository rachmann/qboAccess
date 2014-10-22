using System.Collections.Generic;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetItems
{
	internal class GetItemsResponse
	{
		public GetItemsResponse( List< Item > items )
		{
			this.Items = items;
		}

		public List< Item > Items { get; set; }
	}
}