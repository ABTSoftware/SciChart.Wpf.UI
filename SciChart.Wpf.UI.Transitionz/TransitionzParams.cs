#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System;
using System.Globalization;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    [Flags]
    public enum TransitionOn
    {
        Once = 0x1,
        Loaded = 0x2,
        DataContextChanged = 0x4,
        Visibility = 0x8
    }

    public interface ITransitionParams<T>
    {
        double BeginTime { get; set; }
        double Duration { get; set; }
        T From { get; set; }
        T To { get; set; }
        EasingFunctionBase Ease { get; set; }
        EasingFunctionBase ReverseEase { get; set; }
        FillBehavior FillBehavior { get; set; }
        TransitionOn TransitionOn { get; set; }
        bool AutoReverse { get; set; }
        RepeatBehavior RepeatBehavior { get; set; }
    }

    public class TransitionzParams<T>
    {
        public TransitionzParams()
        {
            this.Duration = 300;
            this.FillBehavior = FillBehavior.HoldEnd;
            this.TransitionOn = TransitionOn.Once;
        }

        public TransitionzParams(double beginTime, double duration, T from, T to, TransitionOn transitionOn, bool autoReverse, RepeatBehavior repeatBehavior)
            : this()
        {
            this.BeginTime = beginTime;
            this.Duration = duration;
            this.From = from;
            this.To = to;
            this.TransitionOn = transitionOn;
            this.AutoReverse = autoReverse;
            this.RepeatBehavior = repeatBehavior;
        }

        public EasingFunctionBase Ease { get; set; }
        public EasingFunctionBase ReverseEase { get; set; }
        public TransitionOn TransitionOn { get; set; }
        public double BeginTime { get; set; }
        public double Duration { get; set; }
        public T From { get; set; }
        public T To { get; set; }
        public FillBehavior FillBehavior { get; set; }
        public bool AutoReverse { get; set; }
        public RepeatBehavior RepeatBehavior { get; set; }

        protected static double ToDbl(string str)
        {
            return double.Parse(str, CultureInfo.InvariantCulture);
        }
    }
}