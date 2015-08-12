/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    internal class DeleteObjectCommand : OssCommand
    {
        private readonly string _bucketName;
        private readonly string _key;

        protected override HttpMethod Method
        {
            get { return HttpMethod.Delete; }
        }

        protected override string Bucket
        {
            get { return _bucketName; }
        }
        
        protected override string Key
        {
            get { return _key; }
        }

        private DeleteObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                    string bucketName, string key)
            : base(client, endpoint, context)
        {
            OssUtils.CheckBucketName(bucketName);
            OssUtils.CheckObjectKey(key);

            _bucketName = bucketName;
            _key = key;
        }

        public static DeleteObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                 string bucketName, string key)
        {
            return new DeleteObjectCommand(client, endpoint, context, bucketName, key);
        }
    }
}
