using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace MP3Tools
{
    static class SettingsManager
    {
        private static readonly string fileName = "settings.json";

        private static Settings defaultSettings;

        static SettingsManager()
        {
            defaultSettings = new Settings();
            defaultSettings.SeparatorsToFind = new char[] { ' ', '_' };
            defaultSettings.PatternsToFind = new string[] { "www.",
                                                            "WWW.",
                                                            "Www.",
                                                            ".info",
                                                            ".INFO",
                                                            ".Info",
                                                            ".ro",
                                                            ".RO",
                                                            ".Ro",
                                                            ".pl",
                                                            ".PL",
                                                            ".Pl",
                                                            ".com",
                                                            ".COM",
                                                            ".Com",
                                                            ".net",
                                                            ".NET",
                                                            ".Net",
                                                            ".ru",
                                                            ".RU",
                                                            ".Ru",
                                                            ".hu",
                                                            ".HU",
                                                            ".Hu",
                                                            ".org",
                                                            ".ORG",
                                                            ".Org",
	                                                        ".eu",
                                                            ".EU",
                                                            ".Eu",
	                                                        ".in",
                                                            ".IN",
                                                            ".In"
                                                };
            defaultSettings.SetID3Tags = true;
            defaultSettings.NewSeparator = " ";
        }


        public static Settings LoadSettingsFromFile()
        {
            Settings sett = null;

            try
            {

                using (StreamReader sr = new StreamReader(fileName))
                {
                    string json = sr.ReadToEnd();

                    sett = JsonConvert.DeserializeObject<Settings>(json);
                }

            }
            catch (Exception)
            {
                sett = defaultSettings;
                SaveSettingsToFile(defaultSettings);
            }

            return sett;
        }

        public static void SaveSettingsToFile(Settings sett)
        {
            try
            {
                string json = JsonConvert.SerializeObject(sett, Formatting.Indented);

                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(json);
                }
            }
            catch (Exception) {};
        }
    }
}
