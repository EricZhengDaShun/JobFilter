using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobFilter
{
    public class HtmlFetchers : IDisposable
    {
        private readonly List<Task> taskList = new List<Task>();
        private bool isRun = true;

        public ConcurrentQueue<string> Urls { get; set; } = new ConcurrentQueue<string>();
        public ConcurrentQueue<WebInfo> WebInfos { get; set; } = new ConcurrentQueue<WebInfo>();

        static public readonly string HtmlError = "Fetch fail!";

        public HtmlFetchers(int num)
        {
            for (int count = 0; count < num; ++count)
            {
                taskList.Add(Task.Run(() => FetchTask(ref isRun)));
            }

        }

        public void Dispose()
        {
            isRun = false;
            Task.WhenAll(taskList).Wait();
        }

        private void FetchTask(ref bool isRun)
        {
            using HtmlFetcher htmlFetcher = new HtmlFetcher();
            string html;
            while (isRun)
            {
                if (!Urls.TryDequeue(out string url))
                {
                    Thread.Sleep(100);
                    continue;
                }

                html = htmlFetcher.LoadUrl(url);
                if (string.IsNullOrEmpty(html))
                {
                    html = HtmlError;
                }

                WebInfos.Enqueue(new WebInfo
                {
                    Url = url,
                    Html = html
                });
            }
            return;
        }
    }
}
