using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
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
			return View();
		}


		public void Grant()
		{
			var oauth_callback_url = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + ConfigurationManager.AppSettings["oauth_callback_url"];
			var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
			var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
			var oauthEndpoint = "https://oauth.intuit.com/oauth/v1"; //Constants.OauthEndPoints.IdFedOAuthBaseUrl;
			IToken token = (IToken)System.Web.HttpContext.Current.Session["requestToken"];
			IOAuthSession session = CreateSession(consumerKey, consumerSecret, oauthEndpoint);
			IToken requestToken = session.GetRequestToken();
			System.Web.HttpContext.Current.Session["requestToken"] = requestToken;
			var RequestToken = requestToken.Token;
			var TokenSecret = requestToken.TokenSecret;
			oauthEndpoint = Constants.OauthEndPoints.AuthorizeUrl + "?oauth_token=" + RequestToken + "&oauth_callback=" + UriUtility.UrlEncode(oauth_callback_url);
			Response.Redirect(oauthEndpoint);
			//return View();
		}

		public void CallBack()
		{
			if (Request.QueryString.HasKeys())
			{
				var oauthVerifyer = Request.QueryString["oauth_verifier"].ToString();

				RestProfile profile = RestProfile.GetRestProfile();

				//profile.RealmId = Request.QueryString["realmId"].ToString();
				var profile__RealmId = Request.QueryString["realmId"].ToString();

				int profile__DataSource;
				switch (Request.QueryString["dataSource"].ToString().ToLower())
				{
					case "qbo": profile__DataSource = (int)IntuitServicesType.QBO; break;
					case "qbd": profile__DataSource = (int)IntuitServicesType.QBD; break;
				}

				OAuthConsumerContext consumerContext = new OAuthConsumerContext
				{
					ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(),
					ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString(),
					SignatureMethod = SignatureMethod.HmacSha1
				};

				IOAuthSession clientSession = new OAuthSession(consumerContext,
												Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
												Constants.OauthEndPoints.IdFedOAuthBaseUrl,
												 Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken);

				try
				{
					IToken accessToken = clientSession.ExchangeRequestTokenForAccessToken((IToken)Session["requestToken"], oauthVerifyer);
					string profile__OAuthAccessToken = accessToken.Token;
					string profile__OAuthAccessTokenSecret = accessToken.TokenSecret;
					profile.Save();
				}
				catch
				{

				}
			}
			//return View();
		}
		
		/// <summary>
		/// Creates Session
		/// </summary>
		/// <returns>Returns OAuth Session</returns>
		protected IOAuthSession CreateSession(string consumerKey, string consumerSecret, string oauthEndpoint)
		{
			OAuthConsumerContext consumerContext = new OAuthConsumerContext
			{
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret,
				SignatureMethod = SignatureMethod.HmacSha1
			};
			return new OAuthSession(consumerContext,
											Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
											oauthEndpoint,
											Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken);
		}



		public class RestProfile : ProfileBase
		{
			public RestProfile() { }

			public static RestProfile GetRestProfile(string username)
			{
				return Create(username) as RestProfile;
			}

			public static RestProfile GetRestProfile()
			{
				//return Create(Membership.GetUser().UserName) as RestProfile;
				return new RestProfile();
			}

			[SettingsAllowAnonymous(true)]
			public string RealmId
			{
				get { return base["RealmId"] as string; }
				set { base["RealmId"] = value; }
			}

			[SettingsAllowAnonymous(true)]
			public string OAuthAccessToken
			{
				get { return base["OAuthAccessToken"] as string; }
				set { base["OAuthAccessToken"] = value; }
			}

			[SettingsAllowAnonymous(true)]
			public string OAuthAccessTokenSecret
			{
				get { return base["OAuthAccessTokenSecret"] as string; }
				set { base["OAuthAccessTokenSecret"] = value; }
			}

			[SettingsAllowAnonymous(true)]
			public int DataSource
			{
				get { object dataSource = base["DataSource"]; if (!dataSource.Equals(null)) { return (int)dataSource; } else { return -1; } }
				set { base["DataSource"] = value; }
			}
		}
	}
}
