using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace JobFilter
{
    class ToolDetector
    {
        public string Html { get; set; }

        private readonly HtmlDocument htmlDocument = new HtmlDocument();

        public List<string> Detect()
        {
            htmlDocument.LoadHtml(Html);

            List<string> tools = htmlDocument.DocumentNode.Descendants()
                .Where(x => (x.Name == "u" && 
                x.Attributes["data-v-cff81d40"] != null))
                .Select(n => n.InnerText)
                .ToList();

            return tools;
        }
    }
}
