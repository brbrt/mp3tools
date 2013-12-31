using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MP3Tools
{
    class ProcessMP3
    {
        private static Settings currentSettings = SettingsManager.LoadSettingsFromFile();


        public static void Process(FileItem fi)
        {
            if (fi.Processed == ProcessState.NotYet)
            {
                string newFileName = GetNewFileName(fi.FullPath);

                fi.NewName = newFileName;


                string newFullPath = RenameFile(fi.FullPath, newFileName + ".mp3");
                

                string artist, title;

                GetArtistTitleFromFileName(newFileName, out artist, out title);


                if (currentSettings.SetID3Tags)
                {
                    SetArtistAndTitleTags(newFullPath, artist, title);
                }

                

                fi.Processed = ProcessState.Done;
            }
        }

        private static string GetNewFileName(string originalName)
        {
            string file = Path.GetFileNameWithoutExtension(originalName);

            string[] elements = file.Split(currentSettings.SeparatorsToFind);

            StringBuilder sb = new StringBuilder(file.Length);

            string current;
            for (int i = 0; i < elements.Length; i++)
            {
                current = elements[i];

                bool contains = false;
                foreach (string minta in currentSettings.PatternsToFind)
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
                        sb.Append(currentSettings.NewSeparator);

                    sb.Append(current);
                }
            }

            string ekezetekNelkul = ManipulateStrings.ReplaceAccents(sb.ToString());
            string specialisKarakterekNelkul = ManipulateStrings.RemoveSpecialCharacters(ekezetekNelkul);

            return specialisKarakterekNelkul;
        }

        private static string RenameFile(string originalFullPath, string newFileName)
        {
            string mappa = Path.GetDirectoryName(originalFullPath);

            string newFullPath = Path.Combine(mappa, newFileName);

            File.Move(originalFullPath, newFullPath);

            return newFullPath;
        }

        private static void GetArtistTitleFromFileName(string fileName, out string artist, out string title)
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

            f.Tag.Artists = new string[] { artist };
            f.Tag.Title = title;

            f.Save();
        }
    }
}
