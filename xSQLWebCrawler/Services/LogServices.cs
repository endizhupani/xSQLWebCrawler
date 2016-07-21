using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSQLWebCrawler.Domain.Entities;

namespace xSQLWebCrawler.Services
{

    static class LogServices
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogForbiddenPatterns(List<ForbiddenSearchPatern> patternList)
        {
            foreach (ForbiddenSearchPatern fp in patternList)
            {
                string message = String.Format("Patterns not allowed to crawl: {0}", fp.RegEx);
                logger.Info(message);
            }
        }
    }
}
