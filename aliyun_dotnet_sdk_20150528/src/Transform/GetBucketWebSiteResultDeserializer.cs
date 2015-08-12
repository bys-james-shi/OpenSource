/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class GetBucketWebSiteResultDeserializer : ResponseDeserializer<BucketWebsiteResult, SetBucketWebsiteRequestModel>
    {
        public GetBucketWebSiteResultDeserializer(IDeserializer<Stream, SetBucketWebsiteRequestModel> contentDeserializer)
            : base(contentDeserializer)
        { }

        public override BucketWebsiteResult Deserialize(ServiceResponse xmlStream)
        {
            var model = ContentDeserializer.Deserialize(xmlStream.Content);
            return new BucketWebsiteResult
            {
                IndexDocument = model.IndexDocument.Suffix,
                ErrorDocument = model.ErrorDocument.Key
            };
        }
    }
}
