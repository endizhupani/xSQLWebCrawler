using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSQLWebCrawler.Domain.Abstract;
using xSQLWebCrawler.Domain.Entities;
using xSQLWebCrawler.Infrastructure;

namespace xSQLWebCrawler.Helpers
{
    public class LinkOfInterest
    {
        public Uri Uri { get; set; }
        public List<string> WordsFound { get; set; }
    }
    public class ReportingHelpers
    {
        IEntitiesRepository repository;
        public ReportingHelpers()
        {
            IKernel kernel = new StandardKernel();
            NInjectDependencyResolver depResolver = new NInjectDependencyResolver(kernel);
            depResolver.AddBindings();
            repository = kernel.Get<IEntitiesRepository>();
        }

        /// <summary>
        /// Saves links to the database
        /// </summary>
        /// <param name="linksToSave"></param>
        public async void SaveLinksToDatabase(List<LinkOfInterest> linksToSave)
        {
            foreach(LinkOfInterest l in linksToSave)
            {
                await repository.AddOrUpdateProcessedLinkAsync(
                    new ProccessedLink {
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        StrUri = l.Uri.ToString(),
                        Status = Status.Pending
                    }
                    );
            }
            await repository.SaveChangesAsync();
        }

        /// <summary>
        /// Saves links to a file
        /// </summary>
        /// <param name="linksToSave">Links to be saven in the file</param>
        /// <param name="filepath">The physical path of the file</param>
        public void SaveLinksToFile(List<LinkOfInterest> linksToSave, string filepath)
        {
            throw new NotImplementedException("Not yet implemented");
        }

        /// <summary>
        /// Sends the list of links by email
        /// </summary>
        /// <param name="linksToSave">Links to be sent</param>
        /// <param name="sendTo">Email address of the reciever</param>
        public void SendLinksByEmail(List<LinkOfInterest> linksToSave, string sendTo)
        {
            throw new NotImplementedException("Not yet implemented");
        }
    }
}
