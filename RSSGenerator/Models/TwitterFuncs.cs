using RSSGenerator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace RSSGenerator.Models
{
    public class TwitterFuncs
    {

        public static IEnumerable<ITweet> GetTweets()
        {
            var tweets = Timeline.GetHomeTimeline();

            return tweets;
        }

        public static SyndicationFeed FilterTweets(IEnumerable<ITweet> tweets)
        {
            SyndicationFeed feed = new SyndicationFeed("Твиты Сергея", "Данная RSS-лента предоставляет список отфильтрованных твитов Сергея", new Uri("http://twitter.com"));

            List<SyndicationItem> rssItems = new List<SyndicationItem>();


            List<string> links = new List<string>();

            foreach (var tweet in tweets)
            {
                if (tweet.Entities.Urls.Count != 0)
                {
                    foreach (var link in tweet.Entities.Urls)
                    {
                        if (!links.Contains(link.URL))
                        {
                            links.Add(link.URL);

                            SyndicationPerson syndicationPerson = new SyndicationPerson(null, tweet.CreatedBy.ToString(), tweet.CreatedBy.Url);

                            SyndicationLink syndicationLink = new SyndicationLink();
                            syndicationLink.Uri = new Uri(tweet.Url.ToString());

                            SyndicationItem item = new SyndicationItem();
                            item.Title = new TextSyndicationContent(tweet.CreatedBy.ToString());
                            item.Content = new TextSyndicationContent(tweet.Text);
                            item.PublishDate = tweet.CreatedAt;
                            item.BaseUri = new Uri(tweet.Url);
                            item.Id = tweet.Id.ToString();
                            item.Authors.Add(syndicationPerson);
                            item.Links.Add(syndicationLink);

                            rssItems.Add(item);
                        }
                    }
                }
            }
            feed.Items = rssItems;
            return feed;
        }
    }
}