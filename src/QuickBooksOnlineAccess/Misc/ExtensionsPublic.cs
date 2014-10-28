using System.Collections.Generic;
using System.Linq;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.UpdateInventory;

namespace QuickBooksOnlineAccess.Misc
{
	public static class ExtensionsPublic
	{
		#region FromPublicService
		public static IEnumerable< Inventory > ToQBInventoryItem( this IEnumerable< Product > source )
		{
			var orders = source.Select( x => x.ToQBInventory() );
			return orders;
		}

		public static Inventory ToQBInventory( this Product source )
		{
			var order = new Inventory()
			{
				NameOrSku = source.Name,
				ProductId = source.Id,
				Quantity = source.QtyOnHand,
				SyncToken = source.SyncToken,
				IncomeAccRefValue = source.IncomeAccRefValue,
				IncomeAccRefName = source.IncomeAccRefName,
				IncomeAccRefType = source.IncomeAccRefType,
				ExpenseAccRefValue = source.ExpenseAccRefValue,
				ExpenseAccRefName = source.ExpenseAccRefName,
				ExpenseAccRefType = source.ExpenseAccRefType
			};
			return order;
		}
		#endregion
	}
}