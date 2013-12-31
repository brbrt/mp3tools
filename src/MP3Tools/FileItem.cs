using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MP3Tools
{
    public enum ProcessState { Error, Cancelled, NotYet, Processing, Done};


    public class FileItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string fullPath;
        private ProcessState processed;
        private string newName;


        public FileItem() {}

        public FileItem(string fileName)
        {
            FullPath = fileName;
            Processed = ProcessState.NotYet;
            NewName = String.Empty;
        }


        public string FullPath
        {
            get
            {
                return fullPath;
            }
            set
            {
                fullPath = value;
                OnPropertyChanged("FullPath");
            }
        }

        public ProcessState Processed
        {
            get
            {
                return processed;
            }
            set
            {
                processed = value;
                OnPropertyChanged("Processed");
            }
        }

        public string NewName
        {
            get
            {
                return newName;
            }
            set
            {
                newName = value;
                OnPropertyChanged("NewName");
            }
        }


        public string FileName
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(fullPath); }
        }

        public string Path
        {
            get { return System.IO.Path.GetDirectoryName(fullPath); }
        }

        public bool IsNameChanged
        {
            get { return NewName != FileName; }
        }


        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
