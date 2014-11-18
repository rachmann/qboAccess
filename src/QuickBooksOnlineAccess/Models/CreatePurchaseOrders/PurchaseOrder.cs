using System;
using System.Collections.Generic;
using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.CreatePurchaseOrders
{
	public class PurchaseOrder : IManualSerializable
	{
		public string ToJson()
		{
			try
			{
				var res = String.Format( "{{DocNumber:{0}, VendorId:{1}, TnxDate:{2}, PoStatus:{3}, LineItems:{4}}}", this.DocNumber, this.VendorValue, this.TnxDate, this.PoStatus, this.LineItems.ToJson() );
				return res;
			}
			catch
			{
				return PredefinedValues.EmptyJsonObject;
			}
		}

		public string DocNumber { get; set; }
		public string VendorName { get; set; }
		public DateTime TnxDate { get; set; }
		public PoStatusEnum PoStatus { get; set; }
		public IEnumerable< OrderLineItem > LineItems { get; set; }
		public string VendorValue { get; set; }
	}

	public enum PoStatusEnum
	{
		Unknown,
		Open,
		Closed
	}
}