using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xSQLWebCrawler.Helpers;

namespace xSQLWebCrawler.Services
{
    public static class ScrapeServices
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
        /// Checks is a HTML page has all the specified keywords
        /// </summary>
        /// <param name="keyWords">A list of keywords to look for</param>
        /// <param name="page">The HTML page to search for keywords</param>
        /// <returns>True if all of the keywords were found, false otherwise</returns>
        public static bool HasAllKeyWords(this HtmlDocument page, List<PreProcessedWord> keyWords)
        {
            HtmlNode body = page.DocumentNode.SelectSingleNode("//body");
            string bodyStr = body.InnerHtml;
            foreach (PreProcessedWord keyword in keyWords)
            {
                if (bodyStr.BoyerMooreSearch(keyword) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a HTML page has any of the specified keywords
        /// </summary>
        /// <param name="keyWords">A list of keywords to look for</param>
        /// <param name="page">The HTML page to search for keywords</param>
        /// <returns>True if any of the keywords were found, false otherwise</returns>
        public static bool HasAnyKeyWord(this HtmlDocument page, List<PreProcessedWord> keyWords)
        {
            HtmlNode body = page.DocumentNode.SelectSingleNode("//body");
            string bodyStr = body.InnerHtml;
            foreach (PreProcessedWord keyword in keyWords)
            {
                if (bodyStr.BoyerMooreSearch(keyword) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
