using System;
using System.Collections.Generic;
using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.CreateOrders
{
	public class Order : IManualSerializable
	{
		public string ToJson()
		{
			try
			{
				var res = String.Format( "{{DocNumber:{0}, CustomerId:{1}, CustomerName:{2}, TnxDate:{3}, OrderStatus:{4}, LineItems:{5}}}", this.DocNumber, this.CustomerValue, this.TnxDate, this.OrderStatus, this.LineItems.ToJson() );
				return res;
			}
			catch
			{
				return PredefinedValues.EmptyJsonObject;
			}
		}

		public string DocNumber { get; set; }
		public string CustomerName { get; set; }
		public DateTime TnxDate { get; set; }
		public OrderStatusEnum OrderStatus { get; set; }
		public IEnumerable< OrderLineItem > LineItems { get; set; }
		public string CustomerValue { get; set; }
	}

	public enum OrderStatusEnum
	{
		Unknown,
		Paid,
		Unpiad
	}
}