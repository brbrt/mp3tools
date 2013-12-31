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


            SongInfo songInfo = GetArtistTitleFromFileName(newFileName);
            newFileName = String.Format("{0}{1}{2}{1}{3}", songInfo.Artist, 
                                            settings.NewSeparator, 
                                            Constants.ARTIST_TITLE_SEPARATOR,
                                            songInfo.Title);

            Console.WriteLine(newFileName);

            return newFileName;
        }

        private string SuggestNiceFileName(string originalName)
        {
            string file = Path.GetFileNameWithoutExtension(originalName);

            string[] elements = file.Split(settings.SeparatorsToFind, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder(file.Length);

            string current;
            for (int i = 0; i < elements.Length; i++)
            {
                current = elements[i];

                bool contains = false;
                foreach (string pattern in settings.PatternsToFind)
                {
                    if (current.Contains(pattern))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    if (i != 0)
                    {
                        sb.Append(settings.NewSeparator);
                    }

                    sb.Append(current);
                }
            }

            string withoutAccents = StringHelper.ReplaceAccents(sb.ToString());
            string withoutSpecialCharacters = StringHelper.RemoveSpecialCharacters(withoutAccents);

            return withoutSpecialCharacters;
        }

        private string RenameFile(string originalFullPath, string newFileName)
        {
            string directory = Path.GetDirectoryName(originalFullPath);

            string newFullPath = Path.Combine(directory, newFileName);

            File.Move(originalFullPath, newFullPath);

            return newFullPath;
        }

        public static SongInfo GetArtistTitleFromFileName(string fileName)
        {
            string artist = null;
            string title = null;

            char[] separator = { Constants.ARTIST_TITLE_SEPARATOR };
            string[] elements = fileName.Split(separator, 2);

            artist = elements[0].Trim();

            if (elements.Length > 1)
            {
                title = elements[1].Trim();
            }
            

            SongInfo songInfo = new SongInfo(artist, title);
            return songInfo;
        }

        private static void SetArtistAndTitleTags(string fileName, string artist, string title)
        {
            TagLib.File f = TagLib.File.Create(fileName);

            f.Tag.Clear();

            f.Tag.Performers = new string[] { artist };
            f.Tag.Title = title;

            f.Save();
        }
    }
}
