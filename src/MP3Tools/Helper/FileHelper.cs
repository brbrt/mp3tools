using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace MP3Tools.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// Decides that the given path is a path of a directory or it's a file.
        /// Returns true if the given file path is a folder.
        /// </summary>
        /// <param name="Path">The path to check.</param>
        /// <returns>True if it's a directory, false if it's not.</returns>
        public static bool IsDirectory(string path)
        {
            return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
        }

        public static List<string> SelectFilesWithExtension(List<string> files, string extension)
        {
            #region Check arguments.

            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentException("extension");
            }

            #endregion


            List<string> selectedFiles = new List<string>(files.Count);

            foreach (string file in files)
            {
                if (String.Compare(System.IO.Path.GetExtension(file), extension, true) == 0)
                {
                    selectedFiles.Add(file);
                }
            }

            return selectedFiles;
        }

        /// <summary>
        /// Gets all files from the given directory, recursively (including the files of the subdirectories).
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static List<string> GetAllFilesFromFolder(string dirName)
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
    }
}
