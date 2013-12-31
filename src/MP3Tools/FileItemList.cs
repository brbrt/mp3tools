using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MP3Tools
{
    class FileItemList : ObservableCollection<FileItem>
    {
        public bool ContainsAlready(string fileName)
        {
            bool contains = false;

            lock (this)
            {
                foreach (FileItem fi in this)
                {
                    if (fi.FullPath == fileName)
                    {
                        contains = true;
                        break;
                    }
                }
            }

            return contains;
        }
    }
}
