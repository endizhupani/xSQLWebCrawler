using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace xSQLWebCrawler.Domain.Entities
{
    public class xSQLCrawlerContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; set; }
    }
}
