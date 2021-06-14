using System;
using System.Collections.Generic;
using OutputParser;
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
        static void Help()
        {
            string output = Output.Get(
                file: @"output\main\help.out"
            );
            Console.Write(output);
        }


        // Downloading
        static void Download(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }
            else
            {
                string url = args[1];
                Dictionary<string, string> options = Options.Parse(args[2..]);
                if (Str.IfStringContainsStringsFromListOR(url, Global.LINKIND_YOUTUBEVID))
                {
                    Youtube.VideoDownload(url, options);
                }
                else
                {
                    Console.Write(Output.Get(@"output\main\error_wrong_site.out"));
                }
            }
        }


        // Informations about program
        static void About()
        {
            string authorsSegment = "";
            foreach (KeyValuePair<string, string> entry in Global.PROGRAM_AUTHORS)
            {
                authorsSegment += String.Format("{0} ({1})\n", entry.Key, entry.Value);
            }
            List<string> args = new() {
                Global.PROGRAM_NAME,
                Global.PROGRAM_VERSION,
                Global.PROGRAM_BUILD_ID,
                Global.PROGRAM_REPOSITORY,
                authorsSegment.TrimEnd(),
                Global.PROGRAM_DATE_START,
                Global.PROGRAM_DATE_LAST,
            };
            string output = Output.Get(
                file: @"output\main\about.out",
                args: args
            );
            Console.Write(output);
        }


        // Informations about specified video (or playlist)
        static void Info(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }
            else
            {
                string url = args[1];
                if (Str.IfStringContainsStringsFromListOR(url, Global.LINKIND_YOUTUBEVID))
                {
                    Youtube.VideoInfo(url);
                }
                else
                {
                    Console.Write(Output.Get(@"output\main\error_wrong_site.out"));
                }
            }
        }


        // Returns value of specified settings key
        static void SettingsGet(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }
            else
            {
                Console.Write(Settings.Get(args[1]));
            }
        }


        // Sets value of specified settings key
        static void SettingsSet(string[] args)
        {
            if (args.Length < 3)
            {
                Help();
            }
            else
            {
                Console.Write(Settings.Set(args[1], args[2]));
            }
        }


        // Resets (deletes) configuration file
        static void SettingsReset()
        {
            Console.Write(Settings.Reset());
        }
    }
}
