using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace JobFilter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configLoader = new ConfigLoader()
            {
                FileName = "setting.json"
            };

            if (!configLoader.Load())
            {
                Console.WriteLine("Load setting file fail !");
                Console.WriteLine(configLoader.Info);
                Console.ReadKey();
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var jobLinks = new List<string>();
            var htmlFetcher = new HtmlFetcher();
            var jobLinkDetector = new JobLinkDetector();
            foreach (var serachJobLink in configLoader.SearchJobResults)
            {
                htmlFetcher.Url = serachJobLink;
                bool isSuccess = await htmlFetcher.Fetch();
                if (!isSuccess) continue;
                jobLinkDetector.Html = htmlFetcher.Html;
                jobLinks.AddRange(jobLinkDetector.Detect());
            }

            var jobFilter = new JobFilter()
            {
                IncludeTools = configLoader.IncludeTool,
                ExcludeTools = configLoader.ExcludeTool
            };

            var result = new List<string>();
            foreach (var jobLink in jobLinks)
            {
                jobFilter.Url = jobLink;
                if (jobFilter.IsPass())
                {
                    result.Add(jobLink);
                    result.AddRange(jobFilter.Tools);
                }
            }

            stopwatch.Stop();
            File.WriteAllLines("result.txt", result);
            Console.WriteLine("Done !");
            Console.WriteLine("Spend {0} s", stopwatch.Elapsed.TotalSeconds);
            Console.ReadKey();
        }
    }
}
