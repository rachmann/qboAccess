using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Intuit.Ipp.Data;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdateItemQuantityOnHand;
using Bill = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetBills.Bill;
using Invoice = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices.Invoice;
using Item = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetItems.Item;
using Payment = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments.Payment;
using PurchaseOrder = QuickBooksOnlineAccess.Models.GetPurchaseOrders.PurchaseOrder;
using SalesReceipt = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts.SalesReceipt;

namespace QuickBooksOnlineAccess.Misc
{
	internal static class Extensions
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

		public static Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder ToQBServicePurchaseOrder( this Intuit.Ipp.Data.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder
			{
			};

			return qbPurchaseOrder;
		}

		public static PurchaseOrder ToQBPurchaseOrder( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new PurchaseOrder
			{
			};

			return qbPurchaseOrder;
		}

		public static IEnumerable< PurchaseOrder > ToQBPurchaseOrder( this IEnumerable< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder > purchaseOrder )
		{
			if( purchaseOrder == null )
				purchaseOrder = new List< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder >();
			var res = purchaseOrder.Select( x => x.ToQBPurchaseOrder() );
			return res;
		}

		public static SalesReceipt ToQBSalesReceipt( this Intuit.Ipp.Data.SalesReceipt salesReceipt )
		{
			var qbSalesReceipt = new SalesReceipt
			{
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

		public static Invoice ToQBAccessInvoice( this Intuit.Ipp.Data.Invoice invoice )
		{
			var qbAccessItem = new Invoice
			{
				Id = invoice.Id,
				DocNumber = invoice.DocNumber,
				NotAvailable = invoice.CurrencyRef != null ? invoice.CurrencyRef.Value : PredefinedValues.NotAvailable,
				TotalAmt = invoice.TotalAmt,
				SyncToken = invoice.SyncToken,
				City = invoice.ShipAddr != null ? invoice.ShipAddr.City : PredefinedValues.NotAvailable,
				Country = invoice.ShipAddr != null ? invoice.ShipAddr.Country : PredefinedValues.NotAvailable,
				CountryCode = invoice.ShipAddr != null ? invoice.ShipAddr.CountryCode : PredefinedValues.NotAvailable,
				PostalCode = invoice.ShipAddr != null ? invoice.ShipAddr.PostalCode : PredefinedValues.NotAvailable,
				PostalCodeSuffix = invoice.ShipAddr != null ? invoice.ShipAddr.PostalCodeSuffix : PredefinedValues.NotAvailable,
				ShipDate = invoice.ShipDate,
				Deposit = invoice.Deposit,
				TrackingNum = invoice.TrackingNum,
				Line = invoice.Line.Select( x => x.ToQBAccessInvoiceLine() ).ToList(),
			};

			return qbAccessItem;
		}

		public static InvoiceLine ToQBAccessInvoiceLine( this Line line )
		{
			var ineDetail = ( line.AnyIntuitObject as SalesItemLineDetail ) ?? new SalesItemLineDetail { Qty = 0 };
			var qbAccessLine = new InvoiceLine
			{
				Id = line.Id,
				Amount = line.Amount,
				Description = line.Description,
				LineNum = line.LineNum,
				Qty = ineDetail.Qty
			};
			return qbAccessLine;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.GetPayments.Line ToQBAccessLine( this Line line )
		{
			var qbAccessLine = new Models.Services.QuickBooksOnlineServicesSdk.GetPayments.Line
			{
				Id = line.Id,
				Amount = line.Amount,
				Description = line.Description,
				LineNum = line.LineNum,
			};

			return qbAccessLine;
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
	}
}