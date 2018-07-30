using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AngularTest1.IdentityClass
{
    public class ApplicationUser : IdentityUser
    {
     
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Cnic { get; set; }

        public bool IsProfileCreated { get; set; }
        public string HaveProfile { get; set; }
        public string TeamId { get; set; }
    }
}
