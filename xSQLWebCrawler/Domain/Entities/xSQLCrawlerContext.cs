
using System.Data.Entity;


namespace xSQLWebCrawler.Domain.Entities
{
    public class xSQLCrawlerContext : DbContext
    {
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; set; }      
        public DbSet<KeywordCombination> KeywordCombinations { get; set; }
        public DbSet<ProccessedLink> ProccessedLinks { get; set; }

    }
}
