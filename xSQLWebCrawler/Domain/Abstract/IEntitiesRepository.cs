using System.Collections.Generic;
using System.Threading.Tasks;
using xSQLWebCrawler.Domain.Entities;

namespace xSQLWebCrawler.Domain.Abstract
{
    interface IEntitiesRepository
    {
        IEnumerable<Site> Sites { get; }
        IEnumerable<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; }
        /// <summary>
        /// Adds or updates a pattern object
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateForbiddenSearchPatternAsync(ForbiddenSearchPatern pattern);
        /// <summary>
        /// Adds or updates a site object
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateSiteAsync(Site site);
        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>True if the opperation was successful, false otherwise</returns>
        Task<bool> SaveChangesAsync();
    }
}
