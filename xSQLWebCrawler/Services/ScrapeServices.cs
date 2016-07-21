using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xSQLWebCrawler.Services
{
    static class ScrapeServices
    {
        /// <summary>
        /// Checks if the date of the post's content is before the start date
        /// </summary>
        /// <param name="htmlPage">Post's content</param>
        /// <param name="startDate"></param>
        /// <returns>true if the post has a date before the start date, false otherwise</returns>
        public static bool IsOldPost(HtmlDocument htmlPage, DateTime startDate)
        {
            string[] datePatterns = {
                @"\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])",
                @"\d{4}\-(0?[1-9]|[12][0-9]|3[01])\-(0?[1-9]|1[012])",
                @"(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])-\d{4}",
                @"(0?[1-9]|[12][0-9]|3[01])\-(0?[1-9]|1[012])-\d{4}",
            };
            foreach (string datePattern in datePatterns)
            {
                Regex regEx = new Regex(datePattern,
                        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                HtmlNodeCollection pNodes = htmlPage.DocumentNode.SelectNodes("//p");
                if (pNodes != null)
                {
                    foreach (HtmlNode paragraph in pNodes)
                    {
                        var date = regEx.Match(paragraph.OuterHtml);
                        if (date != null)
                        {
                            string dateStr = date.Groups[0].Value;
                            if (!String.IsNullOrEmpty(dateStr))
                            {
                                DateTime dateFound = Convert.ToDateTime(dateStr);
                                if (dateFound < startDate)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                HtmlNodeCollection spanNodes = htmlPage.DocumentNode.SelectNodes("//span");
                if (spanNodes != null)
                {
                    foreach (HtmlNode span in spanNodes)
                    {
                        var date = regEx.Match(span.OuterHtml);
                        if (date != null)
                        {
                            string dateStr = date.Groups[0].Value;
                            if (!String.IsNullOrEmpty(dateStr))
                            {
                                DateTime dateFound = Convert.ToDateTime(dateStr);
                                if (dateFound < startDate)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        

        /// <summary>
        /// Checks a HTML page for the specified keywords
        /// </summary>
        /// <param name="keyWords">A list of keywords to look for</param>
        /// <param name="page">The HTML page to search for keywords</param>
        /// <param name="keyWordsFound">A list of keywords found in the page</param>
        /// <returns>True if any of the keywords were found, false otherwise</returns>
        public static bool CheckForKeyWords(List<string> keyWords, HtmlDocument page, out List<string> keyWordsFound)
        {
            HtmlNode body = page.DocumentNode.SelectSingleNode("//body");
            foreach (string keyword in keyWords)
            {

            }
            throw new NotImplementedException("This method is not yet implemented");
        }


    }
}
