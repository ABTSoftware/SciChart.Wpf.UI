using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.UI.Reactive.Tests.QualityTools;
using Abt.Framework.Core.Workflow;
using NUnit.Framework;

namespace Abt.Framework.Core.Tests.Async
{
    public class SimpleWorkflowTests
    {
        public class HelloActivity : BaseSimpleActivity
        {
            protected override object DoWork(object input)
            {
                return "Hello";
            }
        }

        public class WorldActivity : BaseSimpleActivity
        {
            protected override object DoWork(object input)
            {
                return ((string)input) + " World";
            }
        }

        [Test]
        public void ShouldExecuteSimpleOneStepWorkflow()
        {
            // Arrange
            var wf = new SimpleWorkflow(new TestSchedulerContext());
            wf.AddStep(new HelloActivity());

            // Act
            var result = wf.Run(null).Result;

            Assert.That(result.WorkflowState, Is.EqualTo(SimpleWorkflowState.Completed));
            Assert.That(result.Result, Is.EqualTo("Hello"));
        }

        [Test]
        public void ShouldExecuteSimpleMultiStepWorkflow()
        {
            // Arrange
            var wf = new SimpleWorkflow(new TestSchedulerContext());
            wf.AddStep(new HelloActivity());
            wf.AddStep(new WorldActivity());

            // Act
            var result = wf.Run(null).Result;

            Assert.That(result.WorkflowState, Is.EqualTo(SimpleWorkflowState.Completed));
            Assert.That(result.Result, Is.EqualTo("Hello World"));
        }
    }
}
