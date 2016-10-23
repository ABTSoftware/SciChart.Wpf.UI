using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    public interface IScaleParams
    {
        double BeginTime { get; set; }
        double Duration { get; set; }
        Point From { get; set; }
        Point To { get; set; }
        EasingFunctionBase Ease { get; set; }
        FillBehavior FillBehavior { get; set; }
        TransitionOn TransitionOn { get; set; }
        bool AutoReverse { get; set; }
    }

    public class ScaleParams : IScaleParams
    {
        public double BeginTime { get; set; }
        public double Duration { get; set; }
        public Point From { get; set; }
        public Point To { get; set; }
        public EasingFunctionBase Ease { get; set; }
        public FillBehavior FillBehavior { get; set; }
        public TransitionOn TransitionOn { get; set; }
        public bool AutoReverse { get; set; }
    }

    [MarkupExtensionReturnType(typeof(IScaleParams))]
    public class ScaleParamsExtension : BaseTransitionzExtension<Point>, ITranslateParams
    {
        public ScaleParamsExtension()
        {
        }

        public ScaleParamsExtension(double beginTime, double duration, Point from, Point to, EasingFunctionBase ease, TransitionOn transitionOn, bool autoReverse)
            : base(beginTime, duration, from, to, ease, null, transitionOn, autoReverse)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}