using System;
using System.Collections.Generic;

namespace AngularTest.Models
{
    public partial class FeatureFundinginterest
    {
        public int Id { get; set; }
        public int FeatureFundingId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
    }
}
