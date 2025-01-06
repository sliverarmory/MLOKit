using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.VertexAI
{
    class ListModels
    {

        public static async Task execute(string credential, string platform, string projectName)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("list-models", credential, platform));

            // check for additional required arguments
            if (projectName.Equals(""))
            {
                Console.WriteLine("");
                Console.WriteLine("[-] ERROR: Missing one of required command arguments");
                Console.WriteLine("");
                return;
            }

            // ignore SSL errors
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // check if credentials provided are valid
            Console.WriteLine("");
            Console.WriteLine("[*] INFO: Checking credentials provided");
            Console.WriteLine("");

            // if creds valid, then provide message and continue
            if (await Utilities.VertexAI.WebUtils.credsValid(credential, "https://cloudresourcemanager.googleapis.com/v1/projects?alt=json&filter=lifecycleState%3AACTIVE&pageSize=500"))
            {
                Console.WriteLine("[+] SUCCESS: Credentials provided are VALID.");
                Console.WriteLine("");

                try
                {

                    // get a listing of all regions for a given project
                    Console.WriteLine("[*] INFO: Listing regions for the " + projectName + " project");
                    Console.WriteLine("");
                    List<string> regionList = await Utilities.VertexAI.WebUtils.getAllRegions(credential, projectName);

                    foreach (string region in regionList)
                    {
                        Console.WriteLine(region);
                    }


                    // create table header
                    string tableHeader = string.Format("{0,30} | {1,20} | {2,25} | {3,15} | {4,15} | {5,15}", "Name", "Model ID", "Creation Date", "Region", "Model Type", "Export Format");
                    Console.WriteLine(tableHeader);
                    Console.WriteLine(new String('-', tableHeader.Length));

                    // get all the models in each region
                    foreach (string region in regionList)
                    {
                        List<Objects.VertexAI.Model> modelList = await Utilities.VertexAI.ModelUtils.getAllModels(credential, region, projectName);

                        // iterate through the list of models 
                        foreach (Objects.VertexAI.Model mod in modelList)
                        {
                            Console.WriteLine("{0,30} | {1,20} | {2,25} | {3,15} | {4,15} | {5,15}", mod.modelDisplayName, mod.modelID, mod.createTime, mod.region, mod.sourceType, mod.exportableFormat);

                        }
                    }


                    Console.WriteLine("");



                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine("[-] ERROR: " + ex.Message);
                    Console.WriteLine("");
                }


            }

            // if creds not valid, display message and return
            else
            {
                Console.WriteLine("[-] ERROR: Credentials provided are INVALID. Check the credentials again.");
                Console.WriteLine("");
                return;
            }


        }

    }
}
