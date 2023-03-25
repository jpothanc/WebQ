using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebQTests
{
    public static class TestHelper
    {
        public static void CleanDirectory(string directoryName, string fileName, string pattern = "*.json")
        {
            var files = from file in Directory.EnumerateFiles(directoryName, pattern)
                        where file.Contains(fileName)
                        select file;

            foreach (var file in files)
            {
                File.Delete(file);
            }

        }

        public static void CreateFiles(string directoryName, string fileName, string data, int days)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            for (int i = days; --i > 0;)
            {
                var file = Path.Combine(directoryName, $"{fileName}_{DateTime.Now.AddDays(-i).ToString("yyyyMMdd")}.json");
                File.WriteAllText(file, data);
            }
        }

        public static string[] GetRecentFiles(string directoryName, string fileName, int fallBack)
        {
            string[] files = new string[fallBack];
            for (int i = 0; i < fallBack; i++)
            {
                var file = GetAbsoluteCachePath(directoryName, fileName, i);
                files[i] = file;
            }
            return files;
        }

        public static string GetAbsoluteCachePath(string directoryName, string fileName, int addDay = 0)
        {
            return Path.Combine(directoryName, $"{fileName}_{DateTime.Now.AddDays(-addDay).ToString("yyyyMMdd")}.json");
        }
    }
}
