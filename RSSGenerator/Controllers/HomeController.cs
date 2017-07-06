using RSSGenerator.Infrastructure;
using RSSGenerator.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web.Configuration;
using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace RSSGenerator.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationContext _authenticationContext;

        private string consumer_key=WebConfigurationManager.AppSettings["consumer_key"];
        private string consumer_secret = WebConfigurationManager.AppSettings["consumer_secret"];
        private string access_token = WebConfigurationManager.AppSettings["access_token"];
        private string access_token_secret = WebConfigurationManager.AppSettings["access_token_secret"];

        public ActionResult TwitterAuth()
        {
            var appCreds = new ConsumerCredentials(consumer_key, consumer_secret);

            var redirectURL = "http://" + Request.Url.Authority + "/Home/ValidateTwitterAuth";
            _authenticationContext = AuthFlow.InitAuthentication(appCreds, redirectURL);

            return new RedirectResult(_authenticationContext.AuthorizationURL);
        }

        public ActionResult ValidateTwitterAuth()
        {
            var verifierCode = Request.Params.Get("oauth_verifier");

            var authorizationId = Request.Params.Get("authorization_id");

            if (verifierCode != null)
            {
                var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId);
                var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);

                ViewBag.User = user;
            }

            return RedirectToAction("RSSGenerator");
        }

        public RssResult RSSGenerator()
        {
                Auth.SetUserCredentials(consumer_key, consumer_secret, access_token, access_token_secret);

                IEnumerable<ITweet> tweets = TwitterFuncs.GetTweets();

                SyndicationFeed feed = TwitterFuncs.FilterTweets(tweets);
                
                return new RssResult(feed);
        }
    }
}
