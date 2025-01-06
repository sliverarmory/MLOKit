using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.AzureML
{
    class Check
    {


        public static async Task execute(string credential, string platform)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("check", credential, platform));


            // ignore SSL errors
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {

                Console.WriteLine("");
                Console.WriteLine("[*] INFO: Performing check module for " + platform);
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

                        Console.WriteLine("[*] INFO: Listing subscriptions user has access to");
                        Console.WriteLine("");

                        // create table header
                        string tableHeader = string.Format("{0,50} | {1,40} | {2,10}", "Name", "Subscription ID", "Status");
                        Console.WriteLine(tableHeader);
                        Console.WriteLine(new String('-', tableHeader.Length));

                        // get a listing of all subscriptions the user has access to in Azure
                        List<Objects.AzureML.Subscription> subscriptionList = await Utilities.AzureML.SubscriptionUtils.getAllSubscriptions(credential, "https://management.azure.com/subscriptions?api-version=2022-12-01");

                        // iterate through the list of subscriptions 
                        foreach (Objects.AzureML.Subscription sub in subscriptionList)
                        {
                            Console.WriteLine("{0,50} | {1,40} | {2,10}", sub.subscriptionDisplayName, sub.subscriptionID, sub.subscriptionState);

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
