﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class SetBucketWebsiteRequestSerializer : RequestSerializer<SetBucketWebsiteRequest, SetBucketWebsiteRequestModel>
    {
        public SetBucketWebsiteRequestSerializer(ISerializer<SetBucketWebsiteRequestModel, Stream> contentSerializer)
            : base(contentSerializer)
        { }

        public override Stream Serialize(SetBucketWebsiteRequest request)
        {
            var model = new SetBucketWebsiteRequestModel
            {
                ErrorDocument = new SetBucketWebsiteRequestModel.ErrorDocumentModel(),
                IndexDocument = new SetBucketWebsiteRequestModel.IndexDocumentModel {Suffix = request.IndexDocument}
            };

            model.ErrorDocument.Key = request.ErrorDocument;
            
            return ContentSerializer.Serialize(model);
        }
    }
}
