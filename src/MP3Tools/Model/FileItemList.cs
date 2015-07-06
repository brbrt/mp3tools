using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MP3Tools.Model
{
    class FileItemList : ObservableCollection<FileItem>
    {

        public void AddNewItems(IEnumerable<FileItem> collection)
        {
            lock (this)
            {
                foreach (var item in collection)
                {
                    if (!Items.Contains(item))
                    {
                        Items.Add(item);
                    }
                }
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
}
