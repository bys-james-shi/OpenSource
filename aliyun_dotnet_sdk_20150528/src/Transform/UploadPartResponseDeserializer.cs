/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class UploadPartResultDeserializer : ResponseDeserializer<UploadPartResult, UploadPartResult>
    {
        private readonly int _partNumber;
        
        public UploadPartResultDeserializer(int partNumber)
            : base(null)
        {
            _partNumber = partNumber;
        }
        
        public override UploadPartResult Deserialize(ServiceResponse xmlStream)
        {
            var result = new UploadPartResult();
            if (xmlStream.Headers.ContainsKey(HttpHeaders.ETag))
            {
                result.ETag = OssUtils.TrimQuotes(xmlStream.Headers[HttpHeaders.ETag]);
            }
            result.PartNumber = _partNumber;
            
            return result;
        }
    }
}
