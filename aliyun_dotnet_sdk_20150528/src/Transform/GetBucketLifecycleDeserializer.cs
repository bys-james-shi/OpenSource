/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class GetBucketLifecycleDeserializer 
        : ResponseDeserializer<IList<LifecycleRule>, LifecycleConfiguration>
    {
        public GetBucketLifecycleDeserializer(IDeserializer<Stream, LifecycleConfiguration> contentDeserializer)
            : base(contentDeserializer)
        { }

        public override IList<LifecycleRule> Deserialize(ServiceResponse xmlStream)
        {
            var lifecycleConfig = ContentDeserializer.Deserialize(xmlStream.Content);
            var rules = new List<LifecycleRule>();
            foreach (var lcc in lifecycleConfig.LifecycleRules)
            {
                var rule = new LifecycleRule
                {
                    ID = lcc.ID, 
                    Prefix = lcc.Prefix
                };

                RuleStatus status;
                if (Enum.TryParse(lcc.Status, out status))
                    rule.Status = status;
                else
                    throw new InvalidEnumArgumentException(@"Unsupported rule status " + lcc.Status);

                if (lcc.Expiration.IsSetDays())
                    rule.ExpriationDays = lcc.Expiration.Days;
                else if (lcc.Expiration.IsSetDate())
                    rule.ExpirationTime = DateTime.Parse(lcc.Expiration.Date);

                rules.Add(rule);
            }
            return rules;            
        }
    }
}
