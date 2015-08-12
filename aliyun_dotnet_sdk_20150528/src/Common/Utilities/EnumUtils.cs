﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.Common.Utilities
{
    internal static class EnumUtils
    {
        private static readonly IDictionary<Enum, StringValueAttribute> StringValues =
            new Dictionary<Enum, StringValueAttribute>();

        public static string GetStringValue(this Enum value)
        {
            string output;
            var type = value.GetType();

            if (StringValues.ContainsKey(value))
            {
                output = StringValues[value].Value;
            }
            else
            {
                var fi = type.GetField(value.ToString());
                var attrs = fi.GetCustomAttributes(typeof (StringValueAttribute), false) 
                    as StringValueAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    output = attrs[0].Value;
                    // Put it in the cache.
                    lock(StringValues)
                    {
                        // Double check
                        if (!StringValues.ContainsKey(value))
                        {
                            StringValues.Add(value, attrs[0]);
                        }
                    }
                }
                else
                {
                    return value.ToString();
                }
            }

            return output;
        }
    }
}
