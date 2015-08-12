/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.Common.Authentication
{
    public class DefaultCredentials : ICredentials 
    {
        public string AccessId { get; private set; }
        public string AccessKey { get; private set; }
        public string SecurityToken { get; private set; }
        public bool UseToken { get { return !string.IsNullOrEmpty(SecurityToken); } }

        public DefaultCredentials(string accessId, string accessKey, string securityToken)
        {
            OssUtils.CheckCredentials(accessId, accessKey);

            AccessId = accessId;
            AccessKey = accessKey;
            SecurityToken = securityToken ?? string.Empty;
        }
    }
}
