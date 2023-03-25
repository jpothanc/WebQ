using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebQ
{
    public class DiskCache : IDiskCache
    {
        private readonly DiskOptions _options;

        public DiskCache(DiskOptions options)
        {
            this._options = options;
            DeleteNonRecentFiles();
        }

        public string Read(string fileName)
        {
            string data;
            try
            {
                var recentFile = GetRecentFile(fileName, _options.BackupDays);
                data = File.ReadAllText(recentFile);
            }
            catch (Exception e)
            {
                throw new IOException($"Cannot read data from {GetAbsoluteCachePath(fileName)}-{e.Message}");
            }
            return data;
        }

        public void Save(string data, string fileName)
        {
            try
            {
                if (!Directory.Exists(_options.CachePath))
                    Directory.CreateDirectory(_options.CachePath);

                File.WriteAllText(GetAbsoluteCachePath(fileName), data);
            }
            catch (Exception e)
            {
                throw new IOException($"Cannot write data to {GetAbsoluteCachePath(fileName)}-{e.Message}");
            }
        }
        #region Private        
        private void DeleteNonRecentFiles()
        {
            try
            {
                var files = from file in Directory.EnumerateFiles(_options.CachePath, "*.json")
                            where File.GetCreationTime(file) < DateTime.Now.AddDays(-_options.BackupDays)
                            select file;

                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception)
            {

            }
        }
        private string[] GetRecentFiles(string fileName, int fallBack)
        {
            string[] files = new string[fallBack];

            for (int i = 0; i < fallBack; i++)
            {
                var file = GetAbsoluteCachePath(fileName, i);
                files[i] = file;
            }

            return files;
        }
        private string GetRecentFile(string fileName, int fallBack)
        {
            var files = GetRecentFiles(fileName, fallBack);

            foreach (var file in files)
            {
                if (File.Exists(file))
                    return file;
            }

            throw new FileNotFoundException($"File not found {fileName}");
        }
        private string GetAbsoluteCachePath(string fileName, int addDay = 0)
        {
            return Path.Combine(_options.CachePath, $"{fileName}_{DateTime.Now.AddDays(-addDay).ToString("yyyyMMdd")}.json");
        }
        #endregion
    }
}
