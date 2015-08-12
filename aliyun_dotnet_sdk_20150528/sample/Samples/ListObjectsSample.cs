﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for listing objects.
    /// </summary>
    public static class ListObjectsSample
    {
        const string accessId = "<your access id>";
        const string accessKey = "<your access key>";
        const string endpoint = "<valid host name>";

        static OssClient ossClient = new OssClient(endpoint, accessId, accessKey);

        const string bucketName = "<your bucket name>";

        static AutoResetEvent evnet = new AutoResetEvent(false);

        public static void ListObjects()
        {
            try
            {
                var keys = new List<string>();
                ObjectListing result = null; 
                string nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(bucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100
                    };
                    result = ossClient.ListObjects(listObjectsRequest);
                    
                    foreach (var summary in result.ObjectSummaries)
                    {
                        Console.WriteLine(summary.Key);
                        keys.Add(summary.Key);
                    }

                    nextMarker = result.NextMarker;
                } while (result.IsTruncated);

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

        public static void AsyncListObjects()
        {
            try
            {
                var listObjectsRequest = new ListObjectsRequest(bucketName);
                ossClient.BeginListObjects(listObjectsRequest, ListObjectCallback, null);

                evnet.WaitOne();
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

        private static void ListObjectCallback(IAsyncResult ar)
        {
            var result = ossClient.EndListObjects(ar);
            foreach (var summary in result.ObjectSummaries)
            {
                Console.WriteLine(summary.Key);
            }

            evnet.Set();
        }
    }
}
