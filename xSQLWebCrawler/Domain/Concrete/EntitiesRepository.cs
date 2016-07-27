using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Domain.Entities;
using log4net;
using System.Linq;
using System.Data.Entity;

namespace xSQLWebCrawler.Domain.Concrete
{
    class EntitiesRepository : IEntitiesRepository
    {
        private xSQLCrawlerContext context = new xSQLCrawlerContext();
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IEnumerable<ForbiddenSearchPatern> ForbiddenSearchPatterns
        {
            get
            {
                return context.ForbiddenSearchPatterns;
            }
        }

        public IEnumerable<Site> Sites
        {
            get
            {
                return context.Sites;
            }
        }

        public IEnumerable<ProccessedLink> ProcessedLinks
        {
            get
            {
                return context.ProccessedLinks;
            }
        }

        /// <summary>
        /// Get n last processed links from the database
        /// </summary>
        /// <param name="numberOfLinks">Number of last processed links to get. 0 to get all</param>
        /// <returns>A collection of the links.</returns>
        public async Task<List<ProccessedLink>> GetProcessedLinksAsync(int numberOfLinks = 0) {
            if (numberOfLinks < 0)
            {
                throw new ArgumentOutOfRangeException("number of links must be at least 0");
            }
            else if (numberOfLinks == 0)
            {
                return await context.ProccessedLinks.OrderByDescending(pl => pl.Created).ToListAsync();
            }
            else {
                return await context.ProccessedLinks.OrderByDescending(pl => pl.Created).Take(numberOfLinks).ToListAsync();
            }
        }

        public List<ProccessedLink> GetProcessedLinks(int numberOfLinks = 0) {
            if (numberOfLinks < 0)
            {
                throw new ArgumentOutOfRangeException("number of links must be at least 0");
            }
            else if (numberOfLinks == 0)
            {
                return context.ProccessedLinks.OrderByDescending(pl => pl.Created).ToList();
            }
            else
            {
                return context.ProccessedLinks.OrderByDescending(pl => pl.Created).Take(numberOfLinks).ToList();
            }
        }

        /// <summary>
        /// Gets all the sites from the database.
        /// </summary>
        /// <param name="includeKeyWords">If true, get all the keyword combinations associated with the site</param>
        /// <param name="includeForbiddenSearchPatterns">If true, get all the forbidden search patterns associated with the site</param>
        /// <returns>Collection of sites</returns>
        public async Task<List<Site>> GetSitesAsync(bool includeKeyWords = false, bool includeForbiddenSearchPatterns = false) {
            if (includeForbiddenSearchPatterns && includeKeyWords)
            {
                return await context.Sites.Include(s => s.ForbiddenSearchPatterns).Include(s => s.KeywordCombinations.Select(c => c.KeyWords)).ToListAsync();
            }
            else if (includeForbiddenSearchPatterns)
            {
                return await context.Sites.Include(s => s.ForbiddenSearchPatterns).ToListAsync();
            }
            else if (includeKeyWords)
            {
                return await context.Sites.Include(s => s.KeywordCombinations.Select(c => c.KeyWords)).ToListAsync();
            }
            return await context.Sites.ToListAsync();
        }
        /// <summary>
        /// Adds a site to the Sites collection
        /// </summary>
        /// <param name="site">The site to add to the collection</param>
        public async Task<bool> AddOrUpdateSiteAsync(Site site)
        {
            if (!String.IsNullOrEmpty(site.Name) && !String.IsNullOrEmpty(site.Uri))
            {
                Site dbSite = await context.Sites.FindAsync(site.SiteId);
                if (dbSite != null)
                {
                    dbSite = site;
                    context.Entry(site).State = EntityState.Modified;
                }
                else {
                    context.Sites.Add(site);
                }
                return true;
            }
            else {
                throw new ArgumentException("Values of the site are not valid");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateForbiddenSearchPatternAsync(ForbiddenSearchPatern pattern)
        {
            if (!String.IsNullOrEmpty(pattern.RegEx) && (pattern.Site != null || pattern.SiteId > 0))
            {
                ForbiddenSearchPatern dbPattern = await context.ForbiddenSearchPatterns.FindAsync(pattern.ForbiddenSearchPatternId);
                if (dbPattern != null)
                {
                    dbPattern = pattern;
                    context.Entry(pattern).State = EntityState.Modified;
                }
                else {
                    context.ForbiddenSearchPatterns.Add(pattern);
                }
                return true;
            }
            else {
                throw new ArgumentException("Values of the pattern are not valid");
            }
        }


        /// <summary>
        /// Adds or updates a processed link in the database
        /// </summary>
        /// <param name="link">The link to be added or updated</param>
        /// <returns>True if the operation was successfull, false otherwise</returns>
        public async Task<bool> AddOrUpdateProcessedLinkAsync(ProccessedLink link)
        {
            if (!String.IsNullOrEmpty(link.StrUri))
            {
                ProccessedLink dbLink = await context.ProccessedLinks.SingleOrDefaultAsync(pl => pl.ProccessedLinkId == link.ProccessedLinkId);
                if (dbLink != null)
                {
                    context.Entry(link).State = EntityState.Modified;
                }
                else
                {
                    context.ProccessedLinks.Add(link);
                }
                return true;
            }
            else {
                throw new ArgumentException("Invalid values for the properties");
            }
        }


        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>True of False depending on whether it successfully saved the data</returns>
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                logger.Error("An Exception occured while saving data to the database. Exception message - " + e.Message);
                if (e.InnerException != null)
                {
                    logger.Error("Inner Exception's message - " + e.InnerException.Message);
                }
                return false;
            }
        }

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>True of False depending on whether it successfully saved the data</returns>
        public bool SaveChanges() {
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                logger.Error("An Exception occured while saving data to the database. Exception message - " + e.Message);
                if (e.InnerException != null)
                {
                    logger.Error("Inner Exception's message - " + e.InnerException.Message);
                }
                return false;
            }
        }
    }
}
