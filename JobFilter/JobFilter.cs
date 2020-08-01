using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobFilter
{
    class JobFilter
    {
        public List<string> IncludeTools { get; set; } = new List<string>();
        public List<string> ExcludeTools { get; set; } = new List<string>();
        public List<string> Tools { get; private set; } = new List<string>();

        private readonly ToolDetector toolDetector = new ToolDetector();

        public bool IsPass(string html)
        {
            Tools.Clear();

            toolDetector.Html = html;
            List<string> tools = toolDetector.Detect();
            if (!IncludeTools.All(x => (tools.Contains(x)))) return false;
            if (!ExcludeTools.All(x => (!tools.Contains(x)))) return false;

            Tools = tools;
            return true;
        }

    }
}
