using System;
using Xunit;

using test_app.api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;
using test_app.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using test_app.api.Data;

namespace test_app.api.tests
{

    public class UnitTest1
    {
        [Fact]
        public async void Auth_And_GetCurrentUser()
        {
            // Arrange
            var steamId = "76561198038038951";

            IList<string> roles = new List<string>() { "User", "Admin" };
            var now = DateTime.UtcNow;

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimsIdentity.DefaultNameClaimType, steamId),
                new Claim(ClaimTypes.NameIdentifier, steamId)
            };

            claims.AddRange(roles.Select(x => new Claim(ClaimsIdentity.DefaultRoleClaimType, x)));

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var user = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims));

            var mockStore = Mock.Of<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockStore, null, null, null, null, null, null, null, null);
            var mockDbContext = new Mock<ApplicationDbContext>();

            mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new UserController(mockUserManager.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var founded = await mockUserManager.Object.FindByIdAsync(steamId);

            // Assert
            Assert.NotNull(founded);
        }
    }
}
