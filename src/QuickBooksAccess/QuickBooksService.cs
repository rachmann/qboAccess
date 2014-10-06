using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickBooksAccess.Misc;
using QuickBooksAccess.Models;
using QuickBooksAccess.Models.GetOrders;
using QuickBooksAccess.Models.GetProducts;
using QuickBooksAccess.Models.Ping;
using QuickBooksAccess.Models.PutInventory;

namespace QuickBooksAccess
{
	public class QuickBooksService : IQuickBooksService
	{
		private void LogTraceException(Exception exception)
		{
			QuickBooksLogger.Log().Trace(exception, "[quickBooks] An exception occured.");
		}

		public async Task<PingInfo> Ping()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				this.LogTraceException(exception);
				throw;
			}
		}

		public QuickBooksService(QuickBooksAuthenticatedUserCredentials quickBooksAuthenticatedUserCredentials)
		{
			//todo: replace me
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Order>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				this.LogTraceException(exception);
				return Enumerable.Empty<Order>();
			}
		}

		public async Task<IEnumerable<Order>> GetOrdersAsync()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				this.LogTraceException(exception);
				return Enumerable.Empty<Order>();
			}
		}

		public async Task<IEnumerable<Product>> GetProductsSimpleAsync()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				this.LogTraceException(exception);
				return Enumerable.Empty<Product>();
			}
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				this.LogTraceException(exception);
				return Enumerable.Empty<Product>();
			}
		}

		public async Task UpdateInventoryAsync(IEnumerable<Inventory> products)
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				this.LogTraceException(exception);
			}
		}
	}
}