using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowCore.IntegrationTests.Scenarios
{
    public abstract class BaseScenario : IDisposable
    {
        protected IWorkflowHost Host;
        protected IPersistenceProvider PersistenceProvider;

        public BaseScenario()
        {            
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Debug);
            
            PersistenceProvider = serviceProvider.GetService<IPersistenceProvider>();
            Host = serviceProvider.GetService<IWorkflowHost>();
            RegisterWorkflows();
            Host.Start();
        }

        protected abstract void RegisterWorkflows();

        public void Dispose()
        {
            Host.Stop();
        }
    }
}
