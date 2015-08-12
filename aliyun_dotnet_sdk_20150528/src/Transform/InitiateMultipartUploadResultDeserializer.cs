/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class InitiateMultipartUploadResultDeserializer 
        : ResponseDeserializer<InitiateMultipartUploadResult, InitiateMultipartResult>
    {
        public InitiateMultipartUploadResultDeserializer(IDeserializer<Stream, InitiateMultipartResult> contentDeserializer)
                 : base(contentDeserializer)
        { }
        
        public override InitiateMultipartUploadResult Deserialize(ServiceResponse xmlStream)
        {
            var result = ContentDeserializer.Deserialize(xmlStream.Content);
            return new InitiateMultipartUploadResult
            {
                BucketName = result.Bucket,
                Key = result.Key,
                UploadId = result.UploadId
            };
        }
    }
}
