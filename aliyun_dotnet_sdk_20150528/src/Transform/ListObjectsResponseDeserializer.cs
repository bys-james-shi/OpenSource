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
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class ListObjectsResponseDeserializer : ResponseDeserializer<ObjectListing, ListBucketResult>
    {
        public ListObjectsResponseDeserializer(IDeserializer<Stream, ListBucketResult> contentDeserializer)
            : base(contentDeserializer)
        { }
        
        public override ObjectListing Deserialize(ServiceResponse xmlStream)
        {
            var listBucketResult = ContentDeserializer.Deserialize(xmlStream.Content);
            
            var objectList = new ObjectListing(listBucketResult.Name)
            {
                Delimiter = listBucketResult.Delimiter,
                IsTruncated = listBucketResult.IsTruncated,
                Marker = listBucketResult.Marker,
                MaxKeys = listBucketResult.MaxKeys,
                NextMarker = listBucketResult.NextMarker,
                Prefix = listBucketResult.Prefix
            };

            if (listBucketResult.Contents != null)
            {
                foreach(var contents in listBucketResult.Contents)
                {
                    var summary = new OssObjectSummary
                    {
                        BucketName = listBucketResult.Name,
                        Key = contents.Key,
                        LastModified = contents.LastModified,
                        ETag = contents.ETag != null ? OssUtils.TrimQuotes(contents.ETag) : string.Empty,
                        Size = contents.Size,
                        StorageClass = contents.StorageClass,
                        Owner = contents.Owner != null ? 
                                new Owner(contents.Owner.Id, contents.Owner.DisplayName) : null
                    };

                    objectList.AddObjectSummary(summary);
                }
            }

            if (listBucketResult.CommonPrefixes != null)
            {
                foreach(var commonPrefixes in listBucketResult.CommonPrefixes)
                {
                    if (commonPrefixes.Prefix != null)
                    {
                        foreach(var prefix in commonPrefixes.Prefix)
                        {
                            objectList.AddCommonPrefix(prefix);
                        }
                    }
                }
            }
            return objectList;
        }
    }
}
