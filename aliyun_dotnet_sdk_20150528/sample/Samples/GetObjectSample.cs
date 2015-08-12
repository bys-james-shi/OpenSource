/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for getting object.
    /// </summary>
   public static class GetObjectSample
   {
       const string accessId = "<your access id>";
       const string accessKey = "<your access key>";
       const string endpoint = "<valid endpoint>";

       static OssClient ossClient = new OssClient(new Uri(endpoint), accessId, accessKey);

       const string bucketName = "<bucket name>";
       const string key = "<object key>";
       const string fileToUpload = "<path/to/upload>";
       const string fileToDownload = "<path/to/download>";

       static AutoResetEvent _evnet = new AutoResetEvent(false);

       public static void GetObject()
       {
           try
           {
                string eTag;
                using (Stream fs = File.Open(fileToUpload, FileMode.Open))
                {
                    // compute content's md5
                    eTag = ComputeContentMd5(fs);
                }

                // put object
                var metadata = new ObjectMetadata {ETag = eTag};
               ossClient.PutObject(bucketName, key, fileToUpload, metadata);

               Console.WriteLine("PutObject done.");

                // verify etag
                var o = ossClient.GetObject(bucketName, key);
                using (var requestStream = o.Content)
                {
                    int len = (int) o.Metadata.ContentLength;
                    var buf = new byte[len];
                    requestStream.Read(buf, 0, len);
                    var fs = File.Open(fileToDownload, FileMode.OpenOrCreate);
                    fs.Write(buf, 0, len);
                    //eTag = ComputeContentMd5(requestStream);
                    //Assert.AreEqual(o.Metadata.ETag, eTag.ToUpper());
                }

               Console.WriteLine("GetObject done.");
           }
           catch (OssException ex)
           {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", 
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
           }
           catch (Exception ex)
           {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
           }
        }

       public static void AsyncGetObject()
       {
           try
           {
               ossClient.PutObject(bucketName, key, fileToUpload, null);
               Console.WriteLine("PutObject done.");

               ossClient.BeginGetObject(bucketName, key, GetObjectCallback, "GetObject");
               _evnet.WaitOne();
           }
           catch (OssException ex)
           {
               Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                   ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
           }
           catch (Exception ex)
           {
               Console.WriteLine("Failed with error info: {0}", ex.Message);
           }
       }

       private static void GetObjectCallback(IAsyncResult ar)
       {
           try
           {
               var result = ossClient.EndGetObject(ar);
               byte[] buffer = new byte[4096];
               var reponseBody = result.Content;
               using (reponseBody)
               {
                   while (reponseBody.Read(buffer, 0, 4096) != 0) 
                       ;
               }
               Console.WriteLine("ETag: " + result.Metadata.ETag);
               Console.WriteLine("AsyncState:" + ar.AsyncState as string);
           }
           catch (Exception ex)
           {
               OssException ae = ex as OssException;
               Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                   ae.ErrorCode, ae.Message, ae.RequestId, ae.HostId);
           }
           finally
           {
               _evnet.Set();
           }
       }

        private static string ComputeContentMd5(Stream inputStream)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(inputStream);
                var sBuilder = new StringBuilder();
                foreach (var t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
