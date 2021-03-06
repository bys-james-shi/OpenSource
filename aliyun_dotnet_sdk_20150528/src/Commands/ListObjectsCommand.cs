﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Globalization;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    internal class ListObjectsCommand : OssCommand<ObjectListing>
    {
        private readonly ListObjectsRequest _listObjectsRequest;

        protected override string Bucket
        {
            get { return _listObjectsRequest.BucketName; }
        }  

        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                Populate(_listObjectsRequest, parameters);
                return parameters;
            }
        }

        private ListObjectsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                   IDeserializer<ServiceResponse, ObjectListing> deserializer,
                                   ListObjectsRequest listObjectsRequest)
            : base(client, endpoint, context, deserializer)
        {
            _listObjectsRequest = listObjectsRequest;
        }
        
        public static ListObjectsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                ListObjectsRequest listObjectsRequest)
        {
            return new ListObjectsCommand(client, endpoint, context,
                                          DeserializerFactory.GetFactory().CreateListObjectsResultDeserializer(),
                                          listObjectsRequest);
        }

        private static void Populate(ListObjectsRequest listObjectsRequest, IDictionary<string, string> parameters)
        {
            if (listObjectsRequest.Prefix != null) 
            {
                parameters[RequestParameters.PREFIX] = listObjectsRequest.Prefix;
            }

            if (listObjectsRequest.Marker != null) 
            {
                parameters[RequestParameters.MARKER] = listObjectsRequest.Marker;
            }

            if (listObjectsRequest.Delimiter != null) 
            {
                parameters[RequestParameters.DELIMITER] = listObjectsRequest.Delimiter;
            }

            if (listObjectsRequest.MaxKeys.HasValue) 
            {
                parameters[RequestParameters.MAX_KEYS] = 
                    listObjectsRequest.MaxKeys.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
