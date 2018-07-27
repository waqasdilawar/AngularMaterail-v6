using System;
using System.Collections.Generic;

namespace AngularTest.Models
{
    public partial class FeatureFunding
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal TargetAmount { get; set; }
        public byte State { get; set; }
        public DateTime FundingEndsAt { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
    }
}
