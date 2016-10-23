using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace SciChart.Wpf.UI.Controls.Markup
{
    /// <summary>
    /// A MarkupExtension which introduces x:Type like syntax to both WPF and Silverlight (Cross-platform). This is used internally
    /// for the themes, but is also useful e.g. when creating custom Control Templates for SciChart
    /// </summary>
    /// <remarks>
    /// Licensed under the CodeProject Open License
    /// http://www.codeproject.com/Articles/305932/Static-and-Type-markup-extensions-for-Silverlight
    /// </remarks>
    /// 
    public class TypeExtension : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeExtension" /> class.
        /// </summary>
        public TypeExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeExtension" /> class.
        /// </summary>
        /// <param name="type">The type to wrap</param>
        public TypeExtension(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets or sets the type information for this extension.
        /// </summary>
        public System.Type Type { get; set; }

        /// <summary>
        /// Gets or sets the type name represented by this markup extension.
        /// </summary>
        public String TypeName { get; set; }

        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type == null)
            {
                if (String.IsNullOrWhiteSpace(TypeName)) throw new InvalidOperationException("No TypeName or Type specified.");
                if (serviceProvider == null) return DependencyProperty.UnsetValue;

                IXamlTypeResolver resolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
                if (resolver == null) return DependencyProperty.UnsetValue;

                Type = resolver.Resolve(TypeName);
            }
            return Type;
        }
    }
}
