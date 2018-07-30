using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularTest1.Models
{
    public class AppUserAuth
    {
        public AppUserAuth() : base()
        {
            UserName = "Not authorized";
            BearerToken = string.Empty;
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string BearerToken { get; set; }
        public bool IsAuthenticated { get; set; }

        public List<AppUserClaim> Claims { get; set; }
    }
}
