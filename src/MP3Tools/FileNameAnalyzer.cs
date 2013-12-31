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
            string newFileName = GetNewFileName(fileName);


            SongInfo songInfo = GetArtistTitleFromFileName(newFileName);
            newFileName = String.Format("{0}{1}{2}{1}{3}", songInfo.Artist, 
                                            settings.NewSeparator, 
                                            Constants.ARTIST_TITLE_SEPARATOR,
                                            songInfo.Title);

            Console.WriteLine(newFileName);

            return newFileName;
        }


        //public static void Process(FileItem fi)
        //{
        //    if (fi.Processed == ProcessState.NotYet)
        //    {
        //        string newFileName = GetNewFileName(fi.FullPath);

        //        fi.NewName = newFileName;


        //        string newFullPath = RenameFile(fi.FullPath, newFileName + ".mp3");
                

        //        string artist, title;

        //        GetArtistTitleFromFileName(newFileName, out artist, out title);


        //        if (currentSettings.SetID3Tags)
        //        {
        //            SetArtistAndTitleTags(newFullPath, artist, title);
        //        }

                

        //        fi.Processed = ProcessState.Done;
        //    }
        //}

        private string GetNewFileName(string originalName)
        {
            string file = Path.GetFileNameWithoutExtension(originalName);

            string[] elements = file.Split(settings.SeparatorsToFind, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder(file.Length);

            string current;
            for (int i = 0; i < elements.Length; i++)
            {
                current = elements[i];

                bool contains = false;
                foreach (string minta in settings.PatternsToFind)
                {
                    if (current.Contains(minta))
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
            string mappa = Path.GetDirectoryName(originalFullPath);

            string newFullPath = Path.Combine(mappa, newFileName);

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
