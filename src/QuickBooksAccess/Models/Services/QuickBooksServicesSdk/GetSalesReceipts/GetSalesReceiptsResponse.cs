﻿using System.Collections.Generic;
using Intuit.Ipp.Data;

namespace QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetSalesReceipts
{
	internal class GetSalesReceiptsResponse
	{
		public GetSalesReceiptsResponse( IEnumerable< SalesReceipt > ordersFilteredFromAndTo )
		{
			this.Orders = ordersFilteredFromAndTo;
		}

		public IEnumerable< SalesReceipt > Orders { get; set; }
	}
}