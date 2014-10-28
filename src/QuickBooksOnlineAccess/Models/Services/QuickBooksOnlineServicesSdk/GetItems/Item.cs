namespace QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetItems
{
	internal class Item
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public decimal Qty { get; set; }
		public string SyncToken { get; set; }
		public string ExpenseAccRefType { get; set; }
		public string ExpenseAccRefName { get; set; }
		public string ExpenseAccRefValue { get; set; }
		public string IncomeAccRefType { get; set; }
		public string IncomeAccRefName { get; set; }
		public string IncomeAccRefValue { get; set; }
	}
}