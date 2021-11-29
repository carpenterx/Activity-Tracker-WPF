using Activity_Tracker_WPF.Resources;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;

namespace Activity_Tracker_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string data;

        public MainWindow()
        {
            InitializeComponent();

            LoadFile(Settings.Default.LastPath);
        }

        private void LoadFile(string path)
        {
            if (File.Exists(path))
            {
                UpdateLastPath(path);
                UpdateFileName(path);

                data = File.ReadAllText(path);
                dataTextBox.ScrollToHome();
                dataTextBox.Text = data;
            }
        }

        private void UpdateLastPath(string newPath)
        {
            if (Settings.Default.LastPath != newPath)
            {
                Settings.Default.LastPath = newPath;
                Settings.Default.Save();
            }
        }

        private void UpdateFileName(string newPath)
        {
            fileNameLabel.Content = Path.GetFileNameWithoutExtension(newPath);
        }

        private void BrowseToFile(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Title = "Open Activity File";
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Text Documents (.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        private void ClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            File.WriteAllText(Settings.Default.LastPath, dataTextBox.Text);
        }
    }
}
