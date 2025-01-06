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
    class BucketUtils
    {


        // get recursive listing of files in a bucket
        public static async Task<List<string>> recursiveListBucket(string credentials, string bucket)
        {
            List<string> fileListing = new List<string>();


            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // web request to get recursive listing of file in bucket
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://storage.googleapis.com/storage/v1/b/" + bucket + "/o?alt=json&fields=prefixes%2Citems%2Fname%2Citems%2Fsize%2Citems%2Fgeneration%2CnextPageToken&maxResults=1000&projection=noAcl");
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

                    string filePath = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":
                                if (filePath != "")
                                {
                                    fileListing.Add(filePath);
                                    filePath = "";
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

                                if (propName.ToLower().Equals("name"))
                                {
                                    filePath = jsonResult.Value.ToString();
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

            }


            return fileListing;

        }


        // get recursive listing of files in a folder within a bucket
        public static async Task<List<string>> recursiveListFolder(string credentials, string bucket, string folderPath)
        {
            List<string> fileListing = new List<string>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // web request to get recursive listing of folder in bucket
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://storage.googleapis.com/storage/v1/b/" + bucket + "/o?alt=json&prefix=" + folderPath.Substring(1) + "&fields=prefixes%2Citems%2Fname%2Citems%2Fsize%2Citems%2Fgeneration%2CnextPageToken&maxResults=1000&projection=noAcl");
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

                    string filePath = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":
                                if (filePath != "")
                                {
                                    fileListing.Add(filePath);
                                    filePath = "";
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

                                if (propName.ToLower().Equals("name"))
                                {
                                    filePath = jsonResult.Value.ToString();
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

            }


            return fileListing;

        }



        // get listing of buckets for a given project
        public static async Task<List<string>> getBuckets(string credentials, string project)
        {
            List<string> bucketListing = new List<string>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // web request to get listing of buckets for a given project
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://storage.googleapis.com/storage/v1/b?alt=json&fields=items%2Fname%2CnextPageToken&maxResults=1000&project=" + project +"&projection=noAcl");
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

                    string bucketName = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":
                                if (bucketName != "")
                                {
                                    bucketListing.Add(bucketName);
                                    bucketName = "";
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

                                if (propName.ToLower().Equals("name"))
                                {
                                    bucketName = jsonResult.Value.ToString();
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

            }

            return bucketListing;

        }



        // get the mediaLinks for a specific file so that it can be downloaded
        public static async Task<List<string>> getMediaLinks(string credentials, string bucket, string filePath)
        {
            List<string> mediaLinkList = new List<string>();


            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string folderPath = "";
                string fileName = "";

                string[] splitFilePath = filePath.Split('/');
                fileName = splitFilePath[splitFilePath.Length - 1];
                folderPath = filePath.Replace(fileName, "");


                // web request to get the mediaLink for a given file
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://storage.googleapis.com/storage/v1/b/" + bucket + "/o?alt=json&prefix=" + folderPath + "&item=" + fileName);
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

                    string mediaLink = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":
                                if (mediaLink != "")
                                {
                                    mediaLinkList.Add(mediaLink);
                                    mediaLink = "";
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

                                if (propName.ToLower().Equals("medialink"))
                                {
                                    mediaLink = jsonResult.Value.ToString();
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

            }


            return mediaLinkList;

        }



        // get the mediaLinks for a specific folder so that all files can be downloaded
        public static async Task<List<string>> getMediaLinksByFolder(string credentials, string bucket, string folderPath)
        {
            List<string> mediaLinkList = new List<string>();

            try
            {

                // ignore SSL errors
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // web request to get the mediaLink for a given file
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://storage.googleapis.com/storage/v1/b/" + bucket + "/o?alt=json&prefix=" + folderPath);
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

                    string mediaLink = "";
                    string propName = "";

                    // read the json results
                    while (jsonResult.Read())
                    {

                        switch (jsonResult.TokenType.ToString())
                        {
                            case "StartObject":
                                break;
                            case "EndObject":
                                if (mediaLink != "")
                                {
                                    mediaLinkList.Add(mediaLink);
                                    mediaLink = "";
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

                                if (propName.ToLower().Equals("medialink"))
                                {
                                    mediaLink = jsonResult.Value.ToString();
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

            }

            return mediaLinkList;

        }

    }
}
