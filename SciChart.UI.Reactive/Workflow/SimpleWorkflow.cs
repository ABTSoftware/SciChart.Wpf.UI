using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Async;

namespace Abt.Framework.Core.Workflow
{
    public interface ISimpleActivity
    {
        Task<SimpleWorkflowResult> Start(ISchedulerContext schedulerContext, object input);
        void AddStep(ISimpleActivity workflowStep);
    }

    public abstract class BaseSimpleActivity : ISimpleActivity
    {
        private ISimpleActivity _subsequentStep;

        protected BaseSimpleActivity()
        {            
        }

        public SimpleWorkflowResult TaskBody(object input)
        {
            try
            {
                object result = DoWork(input);
                return new SimpleWorkflowResult(SimpleWorkflowState.Completed, result);
            }
            catch (Exception ex)
            {
                return new SimpleWorkflowResult(SimpleWorkflowState.Faulted, ex);
            }
        }

        public Task<SimpleWorkflowResult> Start(ISchedulerContext schedulerContext, object input)
        {
            var s = schedulerContext.Default;

            var task = Task.Factory.StartNew(() => TaskBody(input), s);
            if (_subsequentStep != null)
            {                
                task.Then(r => _subsequentStep.Start(schedulerContext, r.Result), s);                
            }
            return task;
        }

        protected abstract object DoWork(object input);        

        public void AddStep(ISimpleActivity workflowStep)
        {
            _subsequentStep = workflowStep;
        }
    }

    public enum SimpleWorkflowState
    {
        Completed, 
        Faulted
    }

    public class SimpleWorkflowResult
    {
        public SimpleWorkflowResult(SimpleWorkflowState state, object result = null)
        {
            WorkflowState = state;
            Result = result;
        }

        public SimpleWorkflowState WorkflowState { get; set; }

        public object Result { get; set; }        
    }

    public class SimpleWorkflow
    {
        private readonly ISchedulerContext _schedulerContext;

        private ISimpleActivity _root;

        public SimpleWorkflow(ISchedulerContext schedulerContext)
        {
            _schedulerContext = schedulerContext;
        }

        public SimpleWorkflow AddStep(ISimpleActivity workflowStep)
        {
            if (_root == null) _root = workflowStep;
            else _root.AddStep(workflowStep);
            return this;
        }

        public Task<SimpleWorkflowResult> Run(object input)
        {
            if (_root == null)
            {
                return TaskEx.FromResult(new SimpleWorkflowResult(SimpleWorkflowState.Completed));
            }

            var buildTask = _root.Start(_schedulerContext, input);
            return buildTask;
        }
    }
}
