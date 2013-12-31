using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MP3Tools.Helper;

namespace MP3Tools
{
    public class DroppedFilesProcessor
    {
        public static readonly string MP3_EXTENSION = ".mp3";


        public List<string> Process(IList<string> droppedFiles)
        {
            // Select all dropped files.
            List<string> allFiles = new List<string>();

            foreach (string f in droppedFiles)
            {
                if (FileHelper.IsDirectory(f))
                {
                    // If the dropped item is a directory, get all files from it recursively.

                    IList<string> innerFiles = FileHelper.GetAllFilesFromFolder(f);
                    allFiles.AddRange(innerFiles);
                }
                else
                {
                    allFiles.Add(f);
                }
            }

            // We only want to work with MP3 files, other fiels will be ignored.
            List<string> mp3Files = FileHelper.SelectFilesWithExtension(allFiles, MP3_EXTENSION);

            return mp3Files;
        }


    }
}
