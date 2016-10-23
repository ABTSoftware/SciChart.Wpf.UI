using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours.Transitionz
{
    public interface IBlurParams : ITransitionParams<double>
    {
    }

    public class BlurParams : TransitionzParams<double>, IBlurParams
    {
    }

    [MarkupExtensionReturnType(typeof(IBlurParams))]
    public class BlurParamsExtension : BaseTransitionzExtension<double>, IOpacityParams
    {
        public BlurParamsExtension() { }

        public BlurParamsExtension(double beginTime, double duration, double from, double to, EasingFunctionBase ease, EasingFunctionBase reverseEase, TransitionOn transitionOn, bool autoReverse)
            : base(beginTime, duration, from, to, ease, reverseEase, transitionOn, autoReverse)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
