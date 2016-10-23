using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace SciChart.Wpf.UI.Controls
{
    /// <summary>
    /// A hyperlink that actually clicks
    /// </summary>
    /// <remarks>Taken from http://www.neovolve.com/post/2011/06/15/WPF-and-the-link-that-just-wont-click.aspx</remarks>
    public class ClickableLink : Hyperlink
    {
        public static readonly DependencyProperty MailToSubjectProperty = DependencyProperty.Register(
            "MailToSubject", typeof(string), typeof(ClickableLink), new PropertyMetadata(default(string)));

        public string MailToSubject
        {
            get { return (string)GetValue(MailToSubjectProperty); }
            set { SetValue(MailToSubjectProperty, value); }
        }

        public static readonly DependencyProperty MailToBodyProperty = DependencyProperty.Register(
            "MailToBody", typeof(IList<string>), typeof(ClickableLink), new PropertyMetadata(default(IList<string>)));

        public IList<string> MailToBody
        {
            get { return (IList<string>)GetValue(MailToBodyProperty); }
            set { SetValue(MailToBodyProperty, value); }
        }

        protected override void OnClick()
        {
            base.OnClick();

            var navigateUri = ResolveAddressValue(NavigateUri);

            if (navigateUri == null)
            {
                return;
            }

            var address = navigateUri.ToString();

            var startInfo = new ProcessStartInfo(address);

            Process.Start(startInfo);
        }

        private Uri ResolveAddressValue(Uri navigateUri)
        {
            if (navigateUri == null)
            {
                return null;
            }

            // Disallow file urls
            if (navigateUri.IsFile)
            {
                return null;
            }

            if (navigateUri.IsUnc)
            {
                return null;
            }

            String address = navigateUri.ToString();

            if (String.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            if (address.Contains("@"))
            {
                if (address.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) == false)
                    address = "mailto:" + address;

                if (MailToSubject != null)
                    address += "?subject=" + MailToSubject;

                if (MailToBody != null)
                    address += "&body=" + string.Join(". ", MailToBody);

                byte[] bytes = Encoding.Default.GetBytes(address);
                address = Encoding.UTF8.GetString(bytes);
            }
            else if (address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) == false &&
                     address.StartsWith("https://", StringComparison.OrdinalIgnoreCase) == false)
            {
                address = "http://" + address;
            }

            try
            {
                return new Uri(address);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
    }
}
