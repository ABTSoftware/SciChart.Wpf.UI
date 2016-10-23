#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    public interface IOpacityParams : ITransitionParams<double>
    {
    }

    public class OpacityParams : TransitionzParams<double>, IOpacityParams
    {
    }

    [MarkupExtensionReturnType(typeof(IOpacityParams))]
    public class OpacityParamsExtension : BaseTransitionzExtension<double>, IOpacityParams
    {
        public OpacityParamsExtension() { }

        public OpacityParamsExtension(double beginTime, double duration, double from, double to, EasingFunctionBase ease, EasingFunctionBase reverseEase, TransitionOn transitionOn, bool autoReverse)
            : base(beginTime, duration, from, to, ease, reverseEase, transitionOn, autoReverse)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}