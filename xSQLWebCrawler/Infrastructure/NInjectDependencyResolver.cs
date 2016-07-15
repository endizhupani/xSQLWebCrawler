using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSQLWebCrawler.Domain.Abstract;
using Ninject;
using xSQLWebCrawler.Domain.Concrete;

namespace xSQLWebCrawler.Infrastructure
{
    class NInjectDependencyResolver
    {
        private IKernel kernel;
        public NInjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
        }
        public void AddBindings() {
            //bindings go here...
            kernel.Bind<IEntitiesRepository>().To<EntitiesRepository>().InSingletonScope();
        }
    }
}
