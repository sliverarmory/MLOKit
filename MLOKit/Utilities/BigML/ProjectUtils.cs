using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MLOKit.Utilities.BigML
{
    class ProjectUtils
    {


        // get a list of all projects
        public static async Task<List<Objects.BigML.Project>> getAllProjects(string credentials, string url)
        {
            List<Objects.BigML.Project> projectList = new List<Objects.BigML.Project>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string[] splitCreds = credentials.Split(';');

                // web request to get list of projects
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(url + "/project?username=" + splitCreds[0] + "&api_key=" + splitCreds[1]);
                if (webRequest != null)
                {

                    // set header values
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    webRequest.UserAgent = "MLOKit-e977ac02118a3cb2c584d92a324e41e9";


                    // get web response and status code
                    HttpWebResponse myWebResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
                    string content;
                    var reader = new StreamReader(myWebResponse.GetResponseStream());
                    content = reader.ReadToEnd();


                    // parse the JSON output and display results
                    JsonTextReader jsonResult = new JsonTextReader(new StringReader(content));

                    string projectName = "";
                    string projectCreator = "";
                    string visibility = "";
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
                                if (!doesProjectAlreadyExistInList(projectID, projectList) && projectID != "")
                                {
                                    projectList.Add(new Objects.BigML.Project(projectName, projectCreator, visibility, projectCreationDate, projectID));
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

                                if (propName.ToLower().Equals("creator"))
                                {
                                    projectCreator = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("name"))
                                {
                                    projectName = jsonResult.Value.ToString();
                                }                           
                                if (propName.ToLower().Equals("resource"))
                                {
                                    projectID = jsonResult.Value.ToString();
                                    projectID = projectID.Split('/')[1];
                                }
                                break;
                            case "Date":
                                if (propName.ToLower().Equals("created"))
                                {
                                    projectCreationDate = jsonResult.Value.ToString();
                                }
                                break;
                            case "Boolean":
                                if (propName.ToLower().Equals("private"))
                                {
                                    visibility = jsonResult.Value.ToString();

                                    if (visibility.ToLower().Equals("true"))
                                    {
                                        visibility = "Private";
                                    }
                                    else
                                    {
                                        visibility = "Public";
                                    }
                                }
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
        public static bool doesProjectAlreadyExistInList(string projectID, List<Objects.BigML.Project> projectList)
        {
            bool doesItExist = false;

            foreach (Objects.BigML.Project proj in projectList)
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
