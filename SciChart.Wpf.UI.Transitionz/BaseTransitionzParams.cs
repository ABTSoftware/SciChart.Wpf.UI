#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013 SciChart Ltd & Transitionz Contributors
// --------------------------------------------------------------------
// Microsoft Public License (Ms-PL) http://opensource.org/licenses/MS-PL
// 
// This license governs use of the accompanying software. If you use the software, you
// accept this license. If you do not accept the license, do not use the software.
// 
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
// same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, 
// prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, 
// sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from 
// such contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present 
// in the software.
// (D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of 
// this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a 
// license that complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. 
// You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws,
// the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
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