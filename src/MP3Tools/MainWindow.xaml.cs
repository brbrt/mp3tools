using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Collections;
using System.Threading.Tasks;

namespace MP3Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileItemList fileItems;

        private Settings settings;
        private DroppedFilesProcessor dropppedFilesProcessor;
        private FileModifier fileModifier;

        public MainWindow()
        {
            InitializeComponent();

            fileItems = new FileItemList();
            listView1.DataContext = fileItems;

            settings = SettingsManager.LoadSettingsFromFile();
            dropppedFilesProcessor = new DroppedFilesProcessor(settings);
            fileModifier = new FileModifier(settings);
        }

        private async void Window_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] droppedItems = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Process the dropped files in the background.
            Task<IList<FileItem>> task = Task.Run<IList<FileItem>>(() => dropppedFilesProcessor.Process(droppedItems));
            IList<FileItem> analyzedFiles = await task;

            fileItems.AddNewItems(analyzedFiles);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            IList<FileItem> filesToModify = fileItems.Where(f => f.Processed == ProcessStatus.Ready).ToList();

            Task task = Task.Run(() => fileModifier.ModifyAll(filesToModify));
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            fileItems
                .Where(fi => fi.Processed == ProcessStatus.Ready)
                .ToList()
                .ForEach(fi => fi.Processed = ProcessStatus.Cancelled);
        }

    }
}
