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

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedItems = (string[])e.Data.GetData(DataFormats.FileDrop);

                DroppedFilesProcessor dfp = new DroppedFilesProcessor();
                IList<string> mp3Files = dfp.Process(droppedItems);


                

                foreach (string mp3File in mp3Files)
                {
                    if (!fileItems.ContainsAlready(mp3File))
                    {
                        FileItem fi = new FileItem(mp3File);
                        fileItems.Add(fi);
                    }
                }



                FileNameAnalyzer fna = new FileNameAnalyzer();
                fna.AnalyzeAll(mp3Files);
            }
        }

        private void label1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WRKR.Stop();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
