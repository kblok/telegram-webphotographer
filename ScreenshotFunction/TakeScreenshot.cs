
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using PuppeteerSharp;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ScreenshotFunction
{
    public static class TakeScreenshot
    {
        [FunctionName("TakeScreenshot")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string url = req.Query["url"];

            if (url == null)
            {
                return new BadRequestObjectResult("Please pass a name on the query string");
            }
            else
            {
                string apikey = "";
                var options = new ConnectOptions()
                {
                    BrowserWSEndpoint = $"wss://chrome.browserless.io?token={apikey}"
                };


                var browser = await Puppeteer.ConnectAsync(options);
                var page = await browser.NewPageAsync();
                
                await page.GoToAsync(url);
                var stream = await page.ScreenshotStreamAsync(new ScreenshotOptions
                {
                    FullPage = true
                });

                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);
                stream.Dispose();

                await page.CloseAsync();
                browser.Disconnect();

                return new FileContentResult(bytesInStream, "image/png");
            }
        }
    }
}
