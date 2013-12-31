using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MP3Tools
{
    class ProcessDroppedFiles
    {
        public static List<string> GetAllMP3(string[] droppedFiles)
        {
            List<string> allFiles = GetAllFiles(droppedFiles);

            List<string> mp3s = OnlyMP3Files(allFiles);


            return mp3s;
        }


        private static List<string> OnlyMP3Files(List<string> files)
        {
            List<string> mp3s = new List<string>(files.Count);

            foreach (string file in files)
            {
                if (String.Compare(System.IO.Path.GetExtension(file), ".mp3", true) == 0)
                    mp3s.Add(file);
            }

            return mp3s;
        }

        private static List<string> GetAllFiles(string[] items)
        {
            List<string> allFiles = new List<string>(items.Length + 5);

            foreach (string item in items)
            {
                if (IsFolder(item))
                {
                    allFiles.AddRange(GetFilesFromFolder(item));
                }
                else
                {
                    allFiles.Add(item);
                }
            }

            return allFiles;
        }

        private static List<string> GetFilesFromFolder(string folderName)
        {
            List<string> files = new List<string>();

            string[] folders = Directory.GetDirectories(folderName);

            foreach (string folder in folders)
            {
                files.AddRange(GetFilesFromFolder(folder));
            }


            files.AddRange(Directory.GetFiles(folderName));

            return files;
        }

        /// <summary>
        /// Returns true if the given file path is a folder.
        /// </summary>
        /// <param name="Path">File path</param>
        /// <returns>True if a folder</returns>
        private static bool IsFolder(string path)
        {
            return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
        }
    }
}
