/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class ListMultipartUploadsResponseDeserializer : ResponseDeserializer<MultipartUploadListing, ListMultipartUploadsResult>
    {
        public ListMultipartUploadsResponseDeserializer(IDeserializer<Stream, ListMultipartUploadsResult> contentDeserializer)
            : base(contentDeserializer)
        { }
             
        public override MultipartUploadListing Deserialize(ServiceResponse xmlStream)
        {
            var listMultipartUploadsResult = ContentDeserializer.Deserialize(xmlStream.Content);
            
            var uploadsList = new MultipartUploadListing(listMultipartUploadsResult.Bucket)
            {
                BucketName = listMultipartUploadsResult.Bucket,
                Delimiter = listMultipartUploadsResult.Delimiter,
                IsTruncated = listMultipartUploadsResult.IsTruncated,
                KeyMarker = listMultipartUploadsResult.KeyMarker,
                MaxUploads = listMultipartUploadsResult.MaxUploads,
                NextKeyMarker = listMultipartUploadsResult.NextKeyMarker,
                NextUploadIdMarker = listMultipartUploadsResult.NextUploadIdMarker,
                Prefix = listMultipartUploadsResult.Prefix,
                UploadIdMarker = listMultipartUploadsResult.UploadIdMarker
            };

            if (listMultipartUploadsResult.CommonPrefix != null)
            {
                if (listMultipartUploadsResult.CommonPrefix.Prefixs != null)
                {
                    foreach (var prefix in listMultipartUploadsResult.CommonPrefix.Prefixs)
                    {
                        uploadsList.AddCommonPrefix(prefix);
                    }
                }
            }
            
            if (listMultipartUploadsResult.Uploads != null)
            {
                foreach (var uploadResult in listMultipartUploadsResult.Uploads)
                {
                    var upload = new MultipartUpload
                    {
                        Initiated = uploadResult.Initiated,
                        Key = uploadResult.Key,
                        UploadId = uploadResult.UploadId,
                        StorageClass = uploadResult.StorageClass
                    };
                    uploadsList.AddMultipartUpload(upload);
                }
            }
            
            return uploadsList;
        }
    }
}
