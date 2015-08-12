/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Commands;
using Aliyun.OSS.Properties;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    internal static class OssUtils
    {
        public static readonly Uri DefaultEndpoint = new Uri("http://oss.aliyuncs.com");

        private const string DefaultBaseChars = "0123456789ABCDEF";
        private const string CharsetName = "utf-8";

        public const long MaxFileSize = 5 * 1024 * 1024 * 1024L;
        public const int MaxPrefixStringSize = 1024;
        public const int MaxMarkerStringSize = 1024;
        public const int MaxDelimiterStringSize = 1024;
        public const int MaxReturnedKeys = 1000;

        public const int DeleteObjectsUpperLimit = 1000;
        public const int BucketCorsRuleLimit = 10;
        public const int LifecycleRuleLimit = 1000;
        public const int ObjectNameLengthLimit = 1023;
        public const int PartNumberUpperLimit = 10000;

        public static bool IsBucketNameValid(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                return false;

            const string pattern = "^[a-z0-9][a-z0-9\\-]{1,61}[a-z0-9]$";
            var regex = new Regex(pattern);
            var m = regex.Match(bucketName);
            return m.Success;
        }

        public static bool IsObjectKeyValid(string key)
        {
            if (string.IsNullOrEmpty(key) || key.StartsWith("/") || key.StartsWith("\\"))
                return false;

            var byteCount = Encoding.GetEncoding(CharsetName).GetByteCount(key);
            return byteCount <= ObjectNameLengthLimit;
        }

        public static String MakeResourcePath(string key)
        {
            return (key == null) ? string.Empty : UrlEncodeKey(key);
        }

        public static Uri MakeBucketEndpoint(Uri endpoint, string bucket, ClientConfiguration conf)
        {
            return new Uri(endpoint.Scheme + "://"
                           + ((bucket != null && !IsCName(conf, endpoint.Host)) 
                               ? (bucket + "." + endpoint.Host) : endpoint.Host)
                           + ((endpoint.Port != 80) ? (":" + endpoint.Port) : ""));
        }

        private static String UrlEncodeKey(String key)
        {
            const char separator = '/';
            var segments = key.Split(separator);
            
            var encodedKey = new StringBuilder();
            encodedKey.Append(HttpUtils.EncodeUri(segments[0], CharsetName));
            for (var i = 1; i < segments.Length; i++)
                encodedKey.Append(separator).Append(HttpUtils.EncodeUri(segments[i], CharsetName));

            if (key.EndsWith(separator.ToString()))
            {
                // String#split ignores trailing empty strings, e.g., "a/b/" will be split as a 2-entries array,
                // so we have to append all the trailing slash to the uri.
                foreach (var ch in key)
                {
                    if (ch == separator)
                        encodedKey.Append(separator);
                    else
                        break;
                }
            }

            return encodedKey.ToString();
        }

        public static string TrimQuotes(string eTag)
        {
            return eTag != null ? eTag.Trim('\"') : null;
        }

        public static string ComputeContentMd5(Stream input)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(input);
                var charset = DefaultBaseChars.ToCharArray();
                var sBuilder = new StringBuilder();
                foreach (var b in data)
                {
                    sBuilder.Append(charset[b >> 4]);
                    sBuilder.Append(charset[b & 0x0F]);
                }

                return Convert.ToBase64String(data);
            }
        }

        public static bool IsWebpageValid(string webpage)
        {
            const string pageSuffix = ".html";
            return !string.IsNullOrEmpty(webpage) && webpage.EndsWith(pageSuffix)
                   && webpage.Length > pageSuffix.Length;
        }

        public static bool IsLoggingPrefixValid(string loggingPrefix)
        {
            if (string.IsNullOrEmpty(loggingPrefix))
                return true;

            const string pattern = "^[a-zA-Z][a-zA-Z0-9\\-]{0,31}$";
            var regex = new Regex(pattern);
            var m = regex.Match(loggingPrefix);
            return m.Success;
        }

        public static string BuildPartCopySource(string bucketName, string objectKey)
        {
            return "/" + bucketName + "/" + UrlEncodeKey(objectKey);
        }

        public static string BuildCopyObjectSource(string bucketName, string objectKey)
        {
            return "/" + bucketName + "/" + UrlEncodeKey(objectKey);
        }

        private static bool IsCName(ClientConfiguration conf, string host)
        {
            if (string.IsNullOrEmpty(host) || host.Trim().Length == 0)
                throw new ArgumentException("Host name should not be null or empty.");

            var domain = host.ToLower();
            return conf.RootDomainList.All(str => !domain.EndsWith(str));
        }

        public static bool IsPartNumberInRange(int? partNumber)
        {
            return (partNumber.HasValue && partNumber > 0
                && partNumber <= OssUtils.PartNumberUpperLimit);
        }

        public static void CheckBucketName(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!IsBucketNameValid(bucketName))
                throw new ArgumentException(Resources.BucketNameInvalid, "bucketName");
        }

        public static void CheckObjectKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (!IsObjectKeyValid(key))
                throw new ArgumentException(Resources.ObjectKeyInvalid, "key");
        }

        public static string DetermineOsVersion()
        {
            try
            {
                var os = Environment.OSVersion;
                return "windows " + os.Version.Major + "." + os.Version.Minor;
            }
            catch (InvalidOperationException /* ex */)
            {
                return "Unknown OSVersion";
            }
        }

        public static string DetermineSystemArchitecture()
        {
            return Environment.Is64BitProcess ? "x86_64" : "x86";
        }

        public static string JoinETag(IEnumerable<string> etags)
        {
            StringBuilder result = new StringBuilder();

            var first = true;
            foreach (var etag in etags)
            {
                if (!first)
                    result.Append(", ");
                result.Append(etag);
                first = false;
            }

            return result.ToString();
        }

        internal static ClientConfiguration GetClientConfiguration(IServiceClient serviceClient)
        {
            var outerClient = (RetryableServiceClient) serviceClient;
            var innerClient = (ServiceClient)outerClient.InnerServiceClient();
            return innerClient.Configuration;
        }

        public static IAsyncResult BeginOperationHelper<TCommand>(TCommand cmd, AsyncCallback callback, Object state)
            where TCommand : OssCommand
        {
            var retryableAsyncResult = cmd.AsyncExecute(callback, state) as RetryableAsyncResult;
            if (retryableAsyncResult == null)
            {
                throw new ArgumentException("retryableAsyncResult should not be null");
            }
            return retryableAsyncResult.InnerAsyncResult;
        }

        public static TResult EndOperationHelper<TResult>(IServiceClient serviceClient, IAsyncResult asyncResult)
        {
            var response = EndOperationHelper(serviceClient, asyncResult);
            var retryableAsyncResult = asyncResult as RetryableAsyncResult;
            Debug.Assert(retryableAsyncResult != null);
            OssCommand<TResult> cmd = (OssCommand<TResult>)retryableAsyncResult.Context.Command;
            return cmd.DeserializeResponse(response);
        }

        private static ServiceResponse EndOperationHelper(IServiceClient serviceClient, IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            var retryableAsyncResult = asyncResult as RetryableAsyncResult;
            if (retryableAsyncResult == null)
                throw new ArgumentException("retryableAsyncResult should not be null");

            ServiceClientImpl.HttpAsyncResult httpAsyncResult =
                retryableAsyncResult.InnerAsyncResult as ServiceClientImpl.HttpAsyncResult;
            return serviceClient.EndSend(httpAsyncResult);
        }

        public static void CheckCredentials(string accessId, string accessKey)
        {
            if (string.IsNullOrEmpty(accessId))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
            if (string.IsNullOrEmpty(accessKey))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");
        }
    }
}
