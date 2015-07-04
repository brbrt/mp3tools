using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MP3Tools
{
    public class FileItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string fullPath;
        private ProcessStatus processed;
        private string newName;


        public FileItem() {}

        public FileItem(string fileName, string newName)
        {
            FullPath = fileName;            
            NewName = newName;
            Processed = ProcessStatus.Ready;
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

        public ProcessStatus Processed
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

        public override bool Equals(object obj)
        {
            FileItem other = obj as FileItem;
            if (other == null)
            {
                return false;
            }

            return this.fullPath == other.fullPath;
        }

    }
}
