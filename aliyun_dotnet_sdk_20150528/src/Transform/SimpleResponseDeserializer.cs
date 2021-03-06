﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class SimpleResponseDeserializer<T> : ResponseDeserializer<T, T>
    {
        public SimpleResponseDeserializer(IDeserializer<Stream, T> contentDeserializer)
            : base(contentDeserializer)
        { }

        public override T Deserialize(ServiceResponse xmlStream)
        {
            using (xmlStream.Content)
            {
                return ContentDeserializer.Deserialize(xmlStream.Content);
            }
        }
    }
}