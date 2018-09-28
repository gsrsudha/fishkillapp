using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using FishKillCommonLibrary;
using System.Collections.Generic;
using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Prediction.Models;
using System.Linq;

namespace FishKillFunctionApp
{
    public static class FishKillValidator
    {
        static CloudBlobContainer _blobContainer;
        [FunctionName("ValidateFishKill")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                if (req.Method == HttpMethod.Get)
                {
                    log.Info("C# HTTP trigger function processed a request.");

                    // parse query parameter
                    string name = req.GetQueryNameValuePairs()
                        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                        .Value;

                    // Get request body
                    dynamic data = await req.Content.ReadAsAsync<object>();

                    // Set name to query string or body data
                    name = name ?? data?.name;

                    return name == null
                        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                        : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
                }

                else if (req.Method == HttpMethod.Post)
                {
                    var stream = await req.Content.ReadAsStreamAsync();
                    var prediction = new Prediction
                    {
                        ProjectId = "7dad5391-6154-4d00-81ed-be1f56964d89", //This is the custom vision project we are predicting against
                        PredictionKey = "ca5362dcc67c45748973839961f84b8a", //This is the custom vision project's prediction key we are predicting against
                        TimeStamp = DateTime.UtcNow,
                        UserId = Guid.NewGuid().ToString(),
                        ImageUrl = await UploadImageToBlobStorage(stream),
                        Results = new Dictionary<string, decimal>()
                    };

                    var endpoint = new PredictionEndpoint { ApiKey = prediction.PredictionKey };
                    //This is where we run our prediction against the default iteration
                    var result = endpoint.PredictImageUrl(new Guid(prediction.ProjectId), new ImageUrl(prediction.ImageUrl));

                    // Loop over each prediction and write out the results
                    foreach (var outcome in result.Predictions)
                    {
                        if (outcome.Probability > .70)
                            prediction.Results.Add(outcome.Tag, (decimal)outcome.Probability);
                    }

                    await CosmosDataService.Instance.InsertItemAsync(prediction);
                    return req.CreateResponse(HttpStatusCode.OK, prediction);
                }
                else {
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid request..");
                }
            }
            catch (Exception e)
            {
                //Catch and unwind any exceptions that might be thrown and return the reason (non-production)
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, e.GetBaseException().Message);
            }

            async Task<string> UploadImageToBlobStorage(Stream stream)
            {
                //Create a new blob block Id
                var blobId = Guid.NewGuid().ToString() + ".jpg";

                if (_blobContainer == null)
                {
                    //You can set your endpoint here as a string in code (ideally this should go in your Function's App Settings)
                    var containerName = "images";                    
                    var endpoint = $"https://fishkillappacctblob.blob.core.windows.net/{containerName}?sv=2017-11-09&ss=b&srt=sco&sp=rwdlac&se=2018-09-29T00:41:11Z&st=2018-09-28T16:41:11Z&spr=https,http&sig=jwlc9vDFgxzmFqDRUqFRKTEEyBobmct0Yed0Wj7zHyA%3D";
                    _blobContainer = new CloudBlobContainer(new Uri(endpoint));
                }

                //Create a new block to store this uploaded image data
                var blockBlob = _blobContainer.GetBlockBlobReference(blobId);
                blockBlob.Properties.ContentType = "image/jpg";

                //You can even store metadata with the content
                blockBlob.Metadata.Add("createdFor", "This Custom Vision Hackathon");

                //Upload and return the new URL associated w/ this blob content
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob.StorageUri.PrimaryUri.ToString();
            }
        }

    }
}
