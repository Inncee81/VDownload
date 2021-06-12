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
            if (Config.Main.R_all().TryGetValue(key, out value))
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
            else if (!(Config.Main.R_all().ContainsKey(key)))
            {
                output = Output.Get(@"output\settings\error_key_does_not_exists.out");
            }
            else
            {
                string oldValue = Config.Main.R_key(key);
                Config.Main.W(key, value);
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
    }
}
