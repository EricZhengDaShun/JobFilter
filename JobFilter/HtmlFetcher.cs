using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JobFilter
{
    public class HtmlFetcher : IDisposable
    {
        private readonly IWebDriver driver;

        public HtmlFetcher()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            driver = new ChromeDriver(chromeOptions);
        }

        public void Dispose()
        {
            driver.Close();
            driver.Quit();
        }

        public string LoadUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
            return driver.PageSource;
        }
    }
}
