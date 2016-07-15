using Abot.Crawler;
using Abot.Poco;
using log4net;
using System;
using System.Net;

namespace xSQLWebCrawler.Services
{
    class CrawlServices
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Starts the crawling
        /// </summary>
        public void DoCrawl()
        {
            //set up crawler
            PoliteWebCrawler crawler = new PoliteWebCrawler();
            crawler.ShouldCrawlPage((pageToCrawl, crawlContext) =>
            {
                return this.ShouldCrawl(pageToCrawl, crawlContext);    
            });


            //attach events
            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
            CrawlResult result = crawler.Crawl(new Uri("http://stackoverflow.com")); //This is synchronous, it will not go to the next line until the crawl has completed



            if (result.ErrorOccurred)
                logger.Error(String.Format("Crawl of {0} completed with error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.Message));
            else
                logger.Info(String.Format("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri));
        }



        /// <summary>
        /// Specifies actions to be undertaken before the crawling of a site starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            logger.Info(String.Format("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri));
        }


        /// <summary>
        /// Specifies actions to be undertaken after the crawling of a site is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                logger.Error(String.Format("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri));
            else
                logger.Info(String.Format("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri));

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                logger.Info(String.Format("Page had no content {0}", crawledPage.Uri.AbsoluteUri));
        }



        /// <summary>
        /// Specifies actions to be undertaken if crawling the links of a page is not allowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            logger.Info(String.Format("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason));
        }


        /// <summary>
        /// Specifies actions to be undertaken if crawling the page is not allowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            logger.Info(String.Format("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason));
        }



        protected CrawlDecision ShouldCrawl(PageToCrawl pageToCrawl, CrawlContext crawlContext)
        {
            CrawlDecision decision = new CrawlDecision { Allow = true };
            if (pageToCrawl.Uri.Authority == "google.com")
                return new CrawlDecision { Allow = false, Reason = "Dont want to crawl google pages" };

            return decision;
        }
    }
}
