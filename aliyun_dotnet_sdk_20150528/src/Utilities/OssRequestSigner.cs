/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    internal class OssRequestSigner : IRequestSigner
    {
        private readonly string _resourcePath;

        public OssRequestSigner(String resourcePath)
        {
            _resourcePath = resourcePath;
        }
        
        public void Sign(ServiceRequest request, ICredentials credentials)
        {
            var accessKeyId = credentials.AccessId;
            var secretAccessKey = credentials.AccessKey;
            var httpMethod = request.Method.ToString().ToUpperInvariant();
            // Because the resource path to is different from the one in the request uri,
            // can't use ServiceRequest.ResourcePath here.
            var resourcePath = _resourcePath;

            if (!string.IsNullOrEmpty(secretAccessKey))
            {
                var canonicalString = SignUtils.BuildCanonicalString(httpMethod, resourcePath, request);
                var signature = ServiceSignature.Create().ComputeSignature(secretAccessKey, canonicalString);
                request.Headers.Add(HttpHeaders.Authorization, "OSS " + accessKeyId + ":" + signature);
            }
        }
    }
}
