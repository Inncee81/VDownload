using OutputParser;
using System.Collections.Generic;

namespace VDownload
{
    class Settings
    {
        // Returns value of specified settings key
        public static string Get(string key)
        {
            string output;
            string value;
            if (Config.Main.ReadAll().TryGetValue(key, out value))
            {
                List<string> args = new() {
                    key,
                    value
                };
                output = Output.Get(
                    file: @"output\settings\get.out",
                    args: args
                );
            }
            else
            {
                output = Output.Get(@"output\settings\error_key_does_not_exists.out");
            }
            return output;
        }

        // Sets value of specified settings key
        public static string Set(string key, string value)
        {
            string output;
            if (key.Trim() == "")
            {
                output = Output.Get(@"output\settings\error_key_is_an_empty_string.out");
            }
            else if (value.Trim() == "")
            {
                output = Output.Get(@"output\settings\error_value_is_an_empty_string.out");
            }
            else if (!(Config.Main.ReadAll().ContainsKey(key)))
            {
                output = Output.Get(@"output\settings\error_key_does_not_exists.out");
            }
            else
            {
                string oldValue = Config.Main.ReadKey(key);
                Config.Main.Write(key, value);
                List<string> args = new() {
                    key,
                    oldValue,
                    value
                };
                output = Output.Get(
                    file: @"output\settings\set.out",
                    args: args
                );
            }
            return output;
        }

        //
        public static string Reset()
        {
            string output = Output.Get(@"output\settings\reset.out");
            try
            {
                Config.Main.ResetFile();
            }
            catch
            {
                output = Output.Get(@"output\settings\error_default_settings_cannot_be_restored.out");
            }
            return output;
        }
    }
}
