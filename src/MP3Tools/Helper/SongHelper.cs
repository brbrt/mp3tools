using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Tools.Helper
{
    public static class SongHelper
    {
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
    }

    public class SongInfo
    {
        public string Artist { get; set; }

        public string Title { get; set; }


        public SongInfo() { }

        public SongInfo(string artist, string title)
        {
            this.Artist = artist;
            this.Title = title;
        }
    }
}
