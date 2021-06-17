using System;
using System.Collections.Generic;
using AdditionalMethods;
using ConsoleOptionsParser;

namespace VDownload
{
    class Program
    {
        // Init function
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "about": About(); break;
                    case "info": Info(args); break;
                    case "download": Download(args); break;
                    case "settings-get": SettingsGet(args); break;
                    case "settings-set": SettingsSet(args); break;
                    case "settings-reset": SettingsReset(); break;
                    default: Help(); break;
                }
            }
            else { Help(); }
        }


        // Commands and settings keys list
        private static void Help()
        {
            Console.WriteLine(TerminalOutput.Get(
                file: @"output\main\help.out"
            ));
        }


        // Informations about program
        private static void About()
        {
            string librariesUsedInApp = "";
            foreach (KeyValuePair<string, string> e in Global.ProgramInfo.LIBRARIES)
            {
                librariesUsedInApp += String.Format("- {0} ({1})\n", e.Key, e.Value);
            }
            Console.WriteLine(TerminalOutput.Get(
                file: @"output\main\about.out",
                args: new()
                {
                    Global.ProgramInfo.VERSION,
                    Global.ProgramInfo.BUILD_ID,
                    Global.ProgramInfo.AUTHOR_NAME,
                    Global.ProgramInfo.AUTHOR_GITHUB,
                    Global.ProgramInfo.REPOSITORY,
                    librariesUsedInApp.TrimEnd(),
                    Global.ProgramInfo.DONATION_LINK,
                    Global.ProgramInfo.AUTHOR_NAME,
                    Global.ProgramInfo.PROJECT_START,
                    Global.ProgramInfo.PROJECT_END,
                }
            ));
        }


        // Informations about specified video (or playlist)
        private static void Info(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }
            else
            {
                string url = args[1];
                if (Str.IfStringContainsStringsFromListOR(url, Global.LinkIndicators.YOUTUBE_S))
                {
                    Youtube.VideoInfo(url);
                }
                else
                {
                    Console.WriteLine(TerminalOutput.Get(@"output\main\error_wrong_site.out"));
                }
            }
        }


        // Downloading
        private static void Download(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }
            else
            {
                string url = args[1];
                Dictionary<string, string> options = Options.Parse(args[2..]);
                if (Str.IfStringContainsStringsFromListOR(url, Global.LinkIndicators.YOUTUBE_S))
                {
                    Youtube.VideoDownload(url, options);
                }
                else
                {
                    Console.WriteLine(TerminalOutput.Get(@"output\main\error_wrong_site.out"));
                }
            }
        }


        // Returns value of specified settings key
        private static void SettingsGet(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }
            else
            {
                Console.WriteLine(Settings.Get(args[1]));
            }
        }


        // Sets value of specified settings key
        private static void SettingsSet(string[] args)
        {
            if (args.Length < 3)
            {
                Help();
            }
            else
            {
                Console.WriteLine(Settings.Set(args[1], args[2]));
            }
        }


        // Resets (deletes) configuration file
        private static void SettingsReset()
        {
            Console.WriteLine(Settings.Reset());
        }
    }
}
