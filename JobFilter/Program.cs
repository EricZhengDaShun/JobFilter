using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.104.com.tw/jobs/search/?ro=0&jobcat=2007001000&kwop=7&keyword=c%2B%2B&area=6001001000&order=15&asc=0&page=2&mode=s&jobsource=2018indexpoc";

            HtmlFetcher htmlFetcher = new HtmlFetcher
            {
                Url = url
            };
            Task<bool> isSuccessful = htmlFetcher.Fetch();
            isSuccessful.Wait();
            if (!isSuccessful.Result)
            {
                Console.WriteLine(htmlFetcher.Info);
                Console.ReadKey();
                return;
            }
            
            using (StreamWriter wr = File.CreateText("1.html"))
            {
                wr.Write(htmlFetcher.Html);
            }

            JobLinkDetector jobLinkDetector = new JobLinkDetector()
            {
                Html = htmlFetcher.Html
            };

            var links = jobLinkDetector.Detect();
            /*
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(htmlFetcher.Html);
            List<HtmlNode> toftitle = resultat.DocumentNode.Descendants().Where(
                x => (x.Name == "div" &&
                x.Attributes["id"]?.Value == "js-job-content")).ToList();

            if (toftitle.Count != 1)
            {
                Console.WriteLine("Parse fail !");
                Console.ReadKey();
                return;
            }

            var jobsNode = toftitle[0];
            var l = (from node in jobsNode.Descendants()
                      where node.Name == "a" &&
                      node.Attributes["href"] != null &&
                      node.Attributes["class"]?.Value == "js-job-link "
                      select node.Attributes["href"].Value).ToList();  
            */

            Console.ReadKey();
        }
    }
}
