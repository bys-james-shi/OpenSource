﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.OpenStorageService
{
    public class SetBucketWebsiteRequest
    {
        /// <summary>
        /// 获取或者设置<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// 索引页面
        /// </summary>
        public string IndexDocument { get; private set; }

        /// <summary>
        /// 错误页面
        /// </summary>
        public string ErrorDocument { get; private set; }

        public SetBucketWebsiteRequest(string bucketName, string indexDocument, string errorDocument)
        {
            BucketName = bucketName;
            IndexDocument = indexDocument;
            ErrorDocument = errorDocument;
        }
    }
}
