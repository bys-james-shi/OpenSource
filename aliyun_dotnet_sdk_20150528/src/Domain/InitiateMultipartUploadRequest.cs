/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;

namespace Aliyun.OpenServices.OpenStorageService
{
#pragma warning disable 618, 3005

    /// <summary>
    /// 指定初始化Multipart Upload的请求参数。
    /// </summary>
    public class InitiateMultipartUploadRequest
    {
        /// <summary>
        /// 获取或者设置<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; set; }
        
        /// <summary>
        /// 获取或者设置<see cref="OssObject" />的值。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 已过时。请使用属性字段ObjectMetadata。
        /// </summary>
        [Obsolete("misspelled, please use ObjectMetadata instead")]
        public ObjectMetadata ObjectMetaData { get; set; }

        /// <summary>
        /// 获取或设置<see cref="OpenStorageService.ObjectMetadata" />
        /// </summary>
        public ObjectMetadata ObjectMetadata { get; set; }
        
        public InitiateMultipartUploadRequest(string bucketName, string key) 
            : this(bucketName, key, null)
        { }
        
        public InitiateMultipartUploadRequest(string bucketName, string key, 
            ObjectMetadata objectMetadata)
        {
            BucketName = bucketName;
            Key = key;
            ObjectMetadata = objectMetadata;
        }
    }

#pragma warning disable 618, 3005
}
