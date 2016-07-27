using Abot.Crawler;
using Abot.Poco;
using HtmlAgilityPack;
using log4net;
using Ninject;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Domain.Entities;
using xSQLWebCrawler.Infrastructure;

namespace xSQLWebCrawler.Services
{
    class CrawlServices
    {
        private IEntitiesRepository repository;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ForbiddenSearchPatern> siteForbiddenPatterns = null;
        private List<ProccessedLink> lastProcessedLinks = null;
        [ThreadStatic]
        private static bool foundOldPost = false;
        //private Uri siteCurrentlyBeingCrawled = null;

        public CrawlServices()
        {
            IKernel kernel = new StandardKernel();
            NInjectDependencyResolver depResolver = new NInjectDependencyResolver(kernel);
            depResolver.AddBindings();
            repository = kernel.Get<IEntitiesRepository>();
            lastProcessedLinks = repository.GetProcessedLinks(10);
        }

        /// <summary>
        /// Starts the crawling
        /// </summary>
        public async void DoCrawl()
        {
            List<Site> sitesToCrawl = await repository.GetSitesAsync(true, true);
            foreach (Site s in sitesToCrawl)
            {
                siteForbiddenPatterns = s.ForbiddenSearchPatterns.ToList();
                LogServices.LogForbiddenPatterns(siteForbiddenPatterns);
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

                
                foundOldPost = false;
                CrawlResult result = crawler.Crawl(s.SiteUri); //This is synchronous, it will not go to the next line until the crawl has completed
                if (result.ErrorOccurred)
                    logger.Error(String.Format("Crawl of {0} was not completed. Error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.Message));
                else
                    logger.Info(String.Format("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri));
            }
        }

        /// <summary>
        /// Specifies actions to be undertaken before the crawling of a site starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            e.CrawlContext.IsCrawlStopRequested = false;
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
            {
                logger.Info(String.Format("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri));
                HtmlDocument pageHtml = crawledPage.HtmlDocument;
                foundOldPost = ScrapeServices.IsOldPost(pageHtml, DateTime.Now.AddMonths(0 - Properties.Settings.Default.MaxNumberOfMonthsToCheckQuestions));
                if (foundOldPost)
                {
                    e.CrawlContext.IsCrawlStopRequested = true;
                }
                //else {
                //    e.CrawlContext.IsCrawlStopRequested = false;
                //}
            }

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
            ForbiddenSearchPatern patternFound = null;
            if (this.HasForbiddenPatterns(pageToCrawl.Uri.ToString(), out patternFound))
            {
                return new CrawlDecision { Allow = false, Reason = String.Format("The Uri contains one of the forbidden patterns (pattern: {0})", patternFound.RegEx) };
            }
            if (foundOldPost)
            {
                return new CrawlDecision { Allow = false, Reason = String.Format("Posts were found that were more than {0} months old", Properties.Settings.Default.MaxNumberOfMonthsToCheckQuestions) };
            }
            if (HasBeenProccessed(pageToCrawl.Uri.ToString(), lastProcessedLinks))
            {
                return new CrawlDecision { Allow = false, Reason = String.Format("Links beyond this point have already been processed") };
            }
            

            return decision;
        }

        /// <summary>
        /// Checks a string to see if it contains one of the patterns in the DB.
        /// </summary>
        /// <param name="stringToCheck">string to check for patterns</param>
        /// <param name="patternFound">takes the value of the pattern found in the string</param>
        /// <returns>True if a pattern is matched, false otherwise</returns>
        protected bool HasForbiddenPatterns(string stringToCheck, out ForbiddenSearchPatern patternFound)
        {

            foreach (ForbiddenSearchPatern pattern in siteForbiddenPatterns)
            {

                Regex regEx = new Regex(pattern.RegEx,
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                if (regEx.IsMatch(stringToCheck))
                {
                    patternFound = pattern;
                    return true;
                }
            }
            patternFound = null;
            return false;
        }

        /// <summary>
        /// Checks if a link is in the list of processed links
        /// </summary>
        /// <param name="link">Link to be checked</param>
        /// <param name="processedLinks">List of links to be checked against.</param>
        /// <returns></returns>
        protected bool HasBeenProccessed(string link, List<ProccessedLink> processedLinks)
        {
            foreach (ProccessedLink pLink in processedLinks)
            {
                if (pLink.StrUri == link)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
