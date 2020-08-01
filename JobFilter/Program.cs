using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace JobFilter
{
    class Program
    {
        private static readonly ConfigLoader configLoader = new ConfigLoader()
        {
            FileName = "setting.json"
        };

        static void Main()
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

            using HtmlFetchers htmlFetchers = new HtmlFetchers(Environment.ProcessorCount);
            JobFilter jobFilter = new JobFilter()
            {
                IncludeTools = configLoader.IncludeTool,
                ExcludeTools = configLoader.ExcludeTool
            };

            List<string> urls = new List<string>();
            urls.AddRange(configLoader.SearchJobResults);

            int getJobLinkErrorNum = GetJobLink(htmlFetchers, urls, out List<string> jobLinks);
            int getToolsErrorNum = GetTools(htmlFetchers, jobFilter, jobLinks, out List<string> jobInfos);

            stopwatch.Stop();
            File.WriteAllLines("result.txt", jobInfos);
            Console.WriteLine("Done !");
            Console.WriteLine("Spend {0} s", stopwatch.Elapsed.TotalSeconds);
            Console.WriteLine("getJobLinkErrorNum: {0}", getJobLinkErrorNum);
            Console.WriteLine("getToolsErrorNum: {0}", getToolsErrorNum);
            Console.ReadKey();
        }

        static int GetJobLink(HtmlFetchers htmlFetchers, List<string> urls, out List<string> jobLinks)
        {
            urls.ForEach(url => htmlFetchers.Urls.Enqueue(url));

            while (htmlFetchers.WebInfos.Count != urls.Count)
            {
                Thread.Sleep(500);
            }

            JobLinkDetector jobLinkDetector = new JobLinkDetector();
            jobLinks = new List<string>();
            int errorNum = 0;

            while (htmlFetchers.WebInfos.TryDequeue(out WebInfo webInfo))
            {
                if (webInfo.Html == HtmlFetchers.HtmlError)
                {
                    ++errorNum;
                    continue;
                }
                jobLinkDetector.Html = webInfo.Html;
                List<string> links = jobLinkDetector.Detect();
                jobLinks.AddRange(links);
            }

            return errorNum;
        }

        static int GetTools(HtmlFetchers htmlFetchers, JobFilter jobFilter, List<string> urls, out List<string> jobInfos)
        {
            urls.ForEach(url => htmlFetchers.Urls.Enqueue(url));

            while (htmlFetchers.WebInfos.Count != urls.Count)
            {
                Thread.Sleep(500);
            }

            jobInfos = new List<string>();
            int errorNum = 0;

            while (htmlFetchers.WebInfos.TryDequeue(out WebInfo webInfo))
            {
                if (webInfo.Html == HtmlFetchers.HtmlError)
                {
                    ++errorNum;
                    continue;
                }

                if (jobFilter.IsPass(webInfo.Html))
                {
                    jobInfos.AddRange(jobFilter.Tools);
                    jobInfos.Add(webInfo.Url);
                }
            }

            return errorNum;
        }
    }
}
