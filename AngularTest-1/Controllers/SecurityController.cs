using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AngularTest1.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AngularTest1.Models;
using AngularTest1.Models;
using AngularTest1.IdentityClass;

namespace AngularTest1.Controllers
{
    [Produces("application/json")]
    [Route("api/Security")]
    public class SecurityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IPasswordHasher<ApplicationUser> _haser;

        private IConfiguration _config;
        private readonly ILogger _logger;
        public SecurityController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> hasher,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _config = configuration;
            _haser = hasher;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<SecurityController>();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var claimResult = await _userManager.AddClaimAsync(user, new Claim("Speaker", "True"));
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return Ok(user);
                }
                return BadRequest(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        /// <summary>
        ///  This action is used only for First time login when user don't have Refresh Token and enters Credentials
        /// </summary>
        /// <param name="loginViewModel">
        /// It Accepts UserName and Password
        /// </param>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.Email);
            
                var now = DateTime.Now;
                var refreshToken = "";
                if (user != null)
                {
                    if (_haser.VerifyHashedPassword(user, user.PasswordHash, loginViewModel.Password)
                        == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _userManager.GetClaimsAsync(user);
                        
                        List<AppUserClaim> appUserClaim = new List<AppUserClaim>();
                        foreach (var item in userClaims)
                        {
                            appUserClaim.Add( new AppUserClaim(){
                                ClaimType=item.Type,
                                ClaimValue=item.Value,
                                UserId=user.Id,
                                ClaimId=null
                               });
                        }
                        if (appUserClaim == null)
                        {

                        }
                        if (loginViewModel.RememberMe)
                        {
                            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
                            if (!await _roleManager.RoleExistsAsync("RefreshToken"))
                                await _roleManager.CreateAsync(new IdentityRole { Name = "RefreshToken" });
                            await _userManager.AddToRoleAsync(user, "RefreshToken");
                            var isToRemembered = await _userManager.IsInRoleAsync(user, "RefreshToken");
                            if (isToRemembered)
                            {
                                IIdentity identity = new GenericIdentity(user.UserName);
                                ClaimsIdentity claimsIdentity = new ClaimsIdentity(identity);
                                claimsIdentity.AddClaims(userClaims.OrderBy(c => c.Type));
                                //Add refresh token to current user's identity claims
                                await ClaimExtension.AddUpdateClaimAsync(claimsIdentity, _userManager, user, "RefreshToken", refresh_token);
                                await ClaimExtension.AddUpdateClaimAsync(claimsIdentity, _userManager, user, "DaysToExpire", _config["Tokens:DaysToExpireRefreshToken"]);
                                await ClaimExtension.AddUpdateClaimAsync(claimsIdentity, _userManager, user, "RefreshTokenExpired", "False");
                            }
                            refreshToken = refresh_token;
                        }
                        //var userClaims = await _userManager.GetClaimsAsync(user);
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }.Union(userClaims);
                    
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var jwt = new JwtSecurityToken(
                            issuer: _config["Tokens:Issuer"],
                            audience: _config["Tokens:Audience"],
                            claims: claims,
                            signingCredentials: cred,
                            expires: now.AddMinutes(Convert.ToInt32(
                                         _config["Tokens:MinutesToExpiration"]))
                            );
                        var handler = new JwtSecurityTokenHandler();
                        var encodedJwt = handler.WriteToken(jwt);
                        var token = new OAuthToken
                        {
                            UserId = user.Id.ToString(),
                            UserName = user.UserName,
                            BearerToken = encodedJwt,
                            ExpiresIn = jwt.ValidTo,
                            RefreshToken = refreshToken,
                            Claims = appUserClaim,
                            IsAuthenticated=true
                        };

                        return Ok(token);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return BadRequest("Something bad happened");
        }
    }
}