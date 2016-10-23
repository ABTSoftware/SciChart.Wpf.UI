#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    public interface ITranslateParams
    {
        double BeginTime { get; set; }
        double Duration { get; set; }
        Point From { get; set; }
        Point To { get; set; }
        EasingFunctionBase Ease { get; set; }
        EasingFunctionBase ReverseEase { get; set; }
        FillBehavior FillBehavior { get; set; }
        TransitionOn TransitionOn { get; set; }
        bool AutoReverse { get; set; }
    }

    public class TranslateParams : ITranslateParams
    {
        public double BeginTime { get; set; }
        public double Duration { get; set; }
        public Point From { get; set; }
        public Point To { get; set; }
        public EasingFunctionBase Ease { get; set; }
        public EasingFunctionBase ReverseEase { get; set; }
        public FillBehavior FillBehavior { get; set; }
        public TransitionOn TransitionOn { get; set; }
        public bool AutoReverse { get; set; }
    }

    [MarkupExtensionReturnType(typeof(ITranslateParams))]
    public class TranslateParamsExtension : BaseTransitionzExtension<Point>, ITranslateParams
    {
        public TranslateParamsExtension()
        {
        }

        public TranslateParamsExtension(double beginTime, double duration, Point from, Point to, EasingFunctionBase ease, EasingFunctionBase reverseEase, TransitionOn transitionOn, bool autoReverse)
            : base(beginTime, duration, from, to, ease, reverseEase, transitionOn, autoReverse)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
