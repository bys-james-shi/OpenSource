/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.Common.Authentication
{
    public class DefaultCredentialsProvider : ICredentialsProvider
    {
        private volatile ICredentials _creds;

        public DefaultCredentialsProvider(ICredentials creds)
        {
            SetCredentials(creds);
        }

        public void SetCredentials(ICredentials creds)
        {
            if (creds == null)
                throw new ArgumentNullException("creds");

            OssUtils.CheckCredentials(creds.AccessId, creds.AccessKey);
            _creds = creds;
        }

        public ICredentials GetCredentials()
        {
            return _creds;
        }
    }
}
