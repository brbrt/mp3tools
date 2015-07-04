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
             return FileHelper.GetAllFilesFrom(droppedFiles)
                 .Where(f => FileHelper.HasExtension(f, Constants.MP3_EXTENSION))
                 .Select(f => ProcessItem(f))                
                 .ToList();
        }

        private FileItem ProcessItem(string originalFileName)
        {
            FileNameAnalyzer fna = new FileNameAnalyzer(settings);
            string newFileName = fna.SuggestNiceFileName(originalFileName);

            return new FileItem(originalFileName, newFileName);
        }
    }
}
