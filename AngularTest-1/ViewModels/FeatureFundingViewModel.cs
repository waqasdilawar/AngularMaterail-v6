using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularTest1.ViewModels
{
    public class FeatureFundingViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal TargetAmount { get; set; }
        public byte State { get; set; }
        public DateTime FundingEndsAt { get; set; }
    }
}
