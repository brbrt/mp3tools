using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MP3Tools
{
    public class StringHelper
    {
        private static bool[] _lookup;

        static StringHelper()
        {
            _lookup = new bool[65535];

            for (char c = '0'; c <= '9'; c++)
            {
                _lookup[c] = true;
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                _lookup[c] = true;
            }

            for (char c = 'a'; c <= 'z'; c++)
            {
                _lookup[c] = true;
            }


            // TODO: make it configurable.
            _lookup[' '] = true;
            _lookup['_'] = true;
            _lookup['-'] = true;
            _lookup['('] = true;
            _lookup[')'] = true;
            _lookup['&'] = true;
            _lookup['.'] = true;
            _lookup[','] = true;
        }


        /// <summary>
        /// Removes all special letters from the given string.
        /// </summary>
        public static string RemoveSpecialCharacters(string original)
        {
            StringBuilder sb = new StringBuilder(original.Length);

            foreach (char c in original)
            {
                if (_lookup[c])
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }



        /// <summary>
        /// Replaces all accents in the given string (eg. éáű > eau)
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <returns>The string without accents.</returns>
        public static string ReplaceAccents(string original)
        {
            StringBuilder sb = new StringBuilder(original.Length);

            String normalizedString = original.Normalize(NormalizationForm.FormD);

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
