using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Database.Extensions
{
    public static class UtilityExtension
    {
       public static string ReplaceParametersFromDictionary(this string startString, IDictionary<string, string> parameters)
        {
            var upperCaseParameterDictionary = (from x in parameters
                                                select x).ToDictionary(x => x.Key.ToUpperInvariant(), x => x.Value);

            var matchString = new Regex(@"@\w*?@");

            var m = matchString.Match(startString);
            while (m.Success)
            {
                Group g = m.Groups[0];
                var keySurroundedByAt = g.Captures[0].Value;
                var keyFound = keySurroundedByAt.Replace("@", string.Empty).ToUpperInvariant();
                if (upperCaseParameterDictionary.ContainsKey(keyFound))
                {
                    startString = startString.Replace(keySurroundedByAt, upperCaseParameterDictionary[keyFound]);
                }

                m = m.NextMatch();
            }
            return startString;
        }
        public static ArrayList ReadSqlFile(string file, string regexType = "Scenario")
        {
            var regExTc = @"--@TC.*?\r\n";
            var regExParam = @"--@.*?\r\n";

            var regExString = (regexType == "Scenario") ? regExTc : regExParam;

            var str = File.ReadAllText(file);

            // array of sqls.
            var sqls = Regex.Split(str, regExString).Where(s => s != string.Empty).ToArray();
            var regex = new Regex(regExString);

            // array of test headers.
            var testHeaders = regex.Matches(str).Cast<Match>().Select(m => m.Value).ToArray();


            if (sqls.Count() != testHeaders.Count())
                return null;

            // Return the two list.           
            var returnArray = new ArrayList {testHeaders, sqls};
            return returnArray;
        }

        public static bool CompareArrayList(ArrayList left, ArrayList right)
        {
            if (left == null && right == null) return true;

            if (left == null || right == null) return false;

            if (left.Count != right.Count) return false; Trace.WriteLine($"Source Count: {left.Count} Target Count: {right.Count}");

            return !left.Cast<object>().Where((t, i) => !t.Equals(right[i])).Any();
        }
    }
}
