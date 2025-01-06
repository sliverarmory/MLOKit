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
    class DatasetUtils
    {

        // get listing of all datasets
        public static async Task<List<Objects.AzureML.Dataset>> getAllDatasets(string credentials, string subscriptionID, string region, string resourceGroup, string workspace)
        {
            List<Objects.AzureML.Dataset> datasetList = new List<Objects.AzureML.Dataset>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // web request to get list of datasets
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://" + region + ".experiments.azureml.net/dataset/v1.0/subscriptions/" + subscriptionID + "/resourceGroups/" + resourceGroup + "/providers/Microsoft.MachineLearningServices/workspaces/" + workspace + "/datasets?includeInvisible=false&pageSize=100&includeLatestDefinition=true");
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

                    string datasetID = "";
                    string datasetState = "";
                    string datasetFilePath = "";
                    string datasetFileType = "";
                    string dataStoreName = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":

                                // if dataset already doesn't exist in our list, add it
                                if (!doesDatasetAlreadyExistInList(datasetID, datasetList) && datasetID != "" && datasetState != "" && datasetFilePath != "" && datasetFileType != "" && dataStoreName != "")
                                {
                                    datasetList.Add(new Objects.AzureML.Dataset(datasetID, datasetState, datasetFilePath, datasetFileType, dataStoreName));
                                    datasetID = "";
                                    datasetState = "";
                                    datasetFilePath = "";
                                    datasetFileType = "";
                                    dataStoreName = "";

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
                                if (propName.ToLower().Equals("datasetid"))
                                {
                                    datasetID = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("state"))
                                {
                                    datasetState = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("azurefilepath"))
                                {
                                    datasetFilePath = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("filetype"))
                                {
                                    datasetFileType = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("datastorename"))
                                {
                                    dataStoreName = jsonResult.Value.ToString();
                                }

                                break;
                            case "Date":
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


            return datasetList;

        }


        // get single dataset by dataset ID
        public static async Task<Objects.AzureML.Dataset> getSingleDataset(string credentials, string subscriptionID, string region, string resourceGroup, string workspace, string datID)
        {
            Objects.AzureML.Dataset dataset = null;

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // web request to get a dataset
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://" + region + ".experiments.azureml.net/dataset/v1.0/subscriptions/" + subscriptionID + "/resourceGroups/" + resourceGroup + "/providers/Microsoft.MachineLearningServices/workspaces/" + workspace + "/datasets/" + datID + "?includeInvisible=false&pageSize=100&includeLatestDefinition=true");
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

                    string datasetID = "";
                    string datasetState = "";
                    string datasetFilePath = "";
                    string datasetFileType = "";
                    string dataStoreName = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":

                                // grab the dataset to return
                                if (datasetID != "" && datasetState != "" && datasetFilePath != "" && datasetFileType != "" && dataStoreName != "")
                                {
                                    dataset = new Objects.AzureML.Dataset(datasetID, datasetState, datasetFilePath, datasetFileType, dataStoreName);

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
                                if (propName.ToLower().Equals("datasetid"))
                                {
                                    datasetID = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("state"))
                                {
                                    datasetState = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("azurefilepath"))
                                {
                                    datasetFilePath = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("filetype"))
                                {
                                    datasetFileType = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("datastorename"))
                                {
                                    dataStoreName = jsonResult.Value.ToString();
                                }

                                break;
                            case "Date":
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


            return dataset;

        }



        // determine whether we already have a dataset in our list by the given unique ID for that dataset
        public static bool doesDatasetAlreadyExistInList(string datasetID, List<Objects.AzureML.Dataset> datasetList)
        {
            bool doesItExist = false;

            foreach (Objects.AzureML.Dataset datSet in datasetList)
            {
                if (datSet.datasetID.Equals(datasetID))
                {
                    doesItExist = true;
                }
            }

            return doesItExist;
        }


    }
}
