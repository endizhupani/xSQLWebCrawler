using log4net;
using log4net.Config;
using Ninject;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Infrastructure;
using xSQLWebCrawler.Services;

namespace xSQLWebCrawler
{
    class Program
    {
        private IEntitiesRepository repository;
        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Program(IEntitiesRepository repositoryParam)
        {
            this.repository = repositoryParam;
        }
        public Program()
        {
        }
        static void Main(string[] args)
        {
            //configure log4net           
            XmlConfigurator.Configure();

            //inject dependencies
            IKernel kernel = new StandardKernel();
            NInjectDependencyResolver depResolver = new NInjectDependencyResolver(kernel);
            depResolver.AddBindings();

            //start the crawl
            CrawlServices crawler = new CrawlServices();
            crawler.DoCrawl();
        }      
    }
}

