using System.Collections.Generic;
using System.Threading.Tasks;
using xSQLWebCrawler.Domain.Entities;

namespace xSQLWebCrawler.Domain.Abstract
{
    interface IEntitiesRepository
    {
        IEnumerable<Site> Sites { get; }
        IEnumerable<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; }
        IEnumerable<ProccessedLink> ProcessedLinks { get; }
        
        /// <summary>
        /// Get n last processed links from the database
        /// </summary>
        /// <param name="numberOfLinks">Number of last processed links to get. 0 to get all</param>
        /// <returns>A collection of the links.</returns>
        Task<List<ProccessedLink>> GetProcessedLinksAsync(int numberOfLinks = 0);
        /// <summary>
        /// Get n last processed links from the database
        /// </summary>
        /// <param name="numberOfLinks">Number of last processed links to get. 0 to get all</param>
        /// <returns>A collection of the links.</returns>
        List<ProccessedLink> GetProcessedLinks(int numberOfLinks = 0);
        /// <summary>
        /// Gets all the sites from the database.
        /// </summary>
        /// <param name="includeKeyWords">If true, get all the keyword combinations associated with the site</param>
        /// <param name="includeForbiddenSearchPatterns">If true, get all the forbidden search patterns associated with the site</param>
        /// <returns>Collection of sites</returns>
        Task<List<Site>> GetSitesAsync(bool includeKeyWords = false, bool includeForbiddenSearchPatterns = false);
        /// <summary>
        /// Adds or updates a processed link in the database
        /// </summary>
        /// <param name="link">The link to be added or updated</param>
        /// <returns>True if the operation was successfull, false otherwise</returns>
        Task<bool> AddOrUpdateProcessedLinkAsync(ProccessedLink link);
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
        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>True if the opperation was successful, false otherwise</returns>
        bool SaveChanges();
    }
}
