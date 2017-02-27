using System;
using System.Collections.Generic;
using System.IO;

namespace AutomatedTest.Database.Extensions
{
    class FileExtensions
    {
        public static string BuildSQL(string fileName, string fieldName = null)
        {
            var sqlFile = $@"..\SqlScripts\{fileName}.sql";

            string result;
            try
            {
                result = File.ReadAllText(sqlFile);
                //using (var sqlStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(sqlFile)))
                //{
                //    result = sqlStream.ReadToEnd();
                //}
            }
            catch (Exception e)
            {
                throw new FileNotFoundException(
                    $"File not found: {sqlFile}\r\nCurrent path is {Directory.GetCurrentDirectory()}");
            }
            if (fieldName != null)
            {
                result = result.Replace("COLUMN_TO_BE_REPLACED", fieldName);
            }
            var environment = BuildConfiguration();

            result = result.Replace("$env$", environment);
            return result;
        }

        public static string BuildConfiguration()
        {
            var environment = "DEV";
#if QA
            environment= "QA";
#endif
            return environment;
        }

        public static string BuildSQL(string fileName, Dictionary<string, string> parameters)
        {
            var environmentCorrectedSQL = BuildSQL(fileName);
            return environmentCorrectedSQL.ReplaceParametersFromDictionary(parameters);
        }


    }
}
