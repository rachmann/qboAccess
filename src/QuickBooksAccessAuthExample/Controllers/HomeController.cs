using System.Web.Mvc;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Intuit.Ipp.Core;
using QuickBooksAccess.Models;

namespace QuickBooksAccessAuthExample.Controllers
{
	public class HomeController : Controller
	{
		private static readonly QuickBooksNonAuthenticatedUserCredentials _quickBooksNonAuthenticatedUserCredentials = new QuickBooksNonAuthenticatedUserCredentials(
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
			var oauth_callback_url = _quickBooksNonAuthenticatedUserCredentials.CallbackUrl;
			var consumerKey = _quickBooksNonAuthenticatedUserCredentials.ConsumerKey;
			var consumerSecret = _quickBooksNonAuthenticatedUserCredentials.ConsumerSecret;
			var oauthEndpoint = _quickBooksNonAuthenticatedUserCredentials.OauthEndPoint; //Constants.OauthEndPoints.IdFedOAuthBaseUrl;
			var token = ( IToken )System.Web.HttpContext.Current.Session[ "requestToken" ];
			var session = this.CreateSession( consumerKey, consumerSecret, oauthEndpoint );
			var requestToken = session.GetRequestToken();
			System.Web.HttpContext.Current.Session[ "requestToken" ] = requestToken;
			var RequestToken = requestToken.Token;
			var TokenSecret = requestToken.TokenSecret;
			oauthEndpoint = _quickBooksNonAuthenticatedUserCredentials.AuthorizeUrl + "?oauth_token=" + RequestToken + "&oauth_callback=" + UriUtility.UrlEncode( oauth_callback_url );
			this.Response.Redirect( oauthEndpoint );
		}

		public void CallBack()
		{
			if( !this.Request.QueryString.HasKeys() )
				return;

			var oauthVerifyer = this.Request.QueryString[ "oauth_verifier" ].ToString();

			var profile__RealmId = this.Request.QueryString[ "realmId" ].ToString();

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
				var profile__OAuthAccessToken = accessToken.Token;
				var profile__OAuthAccessTokenSecret = accessToken.TokenSecret;
			}
			catch
			{
			}
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