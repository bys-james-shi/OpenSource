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
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    internal class ListMultipartUploadsCommand : OssCommand<MultipartUploadListing>
    {
        private readonly ListMultipartUploadsRequest _listMultipartUploadsRequest;

        protected override HttpMethod Method
        {
            get { return HttpMethod.Get; }
        }

        protected override string Bucket
        {
            get { return _listMultipartUploadsRequest.BucketName; }
        }
        
        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                Populate(_listMultipartUploadsRequest, parameters);
                return parameters;
            }
        }
        
        private ListMultipartUploadsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, MultipartUploadListing> deserializeMethod,
                                 ListMultipartUploadsRequest listMultipartUploadsRequest)
            : base(client, endpoint, context, deserializeMethod)
        {
            OssUtils.CheckBucketName(listMultipartUploadsRequest.BucketName);
            _listMultipartUploadsRequest = listMultipartUploadsRequest;
        }
        
        public static ListMultipartUploadsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                ListMultipartUploadsRequest listMultipartUploadsRequest)
        {
            return new ListMultipartUploadsCommand(client, endpoint,context, 
                                                   DeserializerFactory.GetFactory().CreateListMultipartUploadsResultDeserializer(),
                                                   listMultipartUploadsRequest);
        }
        
        private static void Populate(ListMultipartUploadsRequest listMultipartUploadsRequest, 
                                    IDictionary<string, string> parameters)
        {
            parameters[RequestParameters.SUBRESOURCE_UPLOADS] = null;
            if (listMultipartUploadsRequest.Delimiter != null)
            {
                parameters[RequestParameters.DELIMITER] = listMultipartUploadsRequest.Delimiter;
            }
            
            if (listMultipartUploadsRequest.KeyMarker != null)
            {
                parameters[RequestParameters.KEY_MARKER] = listMultipartUploadsRequest.KeyMarker;
            }
            
            if (listMultipartUploadsRequest.MaxUploads.HasValue)
            {
                parameters[RequestParameters.MAX_UPLOADS] = 
                    listMultipartUploadsRequest.MaxUploads.Value.ToString(CultureInfo.InvariantCulture); ;
            }
            
            if (listMultipartUploadsRequest.Prefix != null)
            {
                parameters[RequestParameters.PREFIX] = listMultipartUploadsRequest.Prefix;
            }
            
            if (listMultipartUploadsRequest.UploadIdMarker != null)
            {
                parameters[RequestParameters.UPLOAD_ID_MARKER] = listMultipartUploadsRequest.UploadIdMarker;
            }
        }
        
    }
}
