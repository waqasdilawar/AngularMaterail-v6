using AngularTest1.IdentityClass;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AngularTest1.Extensions
{
    public static class ClaimExtension
    {
        public static async Task AddUpdateClaimAsync(ClaimsIdentity claimsIdentity, UserManager<ApplicationUser> userManager, ApplicationUser user, string key, string value)
        {
            // check for existing claim and remove it
            var existingClaim = claimsIdentity.FindFirst(key);
            if (existingClaim != null)
            {
                await _RemoveClaimAsync(claimsIdentity, userManager, user, key);
            }

            // add new claim
            var claim = new Claim(key, value);
            claimsIdentity.AddClaim(claim);
            //Persist to store
            await userManager.AddClaimAsync(user, claim);
        }
        private static async Task _RemoveClaimAsync(ClaimsIdentity claimsIdentity, UserManager<ApplicationUser> userManager,
            ApplicationUser user,string key)
        {
            try
            {
                if (claimsIdentity == null)
                    return;

                // check for existing claim and remove it
                var existingClaims = claimsIdentity.FindAll(key).ToList();
                existingClaims.ForEach(c => claimsIdentity.RemoveClaim(c));

                //remove old claims from store
                var claims = await userManager.GetClaimsAsync(user);
              
                foreach (var item in claims.Where(x => x.Type == key))
                {
                    await userManager.RemoveClaimAsync(user, item);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
