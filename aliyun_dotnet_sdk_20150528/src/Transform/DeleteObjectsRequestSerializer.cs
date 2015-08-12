/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using System.Linq;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class DeleteObjectsRequestSerializer : RequestSerializer<DeleteObjectsRequest, DeleteObjectsRequestModel>
    {
        public DeleteObjectsRequestSerializer(ISerializer<DeleteObjectsRequestModel, Stream> contentSerializer)
            : base(contentSerializer)
        {
        }

        public override Stream Serialize(DeleteObjectsRequest request)
        {
            var model = new DeleteObjectsRequestModel
            {
                Quiet = request.Quiet,
                Keys = request.Keys.Select(key => new DeleteObjectsRequestModel.ObjectToDel {Key = key}).ToArray()
            };
            return ContentSerializer.Serialize(model);
        }
    }
}
