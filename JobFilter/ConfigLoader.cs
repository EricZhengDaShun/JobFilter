using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JobFilter
{
    class ConfigLoader
    {
        public string FileName { get; set; }
        public string Info { get; private set; }
        public List<string> SearchJobResults { get; private set; } = new List<string>();
        public List<string> IncludeTool { get; private set; } = new List<string>();
        public List<string> ExcludeTool { get; private set; } = new List<string>();

        public bool Load()
        {
            SearchJobResults.Clear();
            bool isSuccessful;
            try
            {
                string settingData;
                using (StreamReader sr = new StreamReader(FileName))
                {
                    settingData = sr.ReadToEnd();
                }

                dynamic jObject = JObject.Parse(settingData);
                string urlHead = jObject.urlHead;
                string urlTail = jObject.urlTail;
                int pageNum = jObject.pageNum;
                for (int count = 0; count < pageNum; ++count)
                {
                    SearchJobResults.Add(urlHead + count.ToString() + urlTail);
                }

                IncludeTool = jObject.includeTools.ToObject<List<string>>();
                ExcludeTool = jObject.excludeTools.ToObject<List<string>>();

                isSuccessful = true;
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                Info = ex.Message;
            }

            return isSuccessful;
        }
    }
}
