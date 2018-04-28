using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using test_app.shared.Data;
using test_app.shared.Logic;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api_admin.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly SteamOptions _steamOptions;

        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IUserRepository userRepository,
           IOptions<SteamOptions> optionsAccessor,
           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _steamOptions = optionsAccessor.Value;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [AllowAnonymous]
        public IActionResult SignInWithSteam()
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", null);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Steam", redirectUrl);

            return Challenge(properties, "Steam");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction("LoginError", BaseHttpResult.GenerateError(remoteError, ResponseType.AccessDenied));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("LoginError", BaseHttpResult.GenerateError("error getting info", ResponseType.AccessDenied));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var roles = await _userManager.GetRolesAsync(user);

                return PartialView("Close", TokenHelper.Generate(user, roles));
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return RedirectToAction("ExternalLoginConfirmation");
                //return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation()
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var steamId = new Uri(info.ProviderKey).Segments.Last();

                var player = new SteamPlayerSummaryDto();

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_steamOptions.ApiKey}&steamids={steamId}");

                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    player = JsonConvert.DeserializeObject<SteamPlayerSummaryRootObject>(stringResponse).Response.Players[0];
                }

                var user = new ApplicationUser
                {
                    Id = steamId,
                    UserName = player.PersonaName,
                    Email = player.PersonaName,
                    SteamId = player.SteamId,
                    SteamUsername = player.PersonaName,
                    SteamAvatar = player.AvatarMedium,
                    SteamProfileState = player.ProfileState
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, "User");

                    if (result.Succeeded)
                    {
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return PartialView("Close", TokenHelper.Generate(user, new List<string>() { "User" }));
                        }
                    }
                }
                AddErrors(result);
            }

            return Ok(true);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}