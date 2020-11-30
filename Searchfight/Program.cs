using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BusinessEntities.Utilities;
using BusinessLogic;

namespace Searchfight
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*args = new string[2];
                args.SetValue(".net", 0);
                args.SetValue("java", 1);*/

                SearchService service = new SearchService();
                string errorMessage = "";
                var results = service.Search(args, out errorMessage);

                if (results != null && string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine();

                    ConsoleHelpers.Print(results.Languages, results.Runners, results.Counts);

                    Console.WriteLine();

                    foreach (var winner in results.Winners)
                        Console.WriteLine("{0} winner: {1}", winner.Key, winner.Value);

                    Console.WriteLine();

                    Console.WriteLine("Total winner: {0}", results.Winner);

                    if (results.Winner != results.NormalizedWinner)
                        Console.WriteLine("Normalized winner: {0}", results.NormalizedWinner); 
                }
                else
                {
                    Console.Write(errorMessage);
                }
                //Console.Write(test());
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);
                Console.ReadKey();
            }
        }

        private static string test()
        {
            string result = "";

            string uriString = "http://www.google.com/search";
            string keywordString = ".net";

            WebClient webClient = new WebClient();

            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("q", keywordString);

            webClient.QueryString.Add(nameValueCollection);
            result = webClient.DownloadString(uriString);
            return result;
        }

    }
}
