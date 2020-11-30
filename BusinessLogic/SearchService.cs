using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Xml.Serialization;
using System.IO;
using BusinessEntities.Utilities;

namespace BusinessLogic
{
    public class SearchService
    {
        public Results Search(string[] args, out string errorMessage)
        {
            Results results = null;
            errorMessage = "";
            try
            {
                if (args.Length == 0)
                    throw new ConfigurationException("Expected at least one argument.");

                var runners = ReadConfiguration(out errorMessage).SearchRunners.Where(runner => !runner.Disabled).ToList();
                results = CollectResults(args, runners).Result;

            }
            catch (ConfigurationException ex)
            {
                errorMessage = ex.Message;
            }
            catch (AggregateException ex)
            {
                string agregateErrorMessage = "";
                ex.Handle(e =>
                {
                    var searchException = e as SearchException;

                    if (searchException != null)
                    {
                        agregateErrorMessage = string.Format("Runner '{0}' failed. {1}", searchException.Runner, searchException.Message);
                        return true;
                    }
                    else
                        return false;
                });
                errorMessage = agregateErrorMessage;
            }
            return results;
        }

        private static async Task<Results> CollectResults(IReadOnlyList<string> languages, IReadOnlyList<ISearchRunner> runners)
        {
            using (var reporter = new ConsoleProgressReporter("Running..."))
            {
                return await Results.Collect(languages, runners, reporter);
            }
        }

        //private Configuration ReadConfiguration()
        private Configuration ReadConfiguration(out string errorMessage)
        {
            Configuration configuration = new Configuration();
            errorMessage = "";
            try
            {
                using (var stream = File.OpenRead("Configuration.xml"))
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(Configuration));
                        configuration = (Configuration)serializer.Deserialize(stream);
                    }
                    catch (InvalidOperationException ex)
                    {
                        errorMessage = "The configuration file is invalid. " + ex.Message;
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {                
                errorMessage = "Unauthorized exception: " + ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                errorMessage = "Could not find configuration file. " + ex.Message;
            }
            catch (IOException ex)
            {
                errorMessage = "An error occurred when reading configuration file. " + ex.Message;
            }
            return configuration;
        }
    }
}
