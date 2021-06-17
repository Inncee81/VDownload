using System.Collections.Generic;

namespace VDownload.Parsers
{
    class Filename
    {
        private static List<char> forbiddenChars = new() { '/', ':', '*', '?', '"', '<', '>', '|', '"' };
        public static string Get(string filename, Dictionary<string, string> data)
        {
            foreach (KeyValuePair<string, string> e in data)
            {
                filename = filename.Replace(e.Key, e.Value);
            }
            foreach (char c in forbiddenChars)
            {
                filename = filename.Replace(c.ToString(), "");
            }
            return filename;
        }
    }
}
