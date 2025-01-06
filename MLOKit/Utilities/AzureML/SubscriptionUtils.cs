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
    class SubscriptionUtils
    {

        // get listing of all subscriptions
        public static async Task<List<Objects.AzureML.Subscription>> getAllSubscriptions(string credentials, string url)
        {
            List<Objects.AzureML.Subscription> subscriptionList = new List<Objects.AzureML.Subscription>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // web request to get list of subscriptions
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
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

                    string subscriptionID = "";
                    string subscriptionDisplayName = "";
                    string subscriptionState = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":

                                // if subscription already doesn't exist in our list, add it
                                if (!doesSubscriptionAlreadyExistInList(subscriptionID, subscriptionList) && subscriptionID != "" && subscriptionDisplayName != "" && subscriptionState != "")
                                {
                                    subscriptionList.Add(new Objects.AzureML.Subscription(subscriptionID, subscriptionDisplayName, subscriptionState));
                                    subscriptionDisplayName = "";
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

                                if (propName.ToLower().Equals("subscriptionid"))
                                {
                                    subscriptionID = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("displayname"))
                                {
                                    subscriptionDisplayName = jsonResult.Value.ToString();
                                }
                                if (propName.ToLower().Equals("state"))
                                {
                                    subscriptionState = jsonResult.Value.ToString();
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


            return subscriptionList;

        }

        // determine whether we already have a subscription in our list by the given unique ID for that subscription
        public static bool doesSubscriptionAlreadyExistInList(string subscriptionID, List<Objects.AzureML.Subscription> subscriptionList)
        {
            bool doesItExist = false;

            foreach (Objects.AzureML.Subscription sub in subscriptionList)
            {
                if (sub.subscriptionID.Equals(subscriptionID))
                {
                    doesItExist = true;
                }
            }

            return doesItExist;
        }


    }
}
