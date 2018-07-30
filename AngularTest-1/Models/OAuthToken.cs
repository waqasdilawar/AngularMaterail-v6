using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularTest1.Models
{
    public class OAuthToken
    {
        public string BearerToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public IEnumerable<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();
    }
}
