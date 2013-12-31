using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MP3Tools.Helper;

namespace MP3Tools
{
    public class FileModifier
    {
        public static readonly string LOG_FILE = "log.txt";

        private readonly Settings settings;


        public FileModifier() 
        {
            this.settings = SettingsManager.LoadSettingsFromFile();
        }


        public void ModifyAll(IList<FileItem> fileItems)
        {
            foreach (FileItem fileItem in fileItems)
            {
                Modify(fileItem);
            }
        }

        public void Modify(FileItem fileItem)
        {
            fileItem.Processed = ProcessState.Processing;

            try
            {
                string fullPath = fileItem.FullPath;

                if (fileItem.IsNameChanged)
                {
                    fullPath = RenameFile(fileItem);
                }


                if (settings.SetID3Tags)
                {
                    SongInfo songInfo = SongHelper.GetArtistTitleFromFileName(fileItem.NewName);
                    SetArtistAndTitleTags(fullPath, songInfo);
                }

                fileItem.Processed = ProcessState.Done;
            }
            catch (Exception ex)
            {
                fileItem.Processed = ProcessState.Error;
            }
        }

        private static string RenameFile(FileItem fileItem)
        {
            string newName = fileItem.NewName + Constants.MP3_EXTENSION;
            string newFullPath = Path.Combine(fileItem.Path, newName);

            File.Move(fileItem.FullPath, newFullPath);

            return newFullPath;
        }

        private static void SetArtistAndTitleTags(string fileName, SongInfo songInfo)
        {
            TagLib.File f = TagLib.File.Create(fileName);

            f.Tag.Clear();

            f.Tag.Performers = new string[] { songInfo.Artist };
            f.Tag.Title = songInfo.Title;

            f.Save();
        }
    }
}
