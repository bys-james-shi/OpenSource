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
    internal class DeleteBucketCommand : OssCommand
    {
        private readonly string _bucketName;

        protected override HttpMethod Method
        {
            get { return HttpMethod.Delete; }
        }

        protected override string Bucket
        {
            get { return _bucketName; }
        }      
        
        private DeleteBucketCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                    string bucketName)
            : base(client, endpoint, context)
        {
            OssUtils.CheckBucketName(bucketName);
            _bucketName = bucketName;
        }
        
        public static DeleteBucketCommand Create(IServiceClient client, Uri endpoint,
                                                 ExecutionContext context,
                                                 string bucketName)
        {
            return new DeleteBucketCommand(client, endpoint, context, bucketName);
        }
    }
}
