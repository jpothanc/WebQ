using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebQ;
using Xunit;

namespace WebQTests
{
    [Collection("Sequential")]
    public class DiskCacheTests
    {

        public static IEnumerable<object[]> OkTestData()
        {
            yield return new object[] { new DiskOptions() { CachePath = Path.Combine("C:","temp"), BackupDays = 3 },
            "webq-cacheName", "data"};
            yield return new object[] { new DiskOptions() { CachePath = Path.Combine("C:","temp", "webcache"), BackupDays = 3 },
            "webq-cacheName1", "data1"};
        }

        public static IEnumerable<object[]> FailTestData()
        {
            yield return new object[] { new DiskOptions() { CachePath = Path.Combine("C:","temp?"), BackupDays = 3 },
            "webq-cacheName", "data"};
            yield return new object[] { new DiskOptions() { CachePath = Path.Combine("C:","temp", "webcache>"), BackupDays = 3 },
            "webq-cacheName1", "data1"};
            yield return new object[] { new DiskOptions() { CachePath = "", BackupDays = 3 },
            "webq-cacheName1", "data1"};
        }

        [Theory]
        [MemberData(nameof(OkTestData))]
        public void TestMethod_SaveToDisk_SavesContentsToDisk(DiskOptions options, string name, string data)
        {
            IDiskCache disk = new DiskCache(options);
            var exception = Record.Exception(() => disk.Save(data, name));
            Assert.Null(exception);
        }


        [Theory]
        [MemberData(nameof(OkTestData))]
        public void TestMethod_SaveToDisk_SavesContentsToDiskWithCorrectDateAndFormat(DiskOptions options, string name, string data)
        {
            IDiskCache disk = new DiskCache(options);

            var files = Directory.GetFiles(options.CachePath, "*.json", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            var exception = Record.Exception(() => disk.Save(data, name));
            Assert.Null(exception);
            
            var targetFileFormat = Path.Combine(options.CachePath, $"{name}_{DateTime.Now.ToString("yyyyMMdd")}.json" );
            files = Directory.GetFiles(options.CachePath, "*.json", SearchOption.TopDirectoryOnly);
            Assert.True(files.Where(x => x == targetFileFormat).ToList().Count() == 1);
            
        }

        [Theory]
        [MemberData(nameof(FailTestData))]
        public void TestMethod_SaveToInvalidDirectory_ThrowsIOExceptionException(DiskOptions options, string name, string data)
        {
            IDiskCache disk = new DiskCache(options);
            var exception = Record.Exception(() => disk.Save(data, name));
            Assert.True(exception is IOException);
            Assert.Contains("Cannot write data to", exception.Message);
        }

        [Theory]
        [MemberData(nameof(OkTestData))]
        public void TestMethod_ReadFromDisk_ReadsContentsFromDisk(DiskOptions options, string name, string data)
        {
            IDiskCache disk = new DiskCache(options);
            var exception = Record.Exception(() => disk.Save(data, name));
            Assert.Null(exception);
            var diskData = disk.Read(name);
            Assert.Equal(data, diskData);
        }

        [Theory]
        [MemberData(nameof(OkTestData))]
        public void TestMethod_ReadFromDisk_FallsBackToPreviousDay(DiskOptions options, string name, string data)
        {
            IDiskCache disk = new DiskCache(options);
            TestHelper.CleanDirectory(options.CachePath,name);
            TestHelper.CreateFiles(options.CachePath, name, data, 2);
            var diskData = disk.Read(name);
            Assert.Equal(data, diskData);
        }

        [Theory]
        [MemberData(nameof(OkTestData))]
        public void TestMethod_ReadFromDisk_FallsBackToPreviousDay1(DiskOptions options, string name, string data)
        {
            
            TestHelper.CleanDirectory(options.CachePath, name);
            TestHelper.CreateFiles(options.CachePath, name, data, 10);
            var oldFiles = TestHelper.GetRecentFiles(options.CachePath, name, 10);
            IDiskCache disk = new DiskCache(options);
            var newFiles = TestHelper.GetRecentFiles(options.CachePath, name, options.BackupDays);
        }


        [Theory]
        [MemberData(nameof(FailTestData))]
        public void TestMethod_ReadFromInvalidDirectory_ThrowsIOExceptionException(DiskOptions options, string name, string data)
        {
            IDiskCache disk = new DiskCache(options);
            Assert.True(!string.IsNullOrEmpty(data));
            var exception = Record.Exception(() => _ = disk.Read(name));
            Assert.True(exception is IOException);
            Assert.Contains("Cannot read data from", exception.Message);
        }


        //[1]Saves Data with todays date
        //[2]File name format and type test //products-stocks.20230322.json
        //Read Todays date
        //If todays date fails read from yesterday
        //Book keeps 


    }
}