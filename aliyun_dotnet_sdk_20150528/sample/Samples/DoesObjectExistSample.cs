﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for determining whether the specified object exists in the specified bucket.
    /// </summary>
    public static class DoesObjectExistSample
    {
        public static void DoesObjectExist(string bucketName, string key)
        {
            const string accessId = "<your access id>";
            const string accessKey = "<your access key>";
            const string endpoint = "<valid host name>";

            var client = new OssClient(endpoint, accessId, accessKey);

            try
            {
                var exist = client.DoesObjectExist(bucketName, key);
                Console.WriteLine("exist ? " + exist);
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
    }
}
