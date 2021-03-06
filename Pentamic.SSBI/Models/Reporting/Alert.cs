﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class Alert : IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string MainValueField { get; set; }
        public string MainValueModification { get; set; }
        public string MainFilterExpression { get; set; }
        public string MainCustomExpression { get; set; }

        public string TargetValueField { get; set; }
        public string TargetValueModification { get; set; }
        public string TargetFilterExpression { get; set; }
        public string TargetCustomExpression { get; set; }

        public AlertCondition Condition { get; set; }
        public bool UseThresold { get; set; }
        public decimal Thresold { get; set; }

        public AlertFrequency Frequency { get; set; }
        public DateTimeOffset LastRun { get; set; }

        public bool IsActive { get; set; }

        public int NotificationId { get; set; }

        public Notification Notification { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}