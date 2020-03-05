using System.Collections.Generic;
using System.Threading.Tasks;

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

            Task<bool> isSuccessful = htmlFetcher.Fetch();
            if (!isSuccessful.IsCompleted) isSuccessful.Wait();
            if (!isSuccessful.Result) return false;

            toolDetector.Html = htmlFetcher.Html;
            List<string> tools = toolDetector.Detect();
            bool found = true;
            foreach (var tool in IncludeTools)
            {
                if (!tools.Contains(tool))
                {
                    found = false;
                    break;
                }
            }
            if (!found) return false;

            found = false;
            foreach (var tool in ExcludeTools)
            {
                if (tools.Contains(tool))
                {
                    found = true;
                    break;
                }
            }
            if (found) return false;

            Tools = tools;
            return true;
        }

    }
}
