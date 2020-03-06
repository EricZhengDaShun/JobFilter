using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JobFilter
{
    class Program
    {
        private static readonly object lockResultObject = new object();
        private static readonly List<string> result = new List<string>();
        private static readonly ConfigLoader configLoader = new ConfigLoader()
        {
            FileName = "setting.json"
        };

        private static async Task GetJobLinks(string link)
        {
            var htmlFetcher = new HtmlFetcher();
            var jobLinkDetector = new JobLinkDetector();
            htmlFetcher.Url = link;
            bool isSuccess = htmlFetcher.Fetch();
            if (!isSuccess) return;
            jobLinkDetector.Html = htmlFetcher.Html;
            List<string> jogLinks = jobLinkDetector.Detect();

            var taskList = new List<Task>();
            foreach (var jogLink in jogLinks)
            {
                taskList.Add(Task.Run(() => CheckJob(jogLink)));
            }
            await Task.WhenAll(taskList);
            return;
        }

        private static void CheckJob(string link)
        {
            var jobFilter = new JobFilter()
            {
                IncludeTools = configLoader.IncludeTool,
                ExcludeTools = configLoader.ExcludeTool
            };

            jobFilter.Url = link;
            if (jobFilter.IsPass())
            {
                lock (lockResultObject)
                {
                    result.AddRange(jobFilter.Tools);
                    result.Add(link);
                }
            }

            return;
        }

        static async Task Main(string[] args)
        {
            if (!configLoader.Load())
            {
                Console.WriteLine("Load setting file fail !");
                Console.WriteLine(configLoader.Info);
                Console.ReadKey();
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var getJobListTasks = new List<Task>();
            foreach (var serachJobLink in configLoader.SearchJobResults)
            {
                getJobListTasks.Add(GetJobLinks(serachJobLink));
            }
            await Task.WhenAll(getJobListTasks);

            stopwatch.Stop();
            File.WriteAllLines("result.txt", result);
            Console.WriteLine("Done !");
            Console.WriteLine("Spend {0} s", stopwatch.Elapsed.TotalSeconds);
            Console.ReadKey();
        }
    }
}
