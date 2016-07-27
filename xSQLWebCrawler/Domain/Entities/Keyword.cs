using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSQLWebCrawler.Domain.Entities
{
    public class Keyword
    {
        [Key]
        [Required]
        public int KeywordId { get; set; }
        [Required]
        public string KeywordText { get; set; }
        public ICollection<KeywordCombination> KeywordCombinations { get; set; }

    }
}
