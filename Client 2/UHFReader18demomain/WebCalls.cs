using System;
using System.Net.Http;
using System.Globalization;
using System.Collections.Generic;

namespace UHFReader18demomain {

    public class WebCalls
    {
        public const string DEVURL = "http://localhost:5001";

        public const string LOGSCAN = "/api/inventory/scan";

        public WebCalls()
        {

        }

        public static async void ScanCall(string data, HttpClient client)
        {
            var newRequest = new HttpRequestMessage();
            // The scan call will only need the scan data and will assign a timestamp on the serverside
            /*var values = new Dictionary<string, string>
            {
                { "scanData", data }
            };
            */

            // dummy values
            var values = new Dictionary<string, string>
            {
                { "scanData", "E20040D40000000000000000" }
            };
            // some reason content is empty
            var content = new FormUrlEncodedContent(values);
            newRequest.Content = content;
            Console.WriteLine("Content: " + content.ReadAsStringAsync());

            try
            {
                var response = await client.PostAsync(DEVURL + LOGSCAN, content);

                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Request Body: " + content.ToString());

                Console.WriteLine("Response: " + responseString);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("HTTP REQUEST ERROR: " + e);
            }
            
        }
    }


}

