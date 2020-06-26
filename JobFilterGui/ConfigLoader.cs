using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace JobFilterGui
{
    public class ConfigLoader
    {
        public JobFilterSetting Setting { get; private set; }
        public string SettingFileName { get; set; }
        public string Info { get; private set; }

        public bool Load()
        {
            bool isSuccessful = true;
            try
            {
                string jsonData;
                using StreamReader sr = new StreamReader(SettingFileName);
                jsonData = sr.ReadToEnd();

                Setting = JsonSerializer.Deserialize<JobFilterSetting>(jsonData);

                if (Setting.HasNull())
                {
                    isSuccessful = false;
                    Info = "Has null!";
                }
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
