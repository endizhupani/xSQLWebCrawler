using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static int BoyerMooreSearch(string pattern, string searchText)
        {
            pattern = pattern.ToLower();
            searchText = searchText.ToLower();
            char[] searchTextArray = searchText.ToCharArray();
            char[] patternArray = pattern.ToCharArray();
            int[] badCharacterRuleTable = GenerateBadCharacterRuleTable(pattern);
            int[] widestBorderPosition = new int[pattern.Length + 1];
            int[] goodSuffixRuleTable = GenerateGoodSuffixRuleCase1Table(patternArray,ref widestBorderPosition);
            int[] goodSuffixRuleTableFinal = GenerateGoodSuffixRuleCase2Table(pattern, widestBorderPosition, goodSuffixRuleTable);
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
        /// Generates the bad character rule table for the pattern
        /// </summary>
        /// <param name="pattern">Pattern to be searched for</param>
        /// <returns>An array representing the bad char rule table</returns>
        public static int[] GenerateBadCharacterRuleTable(string pattern)
        {
            int[] badCharRuleTable = new int[237];
            int a;
            int j;
            byte[] asciiCodesArray = Encoding.ASCII.GetBytes(pattern);
            for (a = 0; a < badCharRuleTable.Length; a++)
                badCharRuleTable[a] = -1;

            for (j = 0; j < pattern.Length; j++)
            {
                a = asciiCodesArray[j];
                badCharRuleTable[a] = j;
            }
            return badCharRuleTable;
        }

        /// <summary>
        /// Generates the good suffix rule table for the first case (the whole matching part is found on the pattern) 
        /// </summary>
        /// <param name="pattern">Pattern to be searched for</param>
        /// <param name="widestBorderPos">an array containing the start indices of the widest border of a suffix starting at position i</param>
        /// <returns>The good suffix rule table for the first case</returns>
        public static int[] GenerateGoodSuffixRuleCase1Table(char[] pattern,ref int[] widestBorderPos)
        {
            //char[] patternArray = pattern.ToCharArray(); 
            widestBorderPos = new int[pattern.Length + 1];
            int[] shiftArray = new int[pattern.Length + 1];
            for (int index = 0; index < shiftArray.Length; index++)
            {
                shiftArray[index] = 0;
            }
            int i = pattern.Length;
            int j = i + 1;
            widestBorderPos[i] = j;
            while (i > 0)
            {
                while (j <= pattern.Length && pattern[i - 1] != pattern[j - 1])
                {
                    if (shiftArray[j] == 0)
                        shiftArray[j] = j - i;
                    j = widestBorderPos[j];
                }
                i--;
                j--;
                widestBorderPos[i] = j;
            }
            return shiftArray;
        }


        /// <summary>
        /// Generates the good table rule for the second case (a suffix of the matching part is found as a prefix of the pattern)
        /// </summary>
        /// <param name="pattern">Pattern to be searched for</param>
        /// <param name="widestBorderPos">an array containing the start indices of the widest border of a suffix starting at position i</param>
        /// <param name="shiftArray">The good suffix rule table from proccessing for the first case</param>
        /// <returns>The final good suffix rule table</returns>
        public static int[] GenerateGoodSuffixRuleCase2Table(string pattern, int[] widestBorderPos, int[] shiftArray)
        {
            int i, j;
            j = widestBorderPos[0];
            //shiftArray = new int[pattern.Length + 1];
            for (i = 0; i <= pattern.Length; i++)
            {
                if (shiftArray[i] == 0)
                    shiftArray[i] = j;
                if (i == j)
                    j = widestBorderPos[j];
            }
            return shiftArray;            
        }
    }
}
