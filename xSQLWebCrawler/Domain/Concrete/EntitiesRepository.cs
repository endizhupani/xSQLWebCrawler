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
    }
}
