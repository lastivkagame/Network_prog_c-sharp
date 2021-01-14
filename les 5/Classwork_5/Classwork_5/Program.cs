using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Classwork_5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //UsingWebClient();
            //Using WebRequest
            //UsingWebRequest();
            await UsingHttpClient();
        }

        private async static Task UsingHttpClient()
        {
            var filename = "randomrobot.png";
            var url = "https://robohash.org/{filename}?set=set4";



            var httpclient = new HttpClient();
            var responce = await httpclient.GetAsync(url);

            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var data =  await responce.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(filename, data);
            }
        }

        private static void UsingWebRequest()
        {
            //var request = new HttpWebRequest()
            var url = "https://randomuser.me/api/";
            var request = WebRequest.CreateHttp(url);
            var responce = request.GetResponse();
            var stream = responce.GetResponseStream();
            var sr = new StreamReader(stream);
            var data = sr.ReadToEnd();

            var user = JsonConvert.DeserializeObject<RandomUser>(data);

            Console.WriteLine($"Name: { user.results.FirstOrDefault().name.first }");
            Console.WriteLine($"LastName: { user.results.FirstOrDefault().name.last }");
            Console.WriteLine($"Email: { user.results.FirstOrDefault().email }");
          //  Console.WriteLine(data);
        }

        private static void UsingWebClient()
        {
            var url = "https://www.gutenberg.org/files/63955/63955-0.txt";
            WebClient webClient = new WebClient();

            var data = webClient.DownloadData(url);

            var temp = url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var filename = temp[temp.Count() - 1];

            File.WriteAllBytes(filename, data);
            //throw new NotImplementedException();
        }
    }
}
