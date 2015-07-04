using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace MP3Tools.Helper
{
    public static class FileHelper
    {
        public static IList<string> GetAllFilesFrom(IList<string> paths)
        {
            List<string> allFiles = new List<string>();

            foreach (string f in paths)
            {
                if (IsDirectory(f))
                {
                    allFiles.AddRange(FileHelper.GetAllFilesFromFolder(f));
                }
                else
                {
                    allFiles.Add(f);
                }
            }

            return allFiles;
        }

        public static bool IsDirectory(string path)
        {
            return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
        }

        public static IList<string> GetAllFilesFromFolder(string dirName)
        {
            List<string> files = new List<string>();

            string[] folders = Directory.GetDirectories(dirName);
            foreach (string folder in folders)
            {
                files.AddRange(GetAllFilesFromFolder(folder));
            }

            files.AddRange(Directory.GetFiles(dirName));

            return files;
        }

        public static bool HasExtension(string fileName, string extension) {
            return String.Compare(System.IO.Path.GetExtension(fileName), extension, true) == 0;
        }

    }
}
