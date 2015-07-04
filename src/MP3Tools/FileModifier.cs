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


        public FileModifier(Settings settings) 
        {
            this.settings = settings;
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
            fileItem.Processed = ProcessStatus.Processing;

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

                fileItem.Processed = ProcessStatus.Done;
                Log(fileItem);
            }
            catch (Exception ex)
            {
                fileItem.Processed = ProcessStatus.Error;
                Log(fileItem, ex);
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


        private void Log(FileItem file, Exception ex = null)
        {
            StringBuilder logMessage = new StringBuilder();

            logMessage.Append("Path: " + file.Path + "\n");
            logMessage.Append("Original name: " + file.FileName + "\n");
            logMessage.Append("New name     : " + file.NewName + "\n");
            logMessage.Append("Status: " + file.Processed + "\n");
            logMessage.Append("Date: " + DateTime.Now.ToString() + "\n");

            if (ex != null)
            {
                logMessage.Append("Exception: " + ex.Message + "\n");
                logMessage.Append("StackTrace: " + ex.StackTrace + "\n");
            }

            logMessage.Append("------------------------------------------------------------\n");


            WriteLog(logMessage.ToString());
        }

        private void WriteLog(string message)
        {
            using (StreamWriter logger = File.AppendText(LOG_FILE))
            {
                logger.Write(message);
            }
        }
    }
}
