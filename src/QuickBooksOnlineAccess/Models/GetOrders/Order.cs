using System;
using System.Collections.Generic;
using QuickBooksOnlineAccess.Misc;

namespace QuickBooksOnlineAccess.Models.GetOrders
{
	public class Order : IManualSerializable
	{
		public OrderType OrderType { get; set; }
		public string OrderId { get; set; }
		public string Currency { get; set; }
		public string DocNumber { get; set; }
		public string ShipCity { get; set; }
		public string ShipCountry { get; set; }
		public string ShipCountryCode { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipPostalCodeSuffix { get; set; }
		public decimal Deposit { get; set; }
		public decimal Balance { get; set; }
		public string TrackingNum { get; set; }
		public string SyncToken { get; set; }
		public DateTime ShipDate { get; set; }
		public decimal TotalAmt { get; set; }
		public IEnumerable< OrderLine > Line { get; set; }
		public DateTime CreateTime { get; set; }
		public string CustomerName { get; set; }
		public string CustomerValue { get; set; }

		public OrderStatus GetOrderStatus()
		{
			//todo: implement
			return OrderStatus.Unknown;
		}

		public OrderPaymentStatus GetOrderPaymentStatus()
		{
			if( this.OrderType == OrderType.Invoice )
			{
				if( this.Balance < PredefinedValues.Eps )
					return OrderPaymentStatus.FullyPaid;
				if( this.TotalAmt - this.Balance < PredefinedValues.Eps )
					return OrderPaymentStatus.Unpaid;
				if( this.Balance > PredefinedValues.Eps )
					return OrderPaymentStatus.PartiallyPaid;
				return OrderPaymentStatus.Unknown;
			}

			if( this.OrderType == OrderType.SalesReceipt )
				return OrderPaymentStatus.FullyPaid;

			return OrderPaymentStatus.Unknown;
		}

		public string ToJson()
		{
			var res = string.Format( "{{OrderId:{0},DocNumber:{1},OrderType:{2}}}", this.OrderId, this.DocNumber, this.OrderType );
			return res;
		}
	}

	public enum OrderStatus
	{
		Unknown,
		Paid
	}

	public enum OrderPaymentStatus
	{
		Unknown,
		FullyPaid,
		Unpaid,
		PartiallyPaid,
	}

	public enum OrderType
	{
		Unknown,
		Invoice,
		SalesReceipt
	}
}