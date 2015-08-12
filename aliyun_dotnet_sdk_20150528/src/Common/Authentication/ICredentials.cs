/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.Common.Authentication
{
    public interface ICredentials
    {
        string AccessId { get; }

        string AccessKey { get; }

        string SecurityToken { get; }

        bool UseToken { get; }
    }
}
