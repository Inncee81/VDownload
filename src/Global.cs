using System;
using System.IO;
using System.Collections.Generic;
using ConsoleTableExt;

namespace VDownload
{
    static class Global
    {
        // Informations about program
        public static string PROGRAM_NAME = "VDownload";
        public static string PROGRAM_VERSION = "0.0.1-dev4";
        public static string PROGRAM_BUILD = "21164";
        public static string PROGRAM_REPOSITORY = "https://github.com/mateuszskoczek/VDownload";
        public static Dictionary<string, string> PROGRAM_AUTHORS = new Dictionary<string, string>() {
            {"Mateusz Skoczek", "https://github.com/mateuszskoczek"},
        };
        public static string PROGRAM_DATE_START = "February 2021";
        public static string PROGRAM_DATE_LAST = "June 2021";

        // Important paths
        public static string PATH_CONFIG = String.Format(
            @"{0}\{1}\config.cfg",
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Global.PROGRAM_NAME
        );
        public static string PATH_OUTPUT = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string PATH_TEMP = String.Format(
            @"{0}\{1}\",
            Path.GetTempPath(),
            "VDownload"
        );

        // Links indicators
        public static List<string> LINKIND_YOUTUBEVID = new()
        {
            "www.youtube.com/watch?v=",
            "youtu.be"
        };

        // Config data
        public static Dictionary<string, string> CFGDATA = new Dictionary<string, string>()
        {
            {"filename", "%title%"},
            {"output_path", Global.PATH_OUTPUT},
            {"video_ext", "mp4"},
            {"audio_ext", "mp3"}
        };

        // Table appearance
        public static Dictionary<HeaderCharMapPositions, char> TABLEAPPEARANCE = new Dictionary<HeaderCharMapPositions, char>() {
            { HeaderCharMapPositions.TopLeft, '╒' },
            { HeaderCharMapPositions.TopCenter, '╤' },
            { HeaderCharMapPositions.TopRight, '╕' },
            { HeaderCharMapPositions.BottomLeft, '╞' },
            { HeaderCharMapPositions.BottomCenter, '╪' },
            { HeaderCharMapPositions.BottomRight, '╡' },
            { HeaderCharMapPositions.BorderTop, '═' },
            { HeaderCharMapPositions.BorderRight, '│' },
            { HeaderCharMapPositions.BorderBottom, '═' },
            { HeaderCharMapPositions.BorderLeft, '│' },
            { HeaderCharMapPositions.Divider, '│' },
        };
    }
}
