using System.Collections.Generic;
using System.Linq;

namespace JobFilter
{
    class JobFilter
    {
        public List<string> IncludeTools { get; set; } = new List<string>();
        public List<string> ExcludeTools { get; set; } = new List<string>();
        public List<string> Tools { get; private set; } = new List<string>();
        public string Url
        {
            get { return htmlFetcher.Url; }
            set { htmlFetcher.Url = value; }
        }

        private readonly HtmlFetcher htmlFetcher = new HtmlFetcher();
        private readonly ToolDetector toolDetector = new ToolDetector();

        public bool IsPass()
        {
            Tools.Clear();

            bool isSuccessful = htmlFetcher.Fetch();
            if (!isSuccessful) return false;

            toolDetector.Html = htmlFetcher.Html;
            List<string> tools = toolDetector.Detect();
            if (!IncludeTools.All(x => (tools.Contains(x)))) return false;
            if (!ExcludeTools.Any(x => (!tools.Contains(x)))) return false;

            Tools = tools;
            return true;
        }

    }
}
