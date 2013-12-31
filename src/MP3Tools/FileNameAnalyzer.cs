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


        public void AnalyzeAll(IList<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                Analyze(fileName);
            }
        }

        public void Analyze(string fileName)
        {
            string newFileName = GetNewFileName(fileName);

            Console.WriteLine(newFileName);
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

            string[] elements = file.Split(settings.SeparatorsToFind);

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
                        sb.Append(settings.NewSeparator);

                    sb.Append(current);
                }
            }

            string ekezetekNelkul = StringHelper.ReplaceAccents(sb.ToString());
            string specialisKarakterekNelkul = StringHelper.RemoveSpecialCharacters(ekezetekNelkul);

            return specialisKarakterekNelkul;
        }

        private string RenameFile(string originalFullPath, string newFileName)
        {
            string mappa = Path.GetDirectoryName(originalFullPath);

            string newFullPath = Path.Combine(mappa, newFileName);

            File.Move(originalFullPath, newFullPath);

            return newFullPath;
        }

        public static void GetArtistTitleFromFileName(string fileName, out string artist, out string title)
        {
            artist = String.Empty;
            title = String.Empty;

            string[] elements = fileName.Split(new char[] { '-' });
            
            artist = elements[0];

            for (int i = 1; i < elements.Length; i++)
            {
                title += elements[1];
            }
            
            artist = artist.Trim();
            title = title.Trim();
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
