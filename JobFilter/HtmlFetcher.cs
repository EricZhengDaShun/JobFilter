using System;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace JobFilter
{
    public class HtmlFetcher
    {
        public string Info { get; private set; }
        public string Url { get; set; }
        public string Html { get; private set; }

        public async Task<bool> Fetch()
        {
            Info = "";
            Html = "";

            bool isSuccess = false;
            try
            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
                {
                    Headless = true
                });
                using var page = await browser.NewPageAsync();
                await page.GoToAsync(Url);
                Html = await page.GetContentAsync();
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
