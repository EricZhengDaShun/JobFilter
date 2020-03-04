using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;

namespace JobFilter
{
    class JobLinkDetector
    {
        public string Html { get; set; }

        private readonly HtmlDocument htmlDocument = new HtmlDocument();

        public List<string> Detect()
        {
            htmlDocument.LoadHtml(Html);
            List<HtmlNode> jobContents = htmlDocument.DocumentNode.Descendants().Where(
                x => (x.Name == "div" &&
                x.Attributes["id"]?.Value == "js-job-content")).ToList();

            if (jobContents.Count != 1)
            {
                return new List<string>();
            }

            var jobsNode = jobContents[0];
            List<string> links = (from node in jobsNode.Descendants()
                     where node.Name == "a" &&
                     node.Attributes["href"] != null &&
                     node.Attributes["class"]?.Value == "js-job-link "
                     select node.Attributes["href"].Value).ToList();

            return links;
        }
    }
}
