using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickBooksOnlineAccess.Models.GetOrders;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.GetPurchaseOrders;
using QuickBooksOnlineAccess.Models.Ping;
using QuickBooksOnlineAccess.Models.UpdateInventory;

namespace QuickBooksOnlineAccess
{
	public interface IQuickBooksOnlineService
	{
		Task< IEnumerable< PurchaseOrder > > GetPurchaseOrdersOrdersAsync( DateTime dateFrom, DateTime dateTo );

		Task CreatePurchaseOrdersOrdersAsync( params Models.CreatePurchaseOrders.PurchaseOrder[] purchaseOrders );

		Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo );

		Task< IEnumerable< Order > > GetOrdersAsync();

		Task UpdateInventoryAsync( IEnumerable< Inventory > products );

		Task< IEnumerable< Product > > GetProductsAsync();

		Task< PingInfo > Ping();

		Func< string > AdditionalLogInfo { get; set; }

		Task< IEnumerable< Order > > GetOrdersAsync( params string[] docNumbers );
		Task CreateOrdersAsync( params Models.CreateOrders.Order[] purchaseOrders );
	}
}