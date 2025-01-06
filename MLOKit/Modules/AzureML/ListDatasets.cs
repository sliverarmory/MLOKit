using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.AzureML
{
    class ListDatasets
    {

        public static async Task execute(string credential, string platform, string subscriptionID, string region, string resourceGroup, string workspace)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("list-datasets", credential, platform));

            // check for additional required arguments
            if (subscriptionID.Equals("") || region.Equals("") || resourceGroup.Equals("") || workspace.Equals(""))
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

            try
            {

                Console.WriteLine("");
                Console.WriteLine("[*] INFO: Performing list-datasets module for " + platform);
                Console.WriteLine("");

                // check if credentials provided are valid
                Console.WriteLine("[*] INFO: Checking credentials provided");
                Console.WriteLine("");

                // if creds valid, then provide message
                if (await Utilities.AzureML.WebUtils.credsValid(credential, "https://management.azure.com/subscriptions?api-version=2022-12-01"))
                {
                    Console.WriteLine("[+] SUCCESS: Credentials provided are VALID.");
                    Console.WriteLine("");


                    try
                    {


                        // create table header
                        string tableHeader = string.Format("{0,50} | {1,40} | {2,10} | {3,15} | {4,20}", "File Name", "ID", "State", "File Type", "Datastore Name");
                        Console.WriteLine(tableHeader);
                        Console.WriteLine(new String('-', tableHeader.Length));

                        // get a listing of all datasets the user has access to in Azure
                        List<Objects.AzureML.Dataset> datasetList = await Utilities.AzureML.DatasetUtils.getAllDatasets(credential, subscriptionID, region, resourceGroup, workspace);

                        // iterate through the list of datasets 
                        foreach (Objects.AzureML.Dataset datSet in datasetList)
                        {
                            string fileName = datSet.datasetFilePath;
                            int lastIndex = fileName.LastIndexOf('/');
                            fileName = fileName.Substring(lastIndex + 1);
                            Console.WriteLine("{0,50} | {1,40} | {2,10} | {3,15} | {4,20}", fileName, datSet.datasetID, datSet.datasetState, datSet.datasetFileType, datSet.datastoreName);

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

                // if creds not valid, display message
                else
                {
                    Console.WriteLine("[-] ERROR: Credentials provided are INVALID. Check the credentials again.");
                    Console.WriteLine("");
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] ERROR: " + ex.Message);
                Console.WriteLine("");
            }


        }

    }
}
