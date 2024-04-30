using System;
using System.Net.Http;
using System.Globalization;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Net;

namespace UHFReader18demomain
{

    public class WebCalls
    {
        public const string DEVURL = "http://localhost:5000";

        public const string LOGSCAN = "/api/inventory/scan";

        // Time of last call trackers
        // private (int, DateTime) lastScan;
        // Dictionary of scan values and last time it was sent as a request
        // private Dictionary<string, DateTime> lastScan = new Dictionary<string, DateTime>();

        public WebCalls()
        {

        }

        public async void ScanCall(List<String[]> scanPackage, HttpClient client)
        {
            // The scan call will only need the scan data and will assign a timestamp on the serverside
            // Previous problems:
            // content was empty with FormURL encoding, now uses JsonSerializer
            Console.WriteLine("Generating payload \n");
            var payloadJSON = new PostData
            {
                SCANS = scanPackage

            };

            // 4.5.2 missing namespace json
            var json = JsonSerializer.Serialize(payloadJSON);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                    var response = await client.PostAsync(DEVURL + LOGSCAN, payload);
                    var responseString = await response.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("HTTP REQUEST ERROR: " + e);
            }

        }
        public bool Debouncer(string ENDPOINT, String[] data)
        {
            // Get time passed between last call and current call
            DateTime currentTime = DateTime.Now;
            DateTime firstTime = DateTime.Parse(data[1]);

            TimeSpan timeDiff = currentTime.Subtract(firstTime);
            int timeDiffMS = timeDiff.Milliseconds;

            // Continue if enough time has passed
            if (timeDiffMS > RateLimiter(ENDPOINT))
            {
                return true;
            }

            // Not enough time has passed
            else
            {
                Console.WriteLine("NOT ENOUGH TIME HAS PASSED \n");
                Console.WriteLine(timeDiffMS);

                return false;
            }
        }

        private int RateLimiter(string ENDPOINT)
        {
            // Return ints are values in milliseconds

            switch (ENDPOINT)
            {
                case LOGSCAN:
                    return 100000;
                    break;
                default:
                    return 0;
                    break;
            }
        }
    }
}

