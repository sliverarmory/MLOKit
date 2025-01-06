using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.BigML
{
    class ListDatasets
    {

        public static async Task execute(string credential, string platform)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("list-datasets", credential, platform));


            // ignore SSL errors
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.WriteLine("");
            Console.WriteLine("[*] INFO: Performing list-datasets module for " + platform);
            Console.WriteLine("");


            // check if credentials provided are valid
            Console.WriteLine("[*] INFO: Checking credentials provided");
            Console.WriteLine("");

            // if creds valid, then provide message and continue
            if (await Utilities.BigML.WebUtils.credsValid(credential, "https://bigml.io"))
            {
                Console.WriteLine("[+] SUCCESS: Credentials provided are VALID.");
                Console.WriteLine("");

                try
                {

                    // create table header
                    string tableHeader = string.Format("{0,40} | {1,10} | {2,25} | {3,30}", "Name", "Visibility", "Creation Date", "Dataset ID");
                    Console.WriteLine(tableHeader);
                    Console.WriteLine(new String('-', tableHeader.Length));

                    // get a listing of all datasets the user has access to in BigML
                    List<Objects.BigML.Dataset> datasetList = await Utilities.BigML.DatasetUtils.getAllDatasets(credential, "https://bigml.io");

                    // iterate through the list of datasets 
                    foreach (Objects.BigML.Dataset datSet in datasetList)
                    {
                        Console.WriteLine("{0,40} | {1,10} | {2,25} | {3,30}", datSet.datasetName, datSet.visibility, datSet.dateCreated, datSet.datasetID);

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
