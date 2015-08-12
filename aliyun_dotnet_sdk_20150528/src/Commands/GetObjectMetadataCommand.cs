/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    internal class GetObjectMetadataCommand : OssCommand<ObjectMetadata>
    {
        private readonly string _bucketName;
        private readonly string _key;

        protected override HttpMethod Method
        {
            get { return HttpMethod.Head; }
        }

        protected override string Bucket
        {
            get { return _bucketName; }
        }

        protected override string Key
        {
            get { return _key; }
        }

        private GetObjectMetadataCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                         IDeserializer<ServiceResponse, ObjectMetadata> deserializer,
                                         string bucketName, string key)
            : base(client, endpoint, context, deserializer)
        {
            OssUtils.CheckBucketName(bucketName);
            OssUtils.CheckObjectKey(key);

            _bucketName = bucketName;
            _key = key;
        }

        public static GetObjectMetadataCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                      string bucketName, string key)
        {
            return new GetObjectMetadataCommand(client, endpoint, context,
                                                DeserializerFactory.GetFactory().CreateGetObjectMetadataResultDeserializer(),
                                                bucketName, key);
        }
    }

}
