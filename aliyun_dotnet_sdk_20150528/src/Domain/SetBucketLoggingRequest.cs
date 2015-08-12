/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.OpenStorageService
{
   public class SetBucketLoggingRequest
    {
        /// <summary>
        /// 获取或设置<see cref="Bucket"/>名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// 获取或设置存放访问日志的Bucket。
        /// </summary>
        public string TargetBucket { get; private set; }

        /// <summary>
        /// 获取或设置存放访问日志的文件名前缀。
        /// </summary>
        public string TargetPrefix { get; private set; }

        public SetBucketLoggingRequest(string bucketName, string targetBucket, string targetPrefix)
        {
            BucketName = bucketName;
            TargetBucket = targetBucket;
            TargetPrefix = targetPrefix;
        }
    }
}
