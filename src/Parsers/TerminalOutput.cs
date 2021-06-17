using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VDownload.Parsers
{
    class TerminalOutput
    {
        private static string replaceChar = "{#}";
        private static string outputFilesLocation = Global.Paths.MAIN;
        public static string Get(string file, bool upSP = true, bool downSP = true, List<string> args = null)
        {
            string text = File.ReadAllText(outputFilesLocation + @"\" + file);
            while (text.Contains(replaceChar) && args != null && args.Count > 0)
            {
                Regex r = new Regex(replaceChar, RegexOptions.IgnoreCase);
                text = r.Replace(text, args[0], 1);
                args.RemoveAt(0);
            }
            if (upSP) { text = "\n" + text; }
            if (downSP) { text += "\n"; }
            return text;
        }
    }
}
