/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class DeleteObjectsResultDeserializer : ResponseDeserializer<DeleteObjectsResult, DeleteObjectsResult>
    {
        public DeleteObjectsResultDeserializer(IDeserializer<Stream, DeleteObjectsResult> contentDeserializer)
                 : base(contentDeserializer)
        { }

        public override DeleteObjectsResult Deserialize(ServiceResponse xmlStream)
        {
            if (int.Parse(xmlStream.Headers[HttpHeaders.ContentLength]) == 0)
                return new DeleteObjectsResult();
            
            return ContentDeserializer.Deserialize(xmlStream.Content);
        }
    }
}
