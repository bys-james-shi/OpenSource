/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    internal static class SignUtils
    {
        private class KeyComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return String.Compare(x, y, StringComparison.Ordinal);
            }
        }

        private const string NewLineMarker = "\n";

        private static readonly IList<string> ParamtersToSign = new List<string> {
            "acl", "uploadId", "partNumber", "uploads", "cors", "logging", 
            "website", "delete", "referer", "lifecycle", "security-token",
            ResponseHeaderOverrides.ResponseCacheControl,
            ResponseHeaderOverrides.ResponseContentDisposition,
            ResponseHeaderOverrides.ResponseContentEncoding,
            ResponseHeaderOverrides.ResponseHeaderContentLanguage,
            ResponseHeaderOverrides.ResponseHeaderContentType,
            ResponseHeaderOverrides.ResponseHeaderExpires
        };

        public static string BuildCanonicalString(string method, string resourcePath,
                           ServiceRequest request)
        {
            var canonicalString = new StringBuilder();
            
            canonicalString.Append(method).Append(NewLineMarker);

            var headers = request.Headers;
            IDictionary<string, string> headersToSign = new Dictionary<string, string>();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    var lowerKey = header.Key.ToLowerInvariant();

                    if (lowerKey == HttpHeaders.ContentType.ToLowerInvariant()
                        || lowerKey == HttpHeaders.ContentMd5.ToLowerInvariant()
                        || lowerKey == HttpHeaders.Date.ToLowerInvariant()
                        || lowerKey.StartsWith(OssHeaders.OssPrefix))
                    {
                        headersToSign.Add(lowerKey, header.Value);
                    }
                }
            }

            if (!headersToSign.ContainsKey(HttpHeaders.ContentType.ToLowerInvariant()))
                headersToSign.Add(HttpHeaders.ContentType.ToLowerInvariant(), "");
            if (!headersToSign.ContainsKey(HttpHeaders.ContentMd5.ToLowerInvariant()))
                headersToSign.Add(HttpHeaders.ContentMd5.ToLowerInvariant(), "");

            // Add params that start with "x-oss-"
            if (request.Parameters != null)
            {
                foreach (var p in request.Parameters)
                {
                    if (p.Key.StartsWith(OssHeaders.OssPrefix))
                        headersToSign.Add(p.Key, p.Value);
                }
            }

            // Add all headers to sign into canonical string, 
            // note that these headers should be ordered before adding.
            foreach (var entry in headersToSign.OrderBy(e => e.Key, new KeyComparer()))
            {
                var key = entry.Key;
                Object value = entry.Value;

                if (key.StartsWith(OssHeaders.OssPrefix))
                    canonicalString.Append(key).Append(':').Append(value);
                else
                    canonicalString.Append(value);

                canonicalString.Append(NewLineMarker);
            }

            // Add canonical resource
            canonicalString.Append(BuildCanonicalizedResource(resourcePath, request.Parameters));

            return canonicalString.ToString();
        }

        private static string BuildCanonicalizedResource(string resourcePath,
                                                         IDictionary<string, string> parameters)
        {
            var canonicalizedResource = new StringBuilder();
            
            canonicalizedResource.Append(resourcePath);

            if (parameters != null)
            {
                var parameterNames = parameters.Keys.OrderBy(e => e);
                var separator = '?';
                foreach (var paramName in parameterNames)
                {
                    if (!ParamtersToSign.Contains(paramName))
                        continue;

                    canonicalizedResource.Append(separator);
                    canonicalizedResource.Append(paramName);
                    var paramValue = parameters[paramName];
                    if (paramValue != null)
                        canonicalizedResource.Append("=").Append(paramValue);

                    separator = '&';
                }
            }

            return canonicalizedResource.ToString();
        }
    }
}
