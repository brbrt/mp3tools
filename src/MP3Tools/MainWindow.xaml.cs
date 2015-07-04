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


        public MainWindow()
        {
            InitializeComponent();

            fileItems = new FileItemList();

            listView1.DataContext = fileItems;
        }

        private async void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedItems = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Process the dropped files in the background.
                Task<IList<FileItem>> task = Task.Run<IList<FileItem>>(() => RunAnalyzeDroppedFiles(droppedItems));
                IList<FileItem> analyzedFiles = await task;

                fileItems.AddNewItems(analyzedFiles);
            }
        }

        private IList<FileItem> RunAnalyzeDroppedFiles(IList<string> droppedFiles)
        {
            DroppedFilesProcessor dfp = new DroppedFilesProcessor();
            return dfp.Process(droppedFiles);
        }

        private void label1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            IList<FileItem> filesToModify = fileItems.Where(f => f.Processed == ProcessState.NotYet).ToList();
            
            Task task = Task.Run(() => ModifyFiles(filesToModify));
        }

        private void ModifyFiles(IList<FileItem> files)
        {
            FileModifier fm = new FileModifier();
            fm.ModifyAll(files);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in listView1.SelectedItems)
            {
                FileItem fi = item as FileItem;

                if (fi.Processed == ProcessState.NotYet)
                {
                    fi.Processed = ProcessState.Cancelled;
                }
            }
        }

    }
}
