using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MLOKit.Utilities.AzureML
{
    class WorkspaceUtils
    {


        // get listing of all workspaces
        public static async Task<List<Objects.AzureML.Workspace>> getAllWorkspaces(string credentials, string subscriptionID)
        {
            List<Objects.AzureML.Workspace> workspaceList = new List<Objects.AzureML.Workspace>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // web request to get list of subscriptions
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://management.azure.com/subscriptions/" + subscriptionID + "/providers/Microsoft.MachineLearningServices/workspaces?api-version=2023-10-01");
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

                    string workspaceName = "";
                    string workspaceID = "";
                    string workspaceRegion = "";
                    string creationTime = "";
                    string createdBy = "";
                    string resourceGroup = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":

                                // if workspace already doesn't exist in our list, add it
                                if (!doesWorkspaceAlreadyExistInList(workspaceID, workspaceList) && workspaceID != "" && workspaceName != "" && workspaceRegion != "" && creationTime != "" && createdBy != "" && resourceGroup != "")
                                {
                                    workspaceList.Add(new Objects.AzureML.Workspace(workspaceName, workspaceID, workspaceRegion, creationTime, createdBy, resourceGroup));
                                    workspaceName = "";
                                    workspaceID = "";
                                    workspaceRegion = "";
                                    creationTime = "";
                                    createdBy = "";
                                    resourceGroup = "";
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
                                if (propName.ToLower().Equals("workspaceid"))
                                {
                                    workspaceID = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("createdby"))
                                {
                                    createdBy = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("mlflowtrackinguri"))
                                {
                                    // url is formatted like this in json - azureml://eastus.api.azureml.ms/mlflow/v1.0/
                                    workspaceRegion = jsonResult.Value.ToString().Split('/')[2];
                                    workspaceRegion = workspaceRegion.Replace(".api.azureml.ms", "");
                                }
                                if (propName.ToLower().Equals("id"))
                                {
                                    // url is formatted like this - /subscriptions/[SUB_ID]/resourceGroups/[RESOURCE_GROUP_NAME]/providers/Microsoft.MachineLearningServices/workspaces/[WORKSPACE_NAME]
                                    resourceGroup = jsonResult.Value.ToString().Split('/')[4];
                                    workspaceName = jsonResult.Value.ToString().Split('/')[8];
                                }
                                break;
                            case "Date":
                                if (propName.ToLower().Equals("creationtime"))
                                {
                                    creationTime = jsonResult.Value.ToString();
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


            return workspaceList;

        }

        // determine whether we already have a workspace in our list by the given unique ID for that workspace
        public static bool doesWorkspaceAlreadyExistInList(string workspaceID, List<Objects.AzureML.Workspace> workspaceList)
        {
            bool doesItExist = false;

            foreach (Objects.AzureML.Workspace workspace in workspaceList)
            {
                if (workspace.workspaceID.Equals(workspaceID))
                {
                    doesItExist = true;
                }
            }

            return doesItExist;
        }


    }
}
