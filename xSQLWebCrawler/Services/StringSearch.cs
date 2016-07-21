using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSQLWebCrawler.Services
{
    static class StringSearch
    {
        /// <summary>
        /// Searches a string using the Boyer Moore string search algorithm
        /// </summary>
        /// <param name="pattern">Pattern to seach for</param>
        /// <param name="searchText">Text to search in</param>
        /// <returns>Index of the first occurence</returns>
        public static int BoyerMooreSearch(string pattern, string searchText)
        {
            throw new NotImplementedException("Method not implemented");
        }

        /// <summary>
        /// Finds the shift amount using the bad character rule
        /// </summary>
        /// <param name="pattern">Pattern being looked for in the string</param>
        /// <param name="mismatch">Character in which the mismatch happened</param>
        /// <returns>Amount by which the pattern needs to be shifted</returns>
        public static int BadCharacterRuleShift(string pattern, char mismatch)
        {
            throw new NotImplementedException("Method not implemented");
        }

        /// <summary>
        /// Finds the shift amount using the good suffix rule
        /// </summary>
        /// <param name="pattern">Pattern being looked for in a string</param>
        /// <param name="matchedPart">Pattern suffix that has been matched</param>
        /// <param name="mismatch">Character in which the missmatch happened</param>
        /// <returns>Amount by which the pattern needs to be shifted</returns>
        public static int GoodSuffixRuleShift(string pattern, string matchedPart, char mismatch)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public static int[] GenerateBadCharacterRuleTable(string pattern)
        {
            int[] badCharRuleTable = new int[237];
            for (int i = 0; i < badCharRuleTable.Length; i++)
            {
                badCharRuleTable[i] = pattern.Length;
            }
            //char[] patternArray = pattern.ToCharArray();
            byte[] asciiCodesArray = Encoding.ASCII.GetBytes(pattern);
            for (int i = 0; i < pattern.Length; i++)
            {
                badCharRuleTable[asciiCodesArray[i]] = Math.Max(1, pattern.Length - 1 - i);                    
            }
            return badCharRuleTable;
        }

        public static int[] GenerateGoodSuffixRuleTable(string pattern)
        {
            throw new NotImplementedException("Method not yet implemented");
        }
    }
}
