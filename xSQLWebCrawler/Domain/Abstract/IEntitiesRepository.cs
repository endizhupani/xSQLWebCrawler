using System.Collections.Generic;
using System.Threading.Tasks;
using xSQLWebCrawler.Domain.Entities;

namespace xSQLWebCrawler.Domain.Abstract
{
    interface IEntitiesRepository
    {
        IEnumerable<Site> Sites { get; }
        IEnumerable<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; }
        Task<bool> SaveChanges();
    }
}
