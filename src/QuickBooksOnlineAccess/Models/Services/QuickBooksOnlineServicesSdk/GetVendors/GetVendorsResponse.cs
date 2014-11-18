using System.Collections.Generic;
using QuickBooksOnlineAccess.Services;

namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetVendors
{
	internal class GetVendorsResponse
	{
		public GetVendorsResponse( IEnumerable< Vendor > vendors )
		{
			this.Vendors = vendors;
		}

		public IEnumerable<Vendor> Vendors{get;set;} 
	}
}