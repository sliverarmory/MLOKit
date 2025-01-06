using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.AzureML
{
    class ListProjects
    {

        public static async Task execute(string credential, string platform, string subscriptionID)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("list-projects", credential, platform));

            // check for additional required arguments
            if (subscriptionID.Equals(""))
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
                Console.WriteLine("[*] INFO: Performing list-projects module for " + platform);
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
                        string tableHeader = string.Format("{0,30} | {1,40} | {2,10} | {3,30} | {4,25}", "Name", "Workspace ID", "Region", "Resource Group", "Creation Time");
                        Console.WriteLine(tableHeader);
                        Console.WriteLine(new String('-', tableHeader.Length));

                        // get a listing of all workspaces the user has access to in Azure
                        List<Objects.AzureML.Workspace> workspaceList = await Utilities.AzureML.WorkspaceUtils.getAllWorkspaces(credential, subscriptionID);

                        // iterate through the list of workspaces 
                        foreach (Objects.AzureML.Workspace workspace in workspaceList)
                        {
                            Console.WriteLine("{0,30} | {1,40} | {2,10} | {3,30} | {4,25}", workspace.workspaceName, workspace.workspaceID, workspace.workspaceRegion, workspace.resourceGroup, workspace.creationTime);

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
