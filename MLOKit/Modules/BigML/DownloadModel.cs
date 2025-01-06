using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;

namespace MLOKit.Modules.BigML
{
    class DownloadModel
    {
        public static async Task execute(string credential, string platform, string modelID)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("download-model", credential, platform));

            // check for additional required arguments
            if (modelID.Equals(""))
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

            Console.WriteLine("");
            Console.WriteLine("[*] INFO: Performing download-model module for " + platform);
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


                    Console.WriteLine("[*] INFO: Downloading model in PMML format with ID " + modelID + " to the current working directory of " + Environment.CurrentDirectory);
                    Console.WriteLine("");

                    // download the model in pmml format
                    string modelContent = await Utilities.BigML.ModelUtils.downloadModel(credential, modelID);

                    // if we got dataset back, then proceed
                    if (modelContent != "")
                    {

                        // create random file name
                        string fileName = Utilities.FileUtils.generateRandomName();
                        fileName = "MLOKit-" + fileName + ".xml";

                        // write model pmml file to disk
                        File.WriteAllText(fileName, modelContent);


                        Console.WriteLine("[+] SUCCESS: Model written to: " + Environment.CurrentDirectory + "\\" + fileName);
                        Console.WriteLine("");

                        Console.WriteLine("");
                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine("[-] ERROR: " + ex.Message);
                    Console.WriteLine("");
                    return;
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
