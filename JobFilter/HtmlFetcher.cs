using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JobFilter
{
    public class HtmlFetcher
    {
        public string Info { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }

        private HttpClient client = new HttpClient();

        public async Task<bool> Fetch()
        {
            Info = "";
            Html = "";

            bool isSuccess = false;
            try
            {
                HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                Html = await response.Content.ReadAsStringAsync();
                isSuccess = true;
            }
            catch (HttpRequestException e)
            {
                Info = e.Message;
            }
            catch (Exception e)
            {
                Info = e.Message;
            }
            return isSuccess;
        }

    }
}
