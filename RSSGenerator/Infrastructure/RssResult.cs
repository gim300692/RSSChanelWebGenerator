using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RSSGenerator.Infrastructure
{
    public class RssResult:ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public RssResult(SyndicationFeed feed)
        {
            Feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";
            Rss20FeedFormatter formatter = new Rss20FeedFormatter(Feed);

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                
                writer.WriteProcessingInstruction("xml-stylesheet", "href='https://dl.dropbox.com/s/absgpv0mnawgrl0/rss.css' type='text/css'");


                if (writer != null)
                {
                    formatter.WriteTo(writer);
                }
            }
        }
    }
}
