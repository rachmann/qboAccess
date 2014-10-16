using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickBooksAccess.Models.GetOrders;
using QuickBooksAccess.Models.GetProducts;
using QuickBooksAccess.Models.Ping;
using QuickBooksAccess.Models.UpdateInventory;

namespace QuickBooksAccess
{
	public interface IQuickBooksService
	{
		Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo );

		Task< IEnumerable< Order > > GetOrdersAsync();

		Task UpdateInventoryAsync( IEnumerable< Inventory > products );

		Task< IEnumerable< Product > > GetProductsAsync();

		Task< PingInfo > Ping();
	}
}