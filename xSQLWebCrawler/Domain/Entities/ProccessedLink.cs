using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSQLWebCrawler.Domain.Entities
{
    public class ProccessedLink
    {
        [Key]
        [Required]
        public int ProccessedLinkId { get; set; }
        public string StrUri { get; set; }
        [NotMapped]
        public Uri Uri {
            get {
                return new Uri(StrUri);
            }
        }
        public Status Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    public enum Status { Responded, Irrelevant, Pending }
}
