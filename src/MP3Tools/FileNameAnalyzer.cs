using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MP3Tools.Helper;

namespace MP3Tools
{
    public class FileNameAnalyzer
    {
        private readonly Settings settings;


        public FileNameAnalyzer()
        {
            this.settings = SettingsManager.LoadSettingsFromFile();
        }


        public List<string> AnalyzeAll(IList<string> fileNames)
        {
            List<string> result = new List<string>();

            foreach (string fileName in fileNames)
            {
                string newFileName = Analyze(fileName);
                result.Add(newFileName);
            }

            return result;
        }

        public string Analyze(string fileName)
        {
            string newFileName = SuggestNiceFileName(fileName);


            SongInfo songInfo = SongHelper.GetArtistTitleFromFileName(newFileName);

            return songInfo.FormatAsFileName(settings.NewSeparator);
        }

        private string SuggestNiceFileName(string originalName)
        {
            string file = Path.GetFileNameWithoutExtension(originalName);

            string[] elements = file.Split(settings.SeparatorsToFind, StringSplitOptions.RemoveEmptyEntries);

            IEnumerable<string> validElements = elements.Where(e => !IsInvalidElement(e));

            string validFileName = String.Join(settings.NewSeparator, validElements);
            string withoutAccents = StringHelper.ReplaceAccents(validFileName);
            string withoutSpecialCharacters = StringHelper.RemoveSpecialCharacters(withoutAccents);

            return withoutSpecialCharacters;
        }

        private bool IsInvalidElement(string fileNameElement)
        {
            foreach (string pattern in settings.PatternsToFind)
            {
                if (fileNameElement.Contains(pattern))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
