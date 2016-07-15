using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace xSQLWebCrawler.Domain.Entities
{
    /// <summary>
    /// Entity for a site to be crawled
    /// </summary>
    public class Site
    {
        [Key]
        [Required]
        public int SiteId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        [NotMapped]
        public Uri SiteUri
        {
            get {
                return new Uri(Uri);
            }
        }
        public IEnumerable<ForbiddenSearchPatern> ForbiddenSearchPatterns { get; set; }
    }
}
