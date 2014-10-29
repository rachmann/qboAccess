using System;
using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.UpdateInventory
{
	public class Inventory : IManualSerializable
	{
		public string ProductId { get; set; }
		public decimal Quantity { get; set; }
		public string NameOrSku { get; set; }
		public string SyncToken { get; set; }
		public string IncomeAccRefValue { get; set; }
		public string IncomeAccRefName { get; set; }
		public string IncomeAccRefType { get; set; }
		public string ExpenseAccRefValue { get; set; }
		public string ExpenseAccRefName { get; set; }
		public string ExpenseAccRefType { get; set; }

		public string ToJson()
		{
			try
			{
				var result = String.Format( "{{Id:{0},NameOrSku:{1},Quantity:{2}}}", this.ProductId, this.NameOrSku, this.Quantity );
				return result;
			}
			catch( Exception )
			{
				return PredefinedValues.EmptyJsonObject;
			}
		}
	}
}