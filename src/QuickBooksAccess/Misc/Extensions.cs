﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.GetItems;
using QuickBooksAccess.Models.Services.QuickBooksServicesSdk.UpdateInventory;

namespace QuickBooksAccess.Misc
{
	internal static class Extensions
	{
		public static Item ToQBAccessItem( this Intuit.Ipp.Data.Item item )
		{
			var qbAccessItem = new Item
			{
				Id = item.Id,
				Name = item.Name,
				Qty = item.QtyOnHand,
			};

			return qbAccessItem;
		}

		public static InventoryItem ToInventoryItem( this Item item )
		{
			var inventoryItem = new InventoryItem
			{
				Qty = item.Qty,
				Sku = item.Name
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