using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP3Tools
{
    [Serializable]
    public class Settings
    {
        public char[] SeparatorsToFind { get; set; }
        public string[] PatternsToFind { get; set; }

        public bool SetID3Tags { get; set; }

        public string NewSeparator { get; set; }
    }
}
