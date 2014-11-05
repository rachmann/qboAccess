using System;
using System.Collections.Generic;
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

		public string DocNumber { get; set; }
		public string VendorName { get; set; }
		public DateTime TnxDate { get; set; }
		public PoStatusEnum PoStatus { get; set; }
		public IEnumerable< OrderLineItem > LineItems { get; set; }
		public string VendorId { get; set; }
	}

	public enum PoStatusEnum
	{
		Unknown,
		Open,
		Closed
	}
}