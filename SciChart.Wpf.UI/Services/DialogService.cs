using System;
using System.Windows;
using Microsoft.Win32;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Wpf.UI.Services
{
    public interface IDialogService
    {
        bool? ShowOpenFileDialog(string filter, out string filename);

        bool? ShowSaveFileDialog(string filter, out string filename);

        Window MainWindow { get; set; }
    }

    [ExportType(typeof(IDialogService), CreateAs.Singleton)]
    public class DialogService : IDialogService
    {
        public Window MainWindow { get; set; }

        public bool? ShowOpenFileDialog(string filter, out string filename)
        {
            var openFileDialog = new OpenFileDialog
                {
                    Filter = filter,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

            bool? result = openFileDialog.ShowDialog();
            filename = openFileDialog.FileName;

            return result;
        }

        public bool? ShowSaveFileDialog(string filter, out string filename)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            bool? result = saveFileDialog.ShowDialog();
            filename = saveFileDialog.FileName;

            return result;
        }
    }
}
