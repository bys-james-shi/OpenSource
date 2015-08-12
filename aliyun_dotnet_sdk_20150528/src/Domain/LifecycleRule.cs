/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// Lifecycle规则状态。
    /// </summary>
    public enum RuleStatus
    {
        Enabled,    // 启用规则
        Disabled    // 禁用规则
    };

    /// <summary>
    /// 表示一条Lifecycle规则。
    /// </summary>
    public class LifecycleRule
    {
        public string ID { get; set; }
        public string Prefix { get; set; }
        public RuleStatus Status { get; set; }
        public int? ExpriationDays { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
