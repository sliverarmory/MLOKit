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
    class ModelUtils
    {


        // get a list of all models
        public static async Task<List<Objects.BigML.Model>> getAllModels(string credentials, string url)
        {
            List<Objects.BigML.Model> modelList = new List<Objects.BigML.Model>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string[] splitCreds = credentials.Split(';');

                // web request to get list of models
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(url + "/model?username=" + splitCreds[0] + "&api_key=" + splitCreds[1]);
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

                    string modelName = "";
                    string modelID = "";
                    string createdBy = "";
                    string associatedProject = "";
                    string visibility = "";
                    string dateCreated = "";
                    string dateUpdated = "";
                    string associatedDataset = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":

                                // if model already doesn't exist in our list, add it
                                if (!doesModelAlreadyExistInList(modelID, modelList) && modelID != "" && modelName != "" && associatedDataset != "" && associatedProject != "" && visibility != "" && dateCreated != "" && dateUpdated != "" && visibility != "")
                                {
                                    modelList.Add(new Objects.BigML.Model(modelName, modelID, createdBy, dateCreated, dateUpdated, associatedProject, associatedDataset, visibility));
                                    modelName = "";
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

                                if (propName.ToLower().Equals("dataset"))
                                {
                                    associatedDataset = jsonResult.Value.ToString();
                                    associatedDataset = associatedDataset.Split('/')[1];
                                }
                                if (propName.ToLower().Equals("project"))
                                {
                                    associatedProject = jsonResult.Value.ToString();
                                    associatedProject = associatedProject.Split('/')[1];
                                }
                                if (propName.ToLower().Equals("creator"))
                                {
                                    createdBy = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("name") && modelName == "")
                                {
                                    modelName = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("resource"))
                                {
                                    modelID = jsonResult.Value.ToString();
                                    modelID = modelID.Split('/')[1];
                                }
                                break;
                            case "Date":
                                if (propName.ToLower().Equals("created"))
                                {
                                    dateCreated = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("updated"))
                                {
                                    dateUpdated = jsonResult.Value.ToString();
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


            return modelList;

        }



        // download a model
        public static async Task<string> downloadModel(string credentials, string modelID)
        {

            string pmmlXML = "";

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string[] splitCreds = credentials.Split(';');

                // web request to download model
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://bigml.io/model/" + modelID + "?username=" + splitCreds[0] + "&api_key=" + splitCreds[1] + "&pmml=yes");
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

                    
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":
                                break;
                            case "StartArray":
                                break;
                            case "EndArray":
                                break;
                            case "PropertyName":
                                propName = jsonResult.Value.ToString();
                                break;
                            case "String":

                                if (propName.ToLower().Equals("pmml"))
                                {
                                    pmmlXML = jsonResult.Value.ToString();
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


            return pmmlXML;

        }


        // determine whether we already have a model in our list by the given unique ID for that model
        public static bool doesModelAlreadyExistInList(string modelID, List<Objects.BigML.Model> modelList)
        {
            bool doesItExist = false;

            foreach (Objects.BigML.Model model in modelList)
            {
                if (model.modelID.Equals(modelID))
                {
                    doesItExist = true;
                }
            }

            return doesItExist;
        }



    }
}
