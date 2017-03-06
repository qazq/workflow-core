using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services;
using Xunit;
using FluentAssertions;

namespace WorkflowCore.IntegrationTests.Scenarios
{
    
    public class BasicWorkflowTest : BaseScenario
    {
        
        public class Step1 : StepBody
        {            
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                BasicWorkflow.Step1Ticker++;
                return ExecutionResult.Next();
            }
        }        

        class BasicWorkflow : IWorkflow
        {
            public static int Step1Ticker = 0;
            public static int Step2Ticker = 0;

            public string Id { get { return "BasicWorkflow"; } }
            public int Version { get { return 1; } }
            public void Build(IWorkflowBuilder<Object> builder)
            {
                builder
                    .StartWith<Step1>()
                    .Then(context =>
                    {
                        Step2Ticker++;
                        return ExecutionResult.Next();
                    });
                        
            }
        }

        protected override void RegisterWorkflows()
        {
            Host.RegisterWorkflow<BasicWorkflow>();
        }

        [Fact]
        public void Scenario()
        {
            //act
            string workflowId = Host.StartWorkflow("BasicWorkflow").Result;
            WorkflowInstance instance = PersistenceProvider.GetWorkflowInstance(workflowId).Result;
            int counter = 0;
            while ((instance.Status == WorkflowStatus.Runnable) && (counter < 60))
            {
                System.Threading.Thread.Sleep(500);
                counter++;
                instance = PersistenceProvider.GetWorkflowInstance(workflowId).Result;
            }

            //assert            
            instance.Status.ShouldBeEquivalentTo(WorkflowStatus.Complete);
            BasicWorkflow.Step1Ticker.ShouldBeEquivalentTo(1);
            BasicWorkflow.Step2Ticker.ShouldBeEquivalentTo(1);
        }        

    }
}
