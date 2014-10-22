using System.Web.Mvc;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Intuit.Ipp.Core;
using QuickBooksOnlineAccess.Models;

namespace QuickBooksOnlineAccessAuthExample.Controllers
{
	public class HomeController : Controller
	{
		private static readonly QuickBooksOnlineNonAuthenticatedUserCredentials _quickBooksNonAuthenticatedUserCredentials = new QuickBooksOnlineNonAuthenticatedUserCredentials(
			"7190f64ab6dd1b4419babf0b779eb888360e",
			"qyprdVavNNKgsUWjF2Dmblss4mTJYL",
			"FaVbor7GtvylsgzcRdrPnvR58GKkEqNXu2FItp1i",
			"http://localhost:27286/home/Callback"
			);

		//
		// GET: /Home/
		public ActionResult Index()
		{
			return this.View();
		}

		public void Grant()
		{
			var oauthEndpoint = _quickBooksNonAuthenticatedUserCredentials.OauthEndPoint;
			var token = ( IToken )System.Web.HttpContext.Current.Session[ "requestToken" ];
			var session = this.CreateSession( _quickBooksNonAuthenticatedUserCredentials.ConsumerKey, _quickBooksNonAuthenticatedUserCredentials.ConsumerSecret, oauthEndpoint );
			var requestToken = session.GetRequestToken();
			System.Web.HttpContext.Current.Session[ "requestToken" ] = requestToken;
			var RequestToken = requestToken.Token;
			var TokenSecret = requestToken.TokenSecret;
			oauthEndpoint = _quickBooksNonAuthenticatedUserCredentials.AuthorizeUrl + "?oauth_token=" + RequestToken + "&oauth_callback=" + UriUtility.UrlEncode( _quickBooksNonAuthenticatedUserCredentials.CallbackUrl );
			this.Response.Redirect( oauthEndpoint );
		}

		public string CallBack()
		{
			if( !this.Request.QueryString.HasKeys() )
				return "none";

			var quickBooksAuthenticatedUserCredentials = new QuickBooksOnlineAuthenticatedUserCredentials();

			var oauthVerifyer = this.Request.QueryString[ "oauth_verifier" ].ToString();

			quickBooksAuthenticatedUserCredentials.RealmId = this.Request.QueryString[ "realmId" ].ToString();

			int profile__DataSource;
			switch( this.Request.QueryString[ "dataSource" ].ToString().ToLower() )
			{
				case "qbo":
					profile__DataSource = ( int )IntuitServicesType.QBO;
					break;
				case "qbd":
					profile__DataSource = ( int )IntuitServicesType.QBD;
					break;
			}

			var consumerContext = new OAuthConsumerContext
			{
				ConsumerKey = _quickBooksNonAuthenticatedUserCredentials.ConsumerKey,
				ConsumerSecret = _quickBooksNonAuthenticatedUserCredentials.ConsumerSecret,
				SignatureMethod = SignatureMethod.HmacSha1
			};

			IOAuthSession clientSession = new OAuthSession( consumerContext,
				_quickBooksNonAuthenticatedUserCredentials.GetRequestTokenUrl,
				_quickBooksNonAuthenticatedUserCredentials.OauthEndPoint,
				_quickBooksNonAuthenticatedUserCredentials.GetAccessTokenUrl );

			try
			{
				var accessToken = clientSession.ExchangeRequestTokenForAccessToken( ( IToken )this.Session[ "requestToken" ], oauthVerifyer );
				quickBooksAuthenticatedUserCredentials.OAuthAccessToken = accessToken.Token;
				quickBooksAuthenticatedUserCredentials.OAuthAccessTokenSecret = accessToken.TokenSecret;
			}
			catch
			{
			}

			return "Success";
		}

		protected IOAuthSession CreateSession( string consumerKey, string consumerSecret, string oauthEndpoint )
		{
			var consumerContext = new OAuthConsumerContext
			{
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret,
				SignatureMethod = SignatureMethod.HmacSha1
			};
			return new OAuthSession( consumerContext,
				_quickBooksNonAuthenticatedUserCredentials.GetRequestTokenUrl,
				oauthEndpoint,
				_quickBooksNonAuthenticatedUserCredentials.GetAccessTokenUrl );
		}
	}
}