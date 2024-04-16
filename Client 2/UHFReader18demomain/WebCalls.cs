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
        public const string DEVURL = "http://localhost:5001";
        public const string LOGSCAN = "/api/inventory/scan";

        // Time of last call trackers
        // private (int, DateTime) lastScan;
        // Dictionary of scan values and last time it was sent as a request
        private Dictionary<string, DateTime> lastScan = new Dictionary<string, DateTime>();

        public WebCalls()
        {

        }

        public async void ScanCall(string data, HttpClient client)
        {
            // The scan call will only need the scan data and will assign a timestamp on the serverside
            // Previous problems:
            // content was empty with FormURL encoding, now uses JsonSerializer
            Console.WriteLine("Generating payload \n");
            var payloadJSON = new PostData
            {
                scanData = data

            };

            var json = JsonSerializer.Serialize(payloadJSON);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                if (Debouncer(LOGSCAN, data, client))
                {
                    var response = await client.PostAsync(DEVURL + LOGSCAN, payload);
                    var responseString = await response.Content.ReadAsStringAsync();

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("HTTP REQUEST ERROR: " + e);
            }

        }
        private bool Debouncer(string ENDPOINT, string data, HttpClient client)
        {
            // Get time passed between last call and current call
            DateTime currentTime = DateTime.Now;

            // Max int, about 24 days
            int timeDiffCalls = 2147483647;

            // If data has been passed before
            if (lastScan.ContainsKey(data))
            {

                // Should deal with overflow somehow
                timeDiffCalls = currentTime.Subtract(lastScan[data]).Milliseconds;
            }
            else
            {
                lastScan.Add(data, currentTime);
            }

            // Continue if enough time has passed
            if (timeDiffCalls > RateLimiter(ENDPOINT))
            {
                lastScan[data] = currentTime;

                return true;
            }

            // Not enough time has passed
            else
            {
                Console.WriteLine("NOT ENOUGH TIME HAS PASSED \n");
                Console.WriteLine(timeDiffCalls);

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

