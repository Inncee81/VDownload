using System.Collections.Generic;

namespace VDownload.Parsers
{
    class UrlWebpage
    {
        private static Dictionary<string, List<string>> urlIndicators = new()
        {
            { "youtube_single", new(){ "youtube.com/watch?v=", "youtu.be" } },
            { "youtube_playlist", new(){ "youtube.com/playlist?list=" } }
        };
        public static string Get(string url)
        {
            foreach (string w in urlIndicators.Keys)
            {
                foreach (string s in urlIndicators[w])
                {
                    if (url.Contains(s))
                    {
                        return w;
                    }
                }
            }
            return null;
        }

    }
}
