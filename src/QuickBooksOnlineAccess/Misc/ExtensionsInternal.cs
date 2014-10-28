﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Intuit.Ipp.Data;
using QuickBooksOnlineAccess.Models.GetOrders;
using QuickBooksOnlineAccess.Models.GetProducts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdateItemQuantityOnHand;
using QuickBooksOnlineAccess.Models.UpdateInventory;
using Bill = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetBills.Bill;
using Invoice = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices.Invoice;
using Item = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetItems.Item;
using Line = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments.Line;
using Payment = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments.Payment;
using PurchaseOrder = QuickBooksOnlineAccess.Models.GetPurchaseOrders.PurchaseOrder;
using SalesReceipt = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts.SalesReceipt;

namespace QuickBooksOnlineAccess.Misc
{
	internal static class ExtensionsInternal
	{
		public static string ToJson( this IEnumerable< PurchaseOrder > source )
		{
			var resultUrl = string.Empty;
			try
			{
				var purchaseOrders = source as IList< PurchaseOrder > ?? source.ToList();
				var items = string.Join( ",", purchaseOrders.Select( x => string.Format( "" ) ) );
				var res = string.Format( "{{Count:{0}, Items:[{1}]}}", purchaseOrders.Count(), items );
				return res;
			}
			catch
			{
			}

			return resultUrl;
		}

		public static ReferenceType ToReferenceType( this Account account )
		{
			if( account == null )
				account = new Account();

			var referenceType = new ReferenceType
			{
				type = account.AccountType.ToString(),
				name = account.Name,
				Value = account.Id
			};

			return referenceType;
		}

		public static PurchaseOrder ToQBPurchaseOrder( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new PurchaseOrder
			{
			};

			return qbPurchaseOrder;
		}

		#region ToQBOrder
		public static IEnumerable< Order > ToQBOrder( this IEnumerable< Invoice > source )
		{
			var orders = source.Select( x => x.ToQBOrder() );
			return orders;
		}

		public static OrderLine ToQBOrderLine( this InvoiceLine source )
		{
			var line = new OrderLine()
			{
				Amount = source.Amount,
				Description = source.Description,
				Id = source.Id,
				LineNum = source.LineNum,
				Qty = source.Qty,
				Sku = source.Sku
			};

			return line;
		}

		public static OrderLine ToQBOrderLine( this SalesReceiptLine source )
		{
			var line = new OrderLine()
			{
				Amount = source.Amount,
				Description = source.Description,
				Id = source.Id,
				LineNum = source.LineNum,
				Qty = source.Qty,
				Sku = source.Sku
			};

			return line;
		}

		public static IEnumerable< OrderLine > ToQBOrderLine( this IEnumerable< InvoiceLine > source )
		{
			var orders = source.Select( x => x.ToQBOrderLine() );

			return orders;
		}

		public static IEnumerable< OrderLine > ToQBOrderLine( this IEnumerable< SalesReceiptLine > source )
		{
			var orders = source.Select( x => x.ToQBOrderLine() );

			return orders;
		}

		public static Order ToQBOrder( this Invoice source )
		{
			var qbOrder = new Order
			{
				OrderType = OrderType.Invoice,
				OrderId = source.Id,
				Currency = source.Currency,
				DocNumber = source.DocNumber,
				ShipCity = source.ShipCity,
				ShipCountry = source.ShipCountry,
				ShipCountryCode = source.ShipCountryCode,
				ShipPostalCode = source.ShipPostalCode,
				ShipPostalCodeSuffix = source.ShipPostalCodeSuffix,
				SyncToken = source.SyncToken,
				TrackingNum = source.TrackingNum,
				Deposit = source.Deposit,
				Line = source.Line.ToQBOrderLine(),
				ShipDate = source.ShipDate,
				TotalAmt = source.TotalAmt,
				CreateTime = source.CreateTime,
			};

			return qbOrder;
		}

		public static Order ToQBOrder( this SalesReceipt source )
		{
			var qbOrder = new Order
			{
				OrderType = OrderType.SalesReceipt,
				OrderId = source.Id,
				Currency = source.Currency,
				DocNumber = source.DocNumber,
				ShipCity = source.ShipCity,
				ShipCountry = source.ShipCountry,
				ShipCountryCode = source.ShipCountryCode,
				ShipPostalCode = source.ShipPostalCode,
				ShipPostalCodeSuffix = source.ShipPostalCodeSuffix,
				SyncToken = source.SyncToken,
				TrackingNum = source.TrackingNum,
				Line = source.Line.ToQBOrderLine(),
				ShipDate = source.ShipDate,
				TotalAmt = source.TotalAmt,
				CreateTime = source.CreateTime,
			};

			return qbOrder;
		}

		public static IEnumerable< Order > ToQBOrder( this IEnumerable< SalesReceipt > source )
		{
			var orders = source.Select( x => x.ToQBOrder() );

			return orders;
		}
		#endregion

		#region FromIQuickBooksOnlineServiceInternal
		public static IEnumerable< PurchaseOrder > ToQBPurchaseOrder( this IEnumerable< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder > purchaseOrder )
		{
			if( purchaseOrder == null )
				purchaseOrder = new List< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder >();
			var res = purchaseOrder.Select( x => x.ToQBPurchaseOrder() );
			return res;
		}

		public static InventoryItem ToInventoryItem( this Item item )
		{
			var inventoryItem = new InventoryItem
			{
				QtyOnHand = item.Qty,
				Sku = item.Name,
				Id = item.Id,
				SyncToken = item.SyncToken,
				IncomeAccRefValue = item.IncomeAccRefValue,
				IncomeAccRefName = item.IncomeAccRefName,
				IncomeAccRefType = item.IncomeAccRefType,
				ExpenseAccRefValue = item.ExpenseAccRefValue,
				ExpenseAccRefName = item.ExpenseAccRefName,
				ExpenseAccRefType = item.ExpenseAccRefType,
			};

			return inventoryItem;
		}

		public static IEnumerable< Product > ToQBProduct( this IEnumerable< Item > source )
		{
			var orders = source.Select( ToQBProduct );
			return orders;
		}

		public static Product ToQBProduct( this Item source )
		{
			var order = new Product()
			{
				ExpenseAccRefName = source.ExpenseAccRefName,
				ExpenseAccRefType = source.ExpenseAccRefType,
				ExpenseAccRefValue = source.ExpenseAccRefValue,
				Id = source.Id,
				IncomeAccRefName = source.IncomeAccRefName,
				IncomeAccRefType = source.IncomeAccRefType,
				IncomeAccRefValue = source.IncomeAccRefValue,
				Name = source.Name,
				QtyOnHand = source.Qty,
				SyncToken = source.SyncToken
			};
			return order;
		}
		#endregion

		#region FromPublicService
		public static IEnumerable< InventoryItem > ToQBInventoryItem( this IEnumerable< Inventory > source )
		{
			var orders = source.Select( x => ToQBInventoryItem( ( Inventory )x ) );
			return orders;
		}

		public static InventoryItem ToQBInventoryItem( this Inventory source )
		{
			var order = new InventoryItem()
			{
			};
			return order;
		}
		#endregion

		#region FromQBSdk
		public static Line ToQBAccessLine( this Intuit.Ipp.Data.Line line )
		{
			var qbAccessLine = new Line
			{
				Id = line.Id,
				Amount = line.Amount,
				Description = line.Description,
				LineNum = line.LineNum,
			};

			return qbAccessLine;
		}

		public static InvoiceLine ToQBAccessInvoiceLine( this Intuit.Ipp.Data.Line line )
		{
			var ineDetail = ( line.AnyIntuitObject as SalesItemLineDetail ) ?? new SalesItemLineDetail { Qty = 0, ItemRef = new ReferenceType() { name = "" } };
			var qbAccessLine = new InvoiceLine
			{
				Id = line.Id,
				Amount = line.Amount,
				Description = line.Description,
				LineNum = line.LineNum,
				Qty = ineDetail.Qty,
				Sku = ineDetail.ItemRef.name,
			};
			return qbAccessLine;
		}

		public static SalesReceiptLine ToQBAccessSalesReceiptLine( this Intuit.Ipp.Data.Line line )
		{
			var ineDetail = ( line.AnyIntuitObject as SalesItemLineDetail ) ?? new SalesItemLineDetail { Qty = 0, ItemRef = new ReferenceType() { name = "" } };
			var qbAccessLine = new SalesReceiptLine
			{
				Id = line.Id,
				Amount = line.Amount,
				Description = line.Description,
				LineNum = line.LineNum,
				Qty = ineDetail.Qty,
				Sku = ineDetail.ItemRef.name,
			};
			return qbAccessLine;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder ToQBServicePurchaseOrder( this Intuit.Ipp.Data.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder
			{
			};

			return qbPurchaseOrder;
		}

		public static SalesReceipt ToQBSalesReceipt( this Intuit.Ipp.Data.SalesReceipt salesReceipt )
		{
			var qbSalesReceipt = new SalesReceipt
			{
				Id = salesReceipt.Id,
				DocNumber = salesReceipt.DocNumber,
				Currency = salesReceipt.CurrencyRef != null ? salesReceipt.CurrencyRef.Value : PredefinedValues.NotAvailable,
				TotalAmt = salesReceipt.TotalAmt,
				SyncToken = salesReceipt.SyncToken,
				Balance = salesReceipt.Balance,
				Line = salesReceipt.Line.Select( x => x.ToQBAccessSalesReceiptLine() ).ToList(),
				PONumber = salesReceipt.PONumber,
				ShipCity = salesReceipt.ShipAddr != null ? salesReceipt.ShipAddr.City : PredefinedValues.NotAvailable,
				ShipCountry = salesReceipt.ShipAddr != null ? salesReceipt.ShipAddr.Country : PredefinedValues.NotAvailable,
				ShipCountryCode = salesReceipt.ShipAddr != null ? salesReceipt.ShipAddr.CountryCode : PredefinedValues.NotAvailable,
				ShipPostalCode = salesReceipt.ShipAddr != null ? salesReceipt.ShipAddr.PostalCode : PredefinedValues.NotAvailable,
				ShipPostalCodeSuffix = salesReceipt.ShipAddr != null ? salesReceipt.ShipAddr.PostalCodeSuffix : PredefinedValues.NotAvailable,
				ShipDate = salesReceipt.ShipDate,
				CreateTime = salesReceipt.MetaData.CreateTime,
				TrackingNum = salesReceipt.TrackingNum,
			};

			return qbSalesReceipt;
		}

		public static Bill ToQBBill( this Intuit.Ipp.Data.Bill payment )
		{
			var qbBill = new Bill
			{
			};

			return qbBill;
		}

		public static Invoice ToQBAccessInvoice( this Intuit.Ipp.Data.Invoice invoice )
		{
			var qbAccessItem = new Invoice
			{
				Id = invoice.Id,
				DocNumber = invoice.DocNumber,
				Currency = invoice.CurrencyRef != null ? invoice.CurrencyRef.Value : PredefinedValues.NotAvailable,
				TotalAmt = invoice.TotalAmt,
				SyncToken = invoice.SyncToken,
				ShipCity = invoice.ShipAddr != null ? invoice.ShipAddr.City : PredefinedValues.NotAvailable,
				ShipCountry = invoice.ShipAddr != null ? invoice.ShipAddr.Country : PredefinedValues.NotAvailable,
				ShipCountryCode = invoice.ShipAddr != null ? invoice.ShipAddr.CountryCode : PredefinedValues.NotAvailable,
				ShipPostalCode = invoice.ShipAddr != null ? invoice.ShipAddr.PostalCode : PredefinedValues.NotAvailable,
				ShipPostalCodeSuffix = invoice.ShipAddr != null ? invoice.ShipAddr.PostalCodeSuffix : PredefinedValues.NotAvailable,
				ShipDate = invoice.ShipDate,
				Deposit = invoice.Deposit,
				TrackingNum = invoice.TrackingNum,
				CreateTime = invoice.MetaData.CreateTime,
				Line = invoice.Line.Select( x => x.ToQBAccessInvoiceLine() ).ToList(),
			};

			return qbAccessItem;
		}

		public static Payment ToQBAccessPayment( this Intuit.Ipp.Data.Payment payment )
		{
			var qbAccessItem = new Payment
			{
				Id = payment.Id,
				DocNumber = payment.DocNumber,
				TotalAmt = payment.TotalAmt,
				SyncToken = payment.SyncToken,
				Line = payment.Line.Select( x => x.ToQBAccessLine() ).ToList(),
			};

			return qbAccessItem;
		}

		public static Item ToQBAccessItem( this Intuit.Ipp.Data.Item item )
		{
			var qbAccessItem = new Item
			{
				Id = item.Id,
				Name = item.Name,
				Qty = item.QtyOnHand,
				SyncToken = item.SyncToken,
				IncomeAccRefValue = item.IncomeAccountRef.Value,
				IncomeAccRefName = item.IncomeAccountRef.name,
				IncomeAccRefType = item.IncomeAccountRef.type,
				ExpenseAccRefValue = item.ExpenseAccountRef.Value,
				ExpenseAccRefName = item.ExpenseAccountRef.name,
				ExpenseAccRefType = item.ExpenseAccountRef.type,
			};

			return qbAccessItem;
		}
		#endregion

		#region Сommon
		public static string ToStringUtcIso8601( this DateTime dateTime )
		{
			var universalTime = dateTime.ToUniversalTime();
			var result = XmlConvert.ToString( universalTime, XmlDateTimeSerializationMode.RoundtripKind );
			return result;
		}

		public static string ToUrlParameterString( this DateTime dateTime )
		{
			var strRes = XmlConvert.ToString( dateTime, "yyyy-MM-ddTHH:mm:ss" );
			var result = strRes.Replace( "T", "%20" );
			return result;
		}

		public static string ToSoapParameterString( this DateTime dateTime )
		{
			var strRes = XmlConvert.ToString( dateTime, "yyyy-MM-ddTHH:mm:ss" );
			var result = strRes.Replace( "T", " " );
			return result;
		}

		public static DateTime ToDateTimeOrDefault( this string srcString )
		{
			try
			{
				var dateTime = DateTime.Parse( srcString, CultureInfo.InvariantCulture );
				return dateTime;
			}
			catch
			{
				return default( DateTime );
			}
		}

		public static int ToIntOrDefault( this string srcString )
		{
			try
			{
				var result = int.Parse( srcString, CultureInfo.InvariantCulture );
				return result;
			}
			catch
			{
				return default( int );
			}
		}

		public static decimal ToDecimalOrDefault( this string srcString )
		{
			decimal parsedNumber;

			try
			{
				parsedNumber = decimal.Parse( srcString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture );
			}
			catch
			{
				try
				{
					parsedNumber = decimal.Parse( srcString, new NumberFormatInfo { NumberDecimalSeparator = "," } );
				}
				catch
				{
					parsedNumber = default( decimal );
				}
			}

			return parsedNumber;
		}

		public static T DeepClone< T >( this T obj )
		{
			using( var ms = new MemoryStream() )
			{
				var formstter = new BinaryFormatter();
				formstter.Serialize( ms, obj );
				ms.Position = 0;
				return ( T )formstter.Deserialize( ms );
			}
		}

		public static string BuildUrl( this IEnumerable< string > urlParrts, bool escapeUrl = false )
		{
			var resultUrl = string.Empty;
			try
			{
				resultUrl = urlParrts.Aggregate( ( ac, x ) =>
				{
					string result;

					if( !string.IsNullOrWhiteSpace( ac ) )
						ac = ac.EndsWith( "/" ) ? ac : ac + "/";

					if( !string.IsNullOrWhiteSpace( x ) )
					{
						x = x.EndsWith( "/" ) ? x : x + "/";
						x = x.StartsWith( "/" ) ? x.TrimStart( '/' ) : x;

						if( escapeUrl )
							result = string.IsNullOrWhiteSpace( ac ) ? new Uri( x ).AbsoluteUri : new Uri( new Uri( ac ), x ).AbsoluteUri;
						else
							result = string.IsNullOrWhiteSpace( ac ) ? x : string.Format( "{0}{1}", ac, x );
						// new Uri(new Uri(ac), x).AbsoluteUri;
					}
					else
					{
						if( escapeUrl )
							result = string.IsNullOrWhiteSpace( ac ) ? string.Empty : new Uri( ac ).AbsoluteUri;
						else
							result = string.IsNullOrWhiteSpace( ac ) ? string.Empty : ac;
					}

					return result;
				} );
			}
			catch
			{
			}

			return resultUrl;
		}

		public static List< List< T > > SplitToChunks< T >( this List< T > source, int chunkSize )
		{
			var i = 0;
			var chunks = new List< List< T > >();
			while( i < source.Count() )
			{
				var temp = source.Skip( i ).Take( chunkSize ).ToList();
				chunks.Add( temp );
				i += chunkSize;
			}
			return chunks;
		}

		public static IEnumerable< IEnumerable< T > > Batch< T >(
			this IEnumerable< T > source, int batchSize )
		{
			using( var enumerator = source.GetEnumerator() )
			{
				while( enumerator.MoveNext() )
					yield return YieldBatchElements( enumerator, batchSize - 1 );
			}
		}

		private static IEnumerable< T > YieldBatchElements< T >(
			IEnumerator< T > source, int batchSize )
		{
			yield return source.Current;
			for( var i = 0; i < batchSize && source.MoveNext(); i++ )
			{
				yield return source.Current;
			}
		}
		#endregion
	}
}