using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.GetProducts
{
	public class Product : IManualSerializable
	{
		public decimal QtyOnHand { get; set; }
		public string Name { get; set; }
		public string Id { get; set; }
		public string SyncToken { get; set; }
		public string IncomeAccRefValue { get; set; }
		public string IncomeAccRefName { get; set; }
		public string IncomeAccRefType { get; set; }
		public string ExpenseAccRefValue { get; set; }
		public string ExpenseAccRefName { get; set; }
		public string ExpenseAccRefType { get; set; }

		public string ToJson()
		{
			var res = string.Format( "{{Id:{0},Name:{1},QtyOnHand:{2}}}", this.Id, this.Name, this.QtyOnHand );
			return res;
		}
	}
}