using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace xSQLWebCrawler.Domain.Entities
{
    /// <summary>
    /// Specifies link patterns for each site. Links containing this patterns will not be crawled
    /// </summary>
    public class ForbiddenSearchPatern
    {
        [Key]
        [Required]
        public int ForbiddenSearchPatternId { get; set; }
        public string RegEx { get; set; }
        public int SiteId { get; set; }
        [ForeignKey("SiteId")]
        public Site Site { get; set; }
    }
}
