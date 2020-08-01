using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                x.Attributes["data-v-1ffbadf4"] != null))
                .Select(n => n.InnerText)
                .ToList();

            return tools;
        }
    }
}
