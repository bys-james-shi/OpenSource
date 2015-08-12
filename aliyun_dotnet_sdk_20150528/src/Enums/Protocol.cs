/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 表示请求OSS服务时采用的通信协议，默认值为HTTP。
    /// </summary>
    public enum Protocol
    {
        [StringValue("http")]
        Http = 0,

        [StringValue("https")]
        Https
    }
}
