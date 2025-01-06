using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.BigML
{
    class ListModels
    {

        public static async Task execute(string credential, string platform)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("list-models", credential, platform));


            // ignore SSL errors
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            Console.WriteLine("");
            Console.WriteLine("[*] INFO: Performing list-models module for " + platform);
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
                    string tableHeader = string.Format("{0,40} | {1,10} | {2,20} | {3,25} | {4,30}", "Name", "Visibility", "Created By", "Creation Date", "Model ID");
                    Console.WriteLine(tableHeader);
                    Console.WriteLine(new String('-', tableHeader.Length));

                    // get a listing of all models the user has access to in BigML
                    List<Objects.BigML.Model> modelList = await Utilities.BigML.ModelUtils.getAllModels(credential, "https://bigml.io");

                    // iterate through the list of models 
                    foreach (Objects.BigML.Model model in modelList)
                    {
                        Console.WriteLine("{0,40} | {1,10} | {2,20} | {3,25} | {4,30}", model.modelName, model.visibility, model.createdBy, model.dateCreated, model.modelID);

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
