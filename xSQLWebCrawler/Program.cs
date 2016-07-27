using log4net;
using log4net.Config;
using Ninject;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Infrastructure;
using xSQLWebCrawler.Domain.Entities;
using System;
using xSQLWebCrawler.Services;
using System.IO;

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

            //start the crawl
            CrawlServices crawler = new CrawlServices();
            crawler.DoCrawl();

            //string pattern = "abbabab";
            //int[] widestBorderPosition = new int[pattern.Length+1];
            ////Act
            //int[] shift = StringSearch.GenerateGoodSuffixRuleCase1Table(pattern, widestBorderPosition);
            //int[] shift2 = StringSearch.GenerateGoodSuffixRuleCase2Table(pattern, widestBorderPosition, shift);
            //for (int i = 0; i < shift.Length; i++)
            //{
            //    Console.WriteLine("Index: " + i + " Value: " + shift[i]);
            //}
            //for (int i = 0; i < shift2.Length; i++)
            //{
            //    Console.WriteLine("Index: " + i + " Value: " + shift2[i]);
            //}
            //string text = File.ReadAllText("C:\\Users\\user\\documents\\visual studio 2015\\Projects\\xSQLWebCrawler\\xSQLWebCrawler\\SearchTest.txt");
            ////string pattern = "EnDiZHupani19940609";
            //string pattern = "-07-20 11:38:";

            //int milisesStart = DateTime.Now.Millisecond;
            //int index = StringSearch.BoyerMooreSearch(pattern, text);
            //int difference1 = DateTime.Now.Millisecond - milisesStart;

            //milisesStart = DateTime.Now.Millisecond;
            //int indexNormalSearch = text.IndexOf(pattern);
            //int diff2 = DateTime.Now.Millisecond - milisesStart;
            //Console.WriteLine("Time required by Boyer-Moore search: " + difference1 + " milliseconds");
            //Console.WriteLine("Time required by normal search: " + diff2 + " milliseconds");
            //Console.ReadKey();

        }
    }
}

