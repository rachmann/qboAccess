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
using QuickBooksOnlineAccess.Models.GetPurchaseOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreatePurchaseOrders;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdateItemQuantityOnHand;
using QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders;
using QuickBooksOnlineAccess.Models.UpdateInventory;
using Bill = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetBills.Bill;
using Customer = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetCustomers.Customer;
using CustomField = Intuit.Ipp.Data.CustomField;
using Invoice = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetInvoices.Invoice;
using Item = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetItems.Item;
using Line = Intuit.Ipp.Data.Line;
using Payment = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetPayments.Payment;
using PurchaseOrder = Intuit.Ipp.Data.PurchaseOrder;
using SalesReceipt = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetSalesReceipts.SalesReceipt;
using Vendor = QuickBooksOnlineAccess.Models.Services.QuickBooksOnlineServicesSdk.GetVendors.Vendor;

namespace QuickBooksOnlineAccess.Misc
{
	internal static class ExtensionsInternal
	{
		#region ToJson
		public static string ToJson( this IEnumerable< IManualSerializable > source )
		{
			try
			{
				var objects = source as IList< IManualSerializable > ?? source.ToList();
				var items = string.Join( ",", objects.Where( x => x != null ).Select( x => x.ToJson() ) );
				var res = string.Format( "{{Count:{0}, Items:[{1}]}}", objects.Count(), items );
				return res;
			}
			catch
			{
				return PredefinedValues.EmptyJsonList;
			}
		}

		public static string ToJson( this IEnumerable< string > source )
		{
			try
			{
				var enumerable = source as IList< string > ?? source.ToList();
				var strSerialized = string.Format( "[{0}]", string.Join( ",", "\"" + enumerable + "\"" ) );
				var res = string.Format( "{{Count:{0}, Items:[{1}]}}", enumerable.Count(), strSerialized );
				return res;
			}
			catch( Exception )
			{
				return PredefinedValues.EmptyJsonList;
			}
		}
		#endregion

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
				Sku = source.Sku,
				UnitPrice = source.UnitPrice
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
				Sku = source.Sku,
				UnitPrice = source.UnitPrice
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
				Balance = source.Balance,
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
				CustomerName = source.CustomerName,
				CustomerValue = source.CustomerValue,
				PrivateNote = source.PrivateNote,
			};

			return qbOrder;
		}

		public static Order ToQBOrder( this SalesReceipt source )
		{
			var qbOrder = new Order
			{
				OrderType = OrderType.SalesReceipt,
				OrderId = source.Id,
				Balance = source.Balance,
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
				CustomerName = source.CustomerName,
				CustomerValue = source.CustomerValue,
				PrivateNote = source.PrivateNote,
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
		public static Intuit.Ipp.Data.SalesReceipt ToIppSaleReceipt( this SaleReceipt saleReceipt )
		{
			var ippSaleReceipt = new Intuit.Ipp.Data.SalesReceipt
			{
				DocNumber = saleReceipt.DocNumber,
				Line = saleReceipt.Line.Select( x => x.ToIppSaleReceiptLine() ).ToArray(),
				TxnDate = saleReceipt.TnxDate,
				TxnDateSpecified = true,
				CustomerRef = new ReferenceType { Value = saleReceipt.CustomerValue, name = saleReceipt.CustomerName },
				CurrencyRef = new ReferenceType { name = "United States Dollar", Value = "USD" },
				PrivateNote = saleReceipt.PrivateNote,
				CustomField = ( saleReceipt.CustomFields ?? Enumerable.Empty< Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.CustomField >() ).Select( x => x.ToQBCustomField() ).TakeWhile( ( cf, i ) => i < 2 ).ToArray(),
			};

			return ippSaleReceipt;
		}

		public static CustomField ToQBCustomField( this Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.CustomField source )
		{
			var customField = new CustomField
			{
				Name = source.Name,
				DefinitionId = source.DefinitioId,
				AnyIntuitObject = source.Value,
				Type = CustomFieldTypeEnum.StringType,
			};
			return customField;
		}

		public static Line ToIppSaleReceiptLine( this Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.Line line )
		{
			var ippSaleReceiptLine = new Line();

			var lineDetail = new SalesItemLineDetail()
			{
				Qty = line.Qty,
				QtySpecified = true,
				ItemRef = new ReferenceType()
				{
					Value = line.ItemValue,
					name = line.ItemName
				},
				ItemElementName = ItemChoiceType.UnitPrice,
				AnyIntuitObject = line.UnitPrice,
			};
			ippSaleReceiptLine.AnyIntuitObject = lineDetail;
			ippSaleReceiptLine.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
			ippSaleReceiptLine.DetailTypeSpecified = true;
			ippSaleReceiptLine.Amount = line.UnitPrice * line.Qty;
			ippSaleReceiptLine.AmountSpecified = true;

			return ippSaleReceiptLine;
		}

		public static Intuit.Ipp.Data.Invoice ToIppInvoice( this Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice.Invoice invoice )
		{
			var qbPurchaseOrder = new Intuit.Ipp.Data.Invoice
			{
				DocNumber = invoice.DocNumber,
				Line = invoice.Line.Select( x => x.ToIppInvoiceLine() ).ToArray(),
				CustomerRef = new ReferenceType { Value = invoice.CustomerValue, name = invoice.CustomerName },
				CurrencyRef = new ReferenceType { name = "United States Dollar", Value = "USD" }
			};

			return qbPurchaseOrder;
		}

		public static Line ToIppInvoiceLine( this Models.Services.QuickBooksOnlineServicesSdk.CreateInvoice.Line line )
		{
			var ippInvoiceLine = new Line();

			var lineDetail = new SalesItemLineDetail()
			{
				Qty = line.Qty,
				QtySpecified = true,
				ItemRef = new ReferenceType()
				{
					Value = line.ItemValue,
					name = line.ItemName
				},
				ItemElementName = ItemChoiceType.UnitPrice,
				AnyIntuitObject = line.UnitPrice,
			};
			ippInvoiceLine.AnyIntuitObject = lineDetail;
			ippInvoiceLine.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
			ippInvoiceLine.DetailTypeSpecified = true;
			ippInvoiceLine.Amount = line.UnitPrice * line.Qty;
			ippInvoiceLine.AmountSpecified = true;

			return ippInvoiceLine;
		}

		public static PurchaseOrder ToIppPurchaseOrder( this Models.Services.QuickBooksOnlineServicesSdk.CreatePurchaseOrders.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new PurchaseOrder
			{
				DocNumber = purchaseOrder.DocNumber,
				//Id = purchaseOrder.Id,
				//SyncToken = purchaseOrder.SyncToken,
				Line = purchaseOrder.LineItems.Select( x => x.ToIppPurchaseOrderLineItem() ).ToArray(),
				POStatus = purchaseOrder.PoStatus.ToIppPurchaseOrderStatusEnum(),
				POStatusSpecified = true,
				VendorRef = new ReferenceType
				{
					name = purchaseOrder.VendorName,
					Value = purchaseOrder.VendorValue,
				}
			};

			return qbPurchaseOrder;
		}

		public static Line ToIppPurchaseOrderLineItem( this PurchaseOrdeLineItem qbInternalPurchaseOrdeLineItem )
		{
			var basedExpenseLineDetail = new ItemBasedExpenseLineDetail
			{
				Qty = qbInternalPurchaseOrdeLineItem.Qty,
				QtySpecified = true,
				ItemRef = new ReferenceType
				{
					name = qbInternalPurchaseOrdeLineItem.ItemName,
					Value = qbInternalPurchaseOrdeLineItem.ItemValue,
				},
				ItemElementName = ItemChoiceType.UnitPrice,
				AnyIntuitObject = qbInternalPurchaseOrdeLineItem.UnitPrice,
			};

			var ippPurchaseOrderLineItem = new Line
			{
				//Amount = qbInternalPurchaseOrdeLineItem.Amount,
				Amount = qbInternalPurchaseOrdeLineItem.Qty * qbInternalPurchaseOrdeLineItem.UnitPrice,
				AmountSpecified = true,
				//Id = qbInternalPurchaseOrdeLineItem.Id,
				//LineNum = qbInternalPurchaseOrdeLineItem.LineNum,
			};

			ippPurchaseOrderLineItem.AnyIntuitObject = basedExpenseLineDetail;
			ippPurchaseOrderLineItem.DetailType = LineDetailTypeEnum.ItemBasedExpenseLineDetail;
			ippPurchaseOrderLineItem.DetailTypeSpecified = true;

			return ippPurchaseOrderLineItem;
		}

		public static PurchaseOrderStatusEnum ToIppPurchaseOrderStatusEnum( this QBPurchaseOrderStatusEnum purchaseOrder )
		{
			var res = PurchaseOrderStatusEnum.Closed;
			switch( purchaseOrder )
			{
				case QBPurchaseOrderStatusEnum.Open:
					res = PurchaseOrderStatusEnum.Open;
					break;
				case QBPurchaseOrderStatusEnum.Closed:
					res = PurchaseOrderStatusEnum.Closed;
					break;
			}

			return res;
		}

		public static PurchaseOrder ToIppPurchaseOrder( this Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new PurchaseOrder
			{
				DocNumber = purchaseOrder.DocNumber,
				Id = purchaseOrder.Id,
				SyncToken = purchaseOrder.SyncToken,
				Line = purchaseOrder.LineItems.Select( x => x.ToIppPurchaseOrderLineItem() ).ToArray(),
				POStatus = purchaseOrder.POStatus.ToIppPurchaseOrderStatusEnum(),
				POStatusSpecified = true,
				VendorRef = new ReferenceType
				{
					name = purchaseOrder.VendorName,
					Value = purchaseOrder.VendorValue,
				}
			};

			return qbPurchaseOrder;
		}

		public static Line ToIppPurchaseOrderLineItem( this QBInternalPurchaseOrdeLineItem qbInternalPurchaseOrdeLineItem )
		{
			var ippPurchaseOrderLineItem = new Line
			{
				Amount = qbInternalPurchaseOrdeLineItem.Amount,
				AmountSpecified = true,
				Id = qbInternalPurchaseOrdeLineItem.Id,
				LineNum = qbInternalPurchaseOrdeLineItem.LineNum,
			};

			var basedExpenseLineDetail = new ItemBasedExpenseLineDetail
			{
				Qty = qbInternalPurchaseOrdeLineItem.Qty,
				QtySpecified = true,
			};

			basedExpenseLineDetail.ItemRef = new ReferenceType
			{
				name = qbInternalPurchaseOrdeLineItem.ItemName,
				Value = qbInternalPurchaseOrdeLineItem.ItemValue,
			};

			ippPurchaseOrderLineItem.AnyIntuitObject = basedExpenseLineDetail;
			//ippPurchaseOrderLineItem.DetailType = LineDetailTypeEnum.PurchaseOrderItemLineDetail;
			ippPurchaseOrderLineItem.DetailType = LineDetailTypeEnum.ItemBasedExpenseLineDetail;
			ippPurchaseOrderLineItem.DetailTypeSpecified = true;

			return ippPurchaseOrderLineItem;
		}

		public static PurchaseOrderStatusEnum ToIppPurchaseOrderStatusEnum( this QBInternalPurchaseOrderStatusEnum purchaseOrder )
		{
			var res = PurchaseOrderStatusEnum.Closed;
			switch( purchaseOrder )
			{
				case QBInternalPurchaseOrderStatusEnum.Open:
					res = PurchaseOrderStatusEnum.Open;
					break;
				case QBInternalPurchaseOrderStatusEnum.Closed:
					res = PurchaseOrderStatusEnum.Closed;
					break;
			}

			return res;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders.PurchaseOrder ToQBInternalPurchaseOrder( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new Models.Services.QuickBooksOnlineServicesSdk.UpdatePurchaseOrders.PurchaseOrder
			{
				DocNumber = purchaseOrder.DocNumber,
				Id = purchaseOrder.Id,
				SyncToken = purchaseOrder.SyncToken,
				LineItems = purchaseOrder.LineItems.Select( x => x.ToQBInternalPurchaseOrderLineItem() ),
				POStatus = ( QBInternalPurchaseOrderStatusEnum )Enum.Parse( typeof( Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum ), purchaseOrder.PoStatus.ToString() ),
				VendorValue = purchaseOrder.VendorValue,
				VendorName = purchaseOrder.VendorName,
			};

			return qbPurchaseOrder;
		}

		public static QBInternalPurchaseOrdeLineItem ToQBInternalPurchaseOrderLineItem( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrdeLineItem purchaseOrder )
		{
			var internalPurchaseOrdeLineItem = new QBInternalPurchaseOrdeLineItem
			{
				ItemName = purchaseOrder.ItemName,
				ItemValue = purchaseOrder.ItemValue,
				Id = purchaseOrder.Id,
				Qty = purchaseOrder.Qty,
				Rate = purchaseOrder.Rate,
				LineNum = purchaseOrder.LineNum,
				Amount = purchaseOrder.Amount,
			};

			return internalPurchaseOrdeLineItem;
		}

		public static Models.GetPurchaseOrders.PurchaseOrder ToQBPurchaseOrder( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new Models.GetPurchaseOrders.PurchaseOrder
			{
				DocNumber = purchaseOrder.DocNumber,
				PoStatus = purchaseOrder.PoStatus.ToQBPurchaseOrderStatusEnum(),
				VendorName = purchaseOrder.VendorName,
				VendorValue = purchaseOrder.VendorValue,
				TnxDate = purchaseOrder.TnxDate,
				LineItems = purchaseOrder.LineItems.ToQBPurchaseOrderLineItem(),
			};

			return qbPurchaseOrder;
		}

		public static PoStatusEnum ToQBPurchaseOrderStatusEnum( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum purchaseOrder )
		{
			switch( purchaseOrder )
			{
				case Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum.Open:
					return PoStatusEnum.Open;
				case Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum.Closed:
					return PoStatusEnum.Closed;
				default:
					return PoStatusEnum.Unknown;
			}
		}

		public static OrderLineItem ToQBPurchaseOrderLineItem( this Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrdeLineItem purchaseOrder )
		{
			var orderLineItem = new OrderLineItem
			{
				Amount = purchaseOrder.Amount,
				Qty = purchaseOrder.Qty,
				Rate = purchaseOrder.Rate,
				Id = purchaseOrder.Id,
				LineNum = purchaseOrder.LineNum,
				ItemName = purchaseOrder.ItemName,
			};
			return orderLineItem;
		}

		public static IEnumerable< OrderLineItem > ToQBPurchaseOrderLineItem( this IEnumerable< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrdeLineItem > purchaseOrder )
		{
			var purchaseOrdeLineItems = purchaseOrder as IList< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrdeLineItem > ?? purchaseOrder.ToList();
			if( purchaseOrder == null || !purchaseOrdeLineItems.Any() )
				return Enumerable.Empty< OrderLineItem >();

			return purchaseOrdeLineItems.Select( x => x.ToQBPurchaseOrderLineItem() );
		}

		public static IEnumerable< Models.GetPurchaseOrders.PurchaseOrder > ToQBPurchaseOrder( this IEnumerable< Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder > purchaseOrder )
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
		public static SaleReceipt ToQBSaleReceipt( this Models.CreateOrders.Order source )
		{
			var saleReceipt = new SaleReceipt()
			{
				DocNumber = source.DocNumber,
				CustomerName = source.CustomerName,
				CustomerValue = source.CustomerValue,
				PrivateNote = source.PrivateNote,
				TnxDate = source.TnxDate,
				Line = source.LineItems.Select( x => x.ToQBSaleReceiptLineItem() ),
				CustomFields = ( source.CustomFields ?? Enumerable.Empty< Models.CreateOrders.CustomField >() ).Select( x => x.ToQBCustomField() ),
			};

			return saleReceipt;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.CustomField ToQBCustomField( this Models.CreateOrders.CustomField source )
		{
			var customField = new Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.CustomField
			{
				Name = source.Name,
				DefinitioId = source.DefinitioId,
				Value = source.Value,
			};
			return customField;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.Line ToQBSaleReceiptLineItem( this Models.CreateOrders.OrderLineItem source )
		{
			var lineItem = new Models.Services.QuickBooksOnlineServicesSdk.CreateSaleReceipts.Line()
			{
				ItemName = source.ItemName,
				ItemValue = source.Id,
				Qty = source.Qty,
				UnitPrice = source.Rate,
			};
			return lineItem;
		}

		public static IEnumerable< InventoryItem > ToQBInventoryItem( this IEnumerable< Inventory > source )
		{
			var orders = source.Select( x => ToQBInventoryItem( ( Inventory )x ) );
			return orders;
		}

		public static InventoryItem ToQBInventoryItem( this Inventory source )
		{
			var order = new InventoryItem()
			{
				QtyOnHand = source.Quantity,
				Sku = source.NameOrSku,
				Id = source.ProductId,
				SyncToken = source.SyncToken,
				IncomeAccRefValue = source.IncomeAccRefValue,
				IncomeAccRefName = source.IncomeAccRefName,
				IncomeAccRefType = source.IncomeAccRefType,
				ExpenseAccRefValue = source.ExpenseAccRefValue,
				ExpenseAccRefName = source.ExpenseAccRefName,
				ExpenseAccRefType = source.ExpenseAccRefType,
			};
			return order;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.CreatePurchaseOrders.PurchaseOrder ToQBPurchaseOrder( this Models.CreatePurchaseOrders.PurchaseOrder source )
		{
			var order = new Models.Services.QuickBooksOnlineServicesSdk.CreatePurchaseOrders.PurchaseOrder()
			{
				DocNumber = source.DocNumber,
				//Id = source.,
				//SyncToken = source.,
				TnxDate = source.TnxDate,
				VendorName = source.VendorName,
				VendorValue = source.VendorValue,
				PoStatus = source.PoStatus.ToQbPurchaseOrderStatusEnum(),
				LineItems = source.LineItems.Select( x => x.ToQBPurchaseOrder() ),
			};
			return order;
		}

		public static QBPurchaseOrderStatusEnum ToQbPurchaseOrderStatusEnum( this Models.CreatePurchaseOrders.PoStatusEnum source )
		{
			switch( source )
			{
				case Models.CreatePurchaseOrders.PoStatusEnum.Closed:
					return QBPurchaseOrderStatusEnum.Closed;
				case Models.CreatePurchaseOrders.PoStatusEnum.Open:
					return QBPurchaseOrderStatusEnum.Open;
				default:
					return QBPurchaseOrderStatusEnum.Unknown;
			}
		}

		public static PurchaseOrdeLineItem ToQBPurchaseOrder( this Models.CreatePurchaseOrders.OrderLineItem source )
		{
			var order = new PurchaseOrdeLineItem()
			{
				ItemName = source.ItemName,
				ItemValue = source.Id,
				Qty = source.Qty,
				UnitPrice = source.Rate,
			};
			return order;
		}
		#endregion

		#region FromQBSdk
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

		public static InvoiceLine ToQBAccessInvoiceLine( this Line line )
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

			if( ineDetail.ItemElementName == ItemChoiceType.UnitPrice && ( ineDetail.AnyIntuitObject is double ) )
				qbAccessLine.UnitPrice = ( double )ineDetail.AnyIntuitObject;
			return qbAccessLine;
		}

		public static SalesReceiptLine ToQBAccessSalesReceiptLine( this Line line )
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

			if( ineDetail.ItemElementName == ItemChoiceType.UnitPrice && ( ineDetail.AnyIntuitObject is double ) )
				qbAccessLine.UnitPrice = ( double )ineDetail.AnyIntuitObject;

			return qbAccessLine;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrdeLineItem ToQBServicePurchaseOrderLineItem( this Line line )
		{
			var ordeLineItem = new Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrdeLineItem
			{
				Amount = line.Amount,
				Id = line.Id,
				LineNum = line.LineNum,
			};

			var intuitObj = line.AnyIntuitObject as ItemBasedExpenseLineDetail;

			if( intuitObj != null )
			{
				ordeLineItem.Qty = intuitObj.Qty;
				ordeLineItem.ItemName = intuitObj.ItemRef.name;
				ordeLineItem.ItemValue = intuitObj.ItemRef.Value;
			}

			return ordeLineItem;
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum ToQBServicePurchaseOrderStatusEnum( this PurchaseOrderStatusEnum orderStatusEnum )
		{
			switch( orderStatusEnum )
			{
				case PurchaseOrderStatusEnum.Open:
					return Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum.Open;
				case PurchaseOrderStatusEnum.Closed:
					return Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum.Closed;
				default:
					return Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.QBPurchaseOrderStatusEnum.Unknown;
			}
		}

		public static Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder ToQBServicePurchaseOrder( this PurchaseOrder purchaseOrder )
		{
			var qbPurchaseOrder = new Models.Services.QuickBooksOnlineServicesSdk.GetPurchaseOrders.PurchaseOrder
			{
				Id = purchaseOrder.Id,
				DocNumber = purchaseOrder.DocNumber,
				TnxDate = purchaseOrder.TxnDate,
				LineItems = purchaseOrder.Line.ToList().Select( x => x.ToQBServicePurchaseOrderLineItem() ).ToList(),
				SyncToken = purchaseOrder.SyncToken,
				VendorName = purchaseOrder.VendorRef.name,
				VendorValue = purchaseOrder.VendorRef.Value,
				PoStatus = purchaseOrder.POStatus.ToQBServicePurchaseOrderStatusEnum()
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
				CustomerName = ( salesReceipt.CustomerRef != null ) ? salesReceipt.CustomerRef.name : null,
				CustomerValue = ( salesReceipt.CustomerRef != null ) ? salesReceipt.CustomerRef.Value : null,
				PrivateNote = salesReceipt.PrivateNote,
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
				Balance = invoice.Balance,
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
				CustomerName = ( invoice.CustomerRef != null ) ? invoice.CustomerRef.name : null,
				CustomerValue = ( invoice.CustomerRef != null ) ? invoice.CustomerRef.Value : null,
				PrivateNote = invoice.PrivateNote,
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
			};

			if( item.IncomeAccountRef != null )
			{
				qbAccessItem.IncomeAccRefValue = item.IncomeAccountRef.Value;
				qbAccessItem.IncomeAccRefName = item.IncomeAccountRef.name;
				qbAccessItem.IncomeAccRefType = item.IncomeAccountRef.type;
			}

			if( item.ExpenseAccountRef != null )
			{
				qbAccessItem.ExpenseAccRefValue = item.ExpenseAccountRef.Value;
				qbAccessItem.ExpenseAccRefName = item.ExpenseAccountRef.name;
				qbAccessItem.ExpenseAccRefType = item.ExpenseAccountRef.type;
			}

			return qbAccessItem;
		}

		public static Vendor ToQBAccessVendor( this Intuit.Ipp.Data.Vendor item )
		{
			var qbAccessVendor = new Vendor
			{
				Id = item.Id,
				Name = item.DisplayName,
			};

			return qbAccessVendor;
		}

		public static Customer ToQBAccessCustomer( this Intuit.Ipp.Data.Customer source )
		{
			var customer = new Customer
			{
				Id = source.Id,
				Name = source.DisplayName,
			};

			return customer;
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