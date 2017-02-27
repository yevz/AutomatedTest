using System;
using System.Configuration;
using System.IO;

namespace Database.Extensions
{
    public class Reporter
    {
        public static string LogFileName = "";


        public static bool ReportResults(string testCase, string resultSet, string sqlQuery, string expectedResult = "0")
        {
            testCase = testCase.Replace("--@TC_", "");
            var r = expectedResult == "" || expectedResult == "2" ? "0" : expectedResult;
            var result = resultSet == r ? "Pass" : "Fail";
            var sql = result == "Fail" ? $", SQL Query: {sqlQuery}" : "";
            Console.WriteLine($"Test Case:{testCase.Trim()}, Result:{result} {sql}");
            return result == "Pass";
        }


        public static void LogMessage(string message)
        {
            if (ConfigurationManager.AppSettings["LogToFile"] == "Y" && LogFileName != "")
                LogMessageToFile(message);
            if (ConfigurationManager.AppSettings["LogToConsole"] == "Y")
                LogMessageToConsole(message);

        }

        public static void LogMessage(string message, params object[] args)
        {
            LogMessage(string.Format(message, args));
        }

        private static void LogMessageToFile(string message)
        {
            try
            {
                using (var w = File.AppendText(LogFileName))
                {
                    w.WriteLine($"{DateTime.Now} {message}");
                }
            }
            catch (Exception exception)
            {

                Console.WriteLine($"Error writing log file: { exception.Message}");
            }

        }
        private static void LogMessageToConsole(string message)
        {

            Console.WriteLine(message);

        }
    }
}
