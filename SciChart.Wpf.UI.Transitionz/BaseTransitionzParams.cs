#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    public abstract class BaseTransitionzExtension<T> : MarkupExtension
    {
        protected BaseTransitionzExtension()
        {
            this.FillBehavior = FillBehavior.HoldEnd;
            this.TransitionOn = TransitionOn.Once;
        }

        protected BaseTransitionzExtension(double beginTime, double duration, T from, T to, EasingFunctionBase ease, EasingFunctionBase reverseEase, TransitionOn transitionOn, bool autoreverse)
            : this()
        {
            this.BeginTime = beginTime;
            this.Duration = duration;
            this.From = from;
            this.To = to;
            this.Ease = ease;
            this.ReverseEase = reverseEase;
            this.TransitionOn = transitionOn;
            this.AutoReverse = autoreverse;
        }

        [ConstructorArgument("BeginTime")]
        public double BeginTime { get; set; }

        [ConstructorArgument("Duration")]
        public double Duration { get; set; }

        [ConstructorArgument("From")]
        public T From { get; set; }

        [ConstructorArgument("To")]
        public T To { get; set; }

        [ConstructorArgument("Ease")]
        public EasingFunctionBase Ease { get; set; }

        [ConstructorArgument("ReverseEase")]
        public EasingFunctionBase ReverseEase { get; set; }

        [ConstructorArgument("TransitionOn")]
        public TransitionOn TransitionOn { get; set; }

        public FillBehavior FillBehavior { get; set; }

        [ConstructorArgument("AutoReverse")]
        public bool AutoReverse { get; set; }
    }
}