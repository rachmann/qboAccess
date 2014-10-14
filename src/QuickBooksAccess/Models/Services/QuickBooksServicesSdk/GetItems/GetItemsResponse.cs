using System.Collections.Generic;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetItems
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