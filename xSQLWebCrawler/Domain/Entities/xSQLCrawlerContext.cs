using System.Data.Entity;


namespace xSQLWebCrawler.Domain.Entities
{
    public class xSQLCrawlerContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; set; }
    }
}
