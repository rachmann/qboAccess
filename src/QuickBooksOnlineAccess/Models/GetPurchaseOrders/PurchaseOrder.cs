using System;
using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.GetPurchaseOrders
{
	public class PurchaseOrder : IManualSerializable
	{
		public string ToJson()
		{
			try
			{
				var res = String.Format( "{{}}" );
				return res;
			}
			catch
			{
				return PredefinedValues.EmptyJsonList;
			}
		}
	}
}