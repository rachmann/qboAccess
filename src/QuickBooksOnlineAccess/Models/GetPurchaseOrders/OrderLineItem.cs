using System;
using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.GetPurchaseOrders
{
	public class OrderLineItem : IManualSerializable
	{
		public string Id { get; set; }
		public decimal Qty { get; set; }
		public decimal Amount { get; set; }
		public string ItemName { get; set; }
		public string LineNum { get; set; }
		public decimal Rate { get; set; }

		public string ToJson()
		{
			try
			{
				var res = string.Format( "Id:{0}, Qty:{1}, Amount:{2}", this.Id, this.Qty, this.Amount );

				return res;
			}
			catch( Exception )
			{
				return PredefinedValues.EmptyJsonObject;
			}
		}
	}
}