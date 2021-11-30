using Activity_Tracker_WPF.Models;
using Activity_Tracker_WPF.Resources;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Controls;

namespace Activity_Tracker_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public DelegateCommand InsertDayCommand { get; }
        public DelegateCommand InsertTaskCommand { get; }

        private string? data;

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
            int newLines = 3;
            if (dataTextBox.Text.Length == 0)
            {
                newLines = 0;
            }
            AddText(GetDate(), newLines);
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
            int currentNewLines = 0;
            while (newLines > currentNewLines)
            {
                dataTextBox.AppendText(Environment.NewLine);
                currentNewLines++;
            }
            
            dataTextBox.AppendText(text);
            FocusTextBox(dataTextBox);
        }

        private static void FocusTextBox(TextBox textBox)
        {
            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
            textBox.Focus();
        }

        private static string GetDate()
        {
            return $"[{DateTime.Today:dd-MM-yyyy}]";
        }

        private static string GetTask()
        {
            return $"- [{DateTime.Now:HH:mm-HH:mm}] worked on ";
        }

        private void LoadFile(string path)
        {
            if (File.Exists(path))
            {
                UpdateLastPath(path);
                UpdateFileName(path);

                data = File.ReadAllText(path);
                dataTextBox.Text = data;
                FocusTextBox(dataTextBox);
            }
        }

        private static void UpdateLastPath(string newPath)
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

        private void CreateNewFile(object sender, System.Windows.RoutedEventArgs e)
        {
            string newFileName = GetFileName();
            string newFilePath = Path.Combine(Settings.Default.SaveRoot, newFileName);
            if (!File.Exists(newFilePath))
            {
                using FileStream fs = File.Create(newFilePath);
            }
            LoadFile(newFilePath);
        }

        private static string GetFileName()
        {
            return $"{Settings.Default.FilenameTemplate} [{DateTime.Now:MMMM yyyy}].txt";
        }
    }
}
