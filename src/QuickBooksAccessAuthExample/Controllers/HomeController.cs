using System;
using System.Configuration;
using System.Web.Mvc;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Intuit.Ipp.Core;
using QuickBooksAccessAuthExample.Infrastructure;

namespace QuickBooksAccessAuthExample.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			return this.View();
		}

		public void Grant()
		{
			var oauth_callback_url = this.Request.Url.GetLeftPart( UriPartial.Authority ) + this.Request.ApplicationPath + ConfigurationManager.AppSettings[ "oauth_callback_url" ];
			var consumerKey = ConfigurationManager.AppSettings[ "consumerKey" ];
			var consumerSecret = ConfigurationManager.AppSettings[ "consumerSecret" ];
			var oauthEndpoint = "https://oauth.intuit.com/oauth/v1"; //Constants.OauthEndPoints.IdFedOAuthBaseUrl;
			var token = ( IToken )System.Web.HttpContext.Current.Session[ "requestToken" ];
			var session = this.CreateSession( consumerKey, consumerSecret, oauthEndpoint );
			var requestToken = session.GetRequestToken();
			System.Web.HttpContext.Current.Session[ "requestToken" ] = requestToken;
			var RequestToken = requestToken.Token;
			var TokenSecret = requestToken.TokenSecret;
			oauthEndpoint = Constants.OauthEndPoints.AuthorizeUrl + "?oauth_token=" + RequestToken + "&oauth_callback=" + UriUtility.UrlEncode( oauth_callback_url );
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
				ConsumerKey = ConfigurationManager.AppSettings[ "consumerKey" ].ToString(),
				ConsumerSecret = ConfigurationManager.AppSettings[ "consumerSecret" ].ToString(),
				SignatureMethod = SignatureMethod.HmacSha1
			};

			IOAuthSession clientSession = new OAuthSession( consumerContext,
				Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
				Constants.OauthEndPoints.IdFedOAuthBaseUrl,
				Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken );

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
				Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
				oauthEndpoint,
				Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken );
		}
	}
}