using System;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using System.Net;
using Newtonsoft.Json.Linq;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.IO;
using System.Collections.Generic;

namespace L03
{
    class Program
    {
        static UserCredential credential;
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "datc-2020";
        static DriveService service;
        
        static string fileName = "test.txt";
        static string filePath = @"C:\Users\User\Desktop\test.txt";
        static string contentType = "text/plain"; //pentru un fisier text
        static string folderId = "1UfzK7u7TDCtndaRrHcLhtz6w9-159ZbX"; //id director drive
        static void Main(string[] args)
        {
            init();
            GetAllFiles();
            UploadFile(service, fileName, filePath, contentType, folderId);
        }
        static void init()
        {

            using(var stream = new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                string creditPath = "token";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore(creditPath, true)).Result;

                Console.WriteLine("Credential file saved to:" + creditPath);
                
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        static void GetAllFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents"); 
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + credential.Token.AccessToken);
            
            using(var response = request.GetResponse())
            {
                using(Stream data  = response.GetResponseStream())
                using(var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach(var file in myData["files"])
                    {
                        if(file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }

        static void UploadFile(DriveService service, string fileName, string filePath, string contentType, string folderId)
        {
            var fileToDrive = new Google.Apis.Drive.v3.Data.File();
            fileToDrive.Name = fileName;
            fileToDrive.Parents = new List<string> { folderId };
            FilesResource.CreateMediaUpload request;

            using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                request = service.Files.Create(fileToDrive, stream, contentType);
                request.Upload();
            }

            var file = request.ResponseBody;
            Console.WriteLine("File upload: " + file.Id);
        }
    }
}
