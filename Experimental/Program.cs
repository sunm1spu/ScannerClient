using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTPTestPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            int loop = 1;
            WebCalls callClient = new WebCalls();

            while (loop == 1) {
                Console.WriteLine("Entering Webcalls \n");
                callClient.ScanCall("test", client);
                Console.ReadLine();
            }
           
        }
    }
}
