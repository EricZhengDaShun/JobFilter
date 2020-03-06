using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JobFilter
{
    public class HtmlFetcher
    {
        public string Info { get; private set; }
        public string Url { get; set; }
        public string Html { get; private set; }

        public bool Fetch()
        {
            Info = "";
            Html = "";

            bool isSuccess = false;
            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("headless");
                IWebDriver driver = new ChromeDriver(chromeOptions);
                driver.Navigate().GoToUrl(Url);
                Html = driver.PageSource;
                driver.Close();
                driver.Quit();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Info = ex.Message;
            }
            return isSuccess;
        }

    }
}
