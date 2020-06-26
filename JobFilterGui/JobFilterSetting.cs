using System;
using System.Collections.Generic;
using System.Text;

namespace JobFilterGui
{
    public class JobFilterSetting
    {
        public string Url { get; set; }
        public string[] IncludeTools { get; set; }
        public string[] ExcludeTools { get; set; }
        public int? PageNum { get; set; }
        public GetAttrValue JobLinkHtml { get; set; }
        public GetNodeContent ToolHtml { get; set; }
        public GetNodeContent JobTitle { get; set; }
        public GetNodeContent Salary { get; set; }

        public bool HasNull()
        {
            if (Url == null) return true;
            if (IncludeTools == null) return true;
            if (ExcludeTools == null) return true;
            if (PageNum == null) return true;

            if (JobLinkHtml == null) return true;
            if (JobLinkHtml.HasNull()) return true;

            if (ToolHtml == null) return true;
            if (ToolHtml.HasNull()) return true;

            if (JobTitle == null) return true;
            if (JobTitle.HasNull()) return true;

            if (Salary == null) return true;
            if (Salary.HasNull()) return true;
            return false;
        }

    }

    public class GetAttrValue
    {
        public string Tag { get; set; }
        public string Attr { get; set; }
        public string AttrValue { get; set; }
        public string Target { get; set; }

        public bool HasNull()
        {
            if (Tag == null) return true;
            if (Attr == null) return true;
            if (AttrValue == null) return true;
            if (Target == null) return true;
            return false;
        }
    }

    public class GetNodeContent
    {
        public string Tag { get; set; }
        public string Attr { get; set; }
        public string AttrValue { get; set; }

        public bool HasNull()
        {
            if (Tag == null) return true;
            if (Attr == null) return true;
            if (AttrValue == null) return true;
            return false;
        }
    }
}
