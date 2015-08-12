﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 指定中止Multipart Upload事件的请求参数
    /// </summary>
    public class AbortMultipartUploadRequest
    {
        /// <summary>
        /// 获取<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }
        
        /// <summary>
        /// 获取或者设置<see cref="OssObject" />的值。
        /// </summary>
        public string Key { get; private set; }
        
        /// <summary>
        /// 获取或者设置需要取消的UploadId。
        /// </summary>
        public string UploadId { get; private set; }
        
        public AbortMultipartUploadRequest(string bucketName, string key, string uploadId)
        {
            BucketName = bucketName;
            Key = key;
            UploadId = uploadId;
        }
    }
}