using System;
using System.Collections.Generic;
using LightConfig;

namespace VDownload
{
    static class Config
    {
        // Main configuration file
        private static string MainPath = String.Format(@"{0}\config.cfg", Global.Paths.APPDATA);
        private static Dictionary<string, string> MainContent = new Dictionary<string, string>()
        {
            {"filename", "%title%"},
            {"output_path", Global.Paths.OUTPUT},
            {"video_ext", "mp4"},
            {"audio_ext", "mp3"},
            {"date_format", "yyyy.MM.dd"},
            {"ffmpeg_path", Global.Paths.FFMPEG},
        };
        public static ConfigObject Main = new ConfigObject(MainPath, MainContent);
    }
}
