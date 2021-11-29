using Activity_Tracker_WPF.Models;
using Activity_Tracker_WPF.Resources;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Activity_Tracker_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public DelegateCommand InsertDayCommand { get; }
        public DelegateCommand InsertTaskCommand { get; }

        private string data;
        //private static string newDayTemplate = "[29-11-2021]";

        public MainWindow()
        {
            InitializeComponent();

            InsertDayCommand = new DelegateCommand(OnInsertDay, CanExecuteCommand);
            InsertTaskCommand = new DelegateCommand(OnInsertTask, CanExecuteCommand);

            LoadFile(Settings.Default.LastPath);

            DataContext = this;
        }

        private void OnInsertDay(object commandParameter)
        {
            AddText(GetDate(), 3);
        }

        private void OnInsertTask(object commandParameter)
        {
            AddText(GetTask());
        }

        private bool CanExecuteCommand(object commandParameter)
        {
            return true;
        }

        private void AddText(string text, int newLines = 1)
        {
            for (int i = 0; i < newLines; i++)
            {
                dataTextBox.AppendText(Environment.NewLine);
            }
            
            dataTextBox.AppendText(text);
            FocusTextBox(dataTextBox);
        }

        private void FocusTextBox(TextBox textBox)
        {
            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
            textBox.Focus();
        }

        private string GetDate()
        {
            return DateTime.Today.ToString("[dd-MM-yyyy]");
        }

        private string GetTask()
        {
            //- [11:00-11:30] worked on Activity Tracker WPF
            return DateTime.Now.ToString("- [HH:mm-HH:mm] ");

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
