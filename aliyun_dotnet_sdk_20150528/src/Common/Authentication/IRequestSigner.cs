/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.Common.Authentication
{
    internal interface IRequestSigner
    {
        void Sign(ServiceRequest request, ICredentials credentials);
    }
}
