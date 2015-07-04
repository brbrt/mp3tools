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
        private readonly Settings settings;

        public DroppedFilesProcessor(Settings settings)
        {
            this.settings = settings;
        }

        public IList<FileItem> Process(IList<string> droppedFiles)
        {
            IList<string> allFiles = FileHelper.GetAllFilesFrom(droppedFiles);

            IList<string> mp3Files = FileHelper.FilterFilesByExtension(allFiles, Constants.MP3_EXTENSION);


            // Analyze all MP3-s and suggest a nice new filename.
            FileNameAnalyzer fna = new FileNameAnalyzer(settings);

            IList<FileItem> result = new List<FileItem>();

            foreach (string file in mp3Files)
            {
                string newName = fna.Analyze(file);

                FileItem fileItem = new FileItem(file, newName);
                result.Add(fileItem);
            }

            return result;
        }


    }
}
