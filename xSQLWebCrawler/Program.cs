using log4net;
using log4net.Config;
using Ninject;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Infrastructure;
using xSQLWebCrawler.Domain.Entities;
using System;
using xSQLWebCrawler.Services;

namespace xSQLWebCrawler
{
    class Program
    {
        private IEntitiesRepository repository;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Program(IEntitiesRepository repositoryParam)
        {
            this.repository = repositoryParam;
        }
        public Program()
        {
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            //configure log4net           
            XmlConfigurator.Configure();

            //inject dependencies
            IKernel kernel = new StandardKernel();
            NInjectDependencyResolver depResolver = new NInjectDependencyResolver(kernel);
            depResolver.AddBindings();
            //Site site = new Site
            //{
            //    Name = "stackoverflow",
            //    Uri = "http://stackoverflow.com",
            //};

            //ForbiddenSearchPatern pattern = new ForbiddenSearchPatern
            //{
            //    RegEx = @"\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico",
            //    Site = site,
            //};



            //start the crawl
            CrawlServices crawler = new CrawlServices();
            crawler.DoCrawl();
        }
    }
}

