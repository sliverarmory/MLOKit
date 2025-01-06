using System;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace MLOKit.Modules.VertexAI
{
    class ListProjects
    {

        public static async Task execute(string credential, string platform)
        {
            // Generate module header
            Console.WriteLine(Utilities.ArgUtils.GenerateHeader("list-projects", credential, platform));


            // ignore SSL errors
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.WriteLine("");
            Console.WriteLine("[*] INFO: Performing list-projects module for " + platform);
            Console.WriteLine("");


            // check if credentials provided are valid
            Console.WriteLine("[*] INFO: Checking credentials provided");
            Console.WriteLine("");

            // if creds valid, then provide message and continue
            if (await Utilities.VertexAI.WebUtils.credsValid(credential, "https://cloudresourcemanager.googleapis.com/v1/projects?alt=json&filter=lifecycleState%3AACTIVE&pageSize=500"))
            {
                Console.WriteLine("[+] SUCCESS: Credentials provided are VALID.");
                Console.WriteLine("");

                try
                {

                    // create table header
                    string tableHeader = string.Format("{0,30} | {1,30} | {2,10} | {3,25}", "Name", "Project ID", "Status", "Creation Date");
                    Console.WriteLine(tableHeader);
                    Console.WriteLine(new String('-', tableHeader.Length));

                    // get a listing of all projects the user has access to in Vertex AI
                    List<Objects.VertexAI.Project> projectList = await Utilities.VertexAI.ProjectUtils.getAllProjects(credential, "https://cloudresourcemanager.googleapis.com");

                    // iterate through the list of projects 
                    foreach (Objects.VertexAI.Project proj in projectList)
                    {
                        Console.WriteLine("{0,30} | {1,30} | {2,10} | {3,25}", proj.projectName, proj.projectID,  proj.projectState, proj.projectCreationDate);

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
