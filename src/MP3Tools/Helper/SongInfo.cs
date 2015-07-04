using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Tools.Helper
{
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
