/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    internal class GetObjectCommand : OssCommand<OssObject>
    {
        private readonly GetObjectRequest _getObjectRequest;

        protected override string Bucket
        {
            get { return _getObjectRequest.BucketName; }
        }
        
        protected override string Key
        { 
            get { return _getObjectRequest.Key; }
        }

        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                _getObjectRequest.Populate(headers);
                return headers;
            }
        }

        protected override IDictionary<string, string> Parameters
        {
            get
            {
                var parameters = base.Parameters;
                _getObjectRequest.ResponseHeaders.Populate(parameters);
                return parameters;
            }
        }

        protected override bool LeaveResponseOpen
        {
            get { return true; }
        }

        private GetObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, OssObject> deserializer,
                                 GetObjectRequest getObjectRequest)
            : base(client, endpoint, context, deserializer)
        {
            _getObjectRequest = getObjectRequest;
        }

        public static GetObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              GetObjectRequest getObjectRequest)
        {
            OssUtils.CheckBucketName(getObjectRequest.BucketName);
            OssUtils.CheckObjectKey(getObjectRequest.Key);
            return new GetObjectCommand(client, endpoint, context,
                                 DeserializerFactory.GetFactory().CreateGetObjectResultDeserializer(getObjectRequest),
                                 getObjectRequest);
        }
    }
}
