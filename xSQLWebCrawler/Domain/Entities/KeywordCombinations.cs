using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSQLWebCrawler.Domain.Entities
{
    public class KeywordCombination
    {
        [Key]
        [Required]
        public int KeywordCombinationId { get; set; }
        public string CombinationText { get; set; }
        /// <summary>
        /// Keyword combination
        /// </summary>
        public ICollection<Keyword> KeyWords { get; set; }
        /// <summary>
        /// Sites linked with this Combination
        /// </summary>
        public ICollection<Site> Sites { get; set; }


    }
}
