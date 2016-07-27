using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSQLWebCrawler.Helpers;

namespace xSQLWebCrawler.Services
{
    public static class StringSearch
    {
        /// <summary>
        /// Searches a string using the Boyer Moore string search algorithm
        /// </summary>
        /// <param name="pattern">Pattern to seach for</param>
        /// <param name="searchText">Text to search in</param>
        /// <returns>Index of the first occurence</returns>
        public static int BoyerMooreSearch(this string searchText, string pattern)
        {
            pattern = pattern.ToLower();
            searchText = searchText.ToLower();
            char[] searchTextArray = searchText.ToCharArray();
            char[] patternArray = pattern.ToCharArray();
            int[] badCharacterRuleTable = BoyerMooreHelpers.GenerateBadCharacterRuleTable(pattern);
            int[] widestBorderPosition = new int[pattern.Length + 1];
            int[] goodSuffixRuleTable = BoyerMooreHelpers.GenerateGoodSuffixRuleCase1Table(patternArray,ref widestBorderPosition);
            int[] goodSuffixRuleTableFinal = BoyerMooreHelpers.GenerateGoodSuffixRuleCase2Table(pattern, widestBorderPosition, goodSuffixRuleTable);
            int matchStartIndex = -1;
            int i = 0, j;
            int n = searchTextArray.Length;
            int m = patternArray.Length;
            bool found = false;
            while (i <= n - m && !found)
            {
                j = m - 1;
                while (j >= 0 && patternArray[j] == searchTextArray[i + j]) j--;
                if (j < 0)
                {
                    matchStartIndex = i;
                    found = true;
                    break;
                }
                else
                    i += Math.Max(goodSuffixRuleTableFinal[j + 1], j - badCharacterRuleTable[searchTextArray[i + j]]);
            }
            return matchStartIndex;
        }

        /// <summary>
        /// Boyer Moorse search with a preprocessed pattern
        /// </summary>
        /// <param name="searchText">Text to be searched</param>
        /// <param name="pattern">Pattern to search for</param>
        /// <returns>Index of the first match</returns>
        public static int BoyerMooreSearch(this string searchText, PreProcessedWord pattern)
        {          
            searchText = searchText.ToLower();
            char[] searchTextArray = searchText.ToCharArray();
            char[] patternArray = pattern.Word.ToCharArray();
            int matchStartIndex = -1;
            int i = 0, j;
            int n = searchTextArray.Length;
            int m = patternArray.Length;
            bool found = false;
            while (i <= n - m && !found)
            {
                j = m - 1;
                while (j >= 0 && patternArray[j] == searchTextArray[i + j]) j--;
                if (j < 0)
                {
                    matchStartIndex = i;
                    found = true;
                    break;
                }
                else
                    i += Math.Max(pattern.GoodSuffixRuleArray[j + 1], j - pattern.BadCharRuleArray[searchTextArray[i + j]]);
            }
            return matchStartIndex;
        }

        

    }
}
