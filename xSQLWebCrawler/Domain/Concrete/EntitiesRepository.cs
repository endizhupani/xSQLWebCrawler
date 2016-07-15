using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Domain.Entities;
using log4net;

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
        /// Saves changes to the database
        /// </summary>
        /// <returns>True of False depending on whether it successfully saved the data</returns>
        public async Task<bool> SaveChanges()
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
