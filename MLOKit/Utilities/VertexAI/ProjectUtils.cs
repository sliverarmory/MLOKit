using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MLOKit.Utilities.VertexAI
{
    class ProjectUtils
    {

        // get a list of all projects
        public static async Task<List<Objects.VertexAI.Project>> getAllProjects(string credentials, string url)
        {
            List<Objects.VertexAI.Project> projectList = new List<Objects.VertexAI.Project>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // web request to get list of projects
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(url + "/v1/projects?alt=json&filter=lifecycleState%3AACTIVE&pageSize=500");
                if (webRequest != null)
                {

                    // set header values
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    webRequest.UserAgent = "MLOKit-e977ac02118a3cb2c584d92a324e41e9";
                    webRequest.Headers.Add("Authorization", "Bearer " + credentials);


                    // get web response and status code
                    HttpWebResponse myWebResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
                    string content;
                    var reader = new StreamReader(myWebResponse.GetResponseStream());
                    content = reader.ReadToEnd();


                    // parse the JSON output and display results
                    JsonTextReader jsonResult = new JsonTextReader(new StringReader(content));

                    string projectName = "";
                    string projectNumber = "";
                    string projectState = "";
                    string projectCreationDate = "";
                    string projectID = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":

                                // if project already doesn't exist in our list, add it
                                if (!doesProjectAlreadyExistInList(projectID, projectList) && projectID != "" && projectName != "" && projectState != "" && projectCreationDate != "" && projectNumber != "")
                                {
                                    projectList.Add(new Objects.VertexAI.Project(projectName, projectNumber, projectState, projectCreationDate, projectID));
                                    projectName = "";
                                }
                                break;
                            case "StartArray":
                                break;
                            case "EndArray":
                                break;
                            case "PropertyName":
                                propName = jsonResult.Value.ToString();
                                break;
                            case "String":

                                if (propName.ToLower().Equals("projectnumber"))
                                {
                                    projectNumber = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("name"))
                                {
                                    projectName = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("projectid"))
                                {
                                    projectID = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("lifecyclestate"))
                                {
                                    projectState = jsonResult.Value.ToString();
                                }
                                break;
                            case "Date":
                                if (propName.ToLower().Equals("createtime"))
                                {
                                    projectCreationDate = jsonResult.Value.ToString();
                                }
                                break;
                            case "Boolean":
                                break;
                            default:
                                break;

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("[-] ERROR: " + ex.Message);
                Console.WriteLine("");
            }


            return projectList;

        }


        // determine whether we already have a project in our list by the given unique ID for that project
        public static bool doesProjectAlreadyExistInList(string projectID, List<Objects.VertexAI.Project> projectList)
        {
            bool doesItExist = false;

            foreach (Objects.VertexAI.Project proj in projectList)
            {
                if (proj.projectID.Equals(projectID))
                {
                    doesItExist = true;
                }
            }

            return doesItExist;
        }

    }

}
