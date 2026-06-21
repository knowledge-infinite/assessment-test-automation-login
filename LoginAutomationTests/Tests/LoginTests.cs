using HMCTS_LoginAutomationTests.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCTS_LoginAutomationTests.Tests
{
    public class LoginTests : BasePageTests
    {
        private const string userName = "tomsmith";
        private const string password = "SuperSecretPassword";
        public LoginTests(FixtureTests fixture) : base(fixture)
        {
                
        }

        [Fact]
        public async Task Valid_credentials_should_display_secure_area_page()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page, BaseUrl);
            var secureAreaPage = new SecureAreaPage(page, BaseUrl);

            await loginPage.GotoAsync();
            await loginPage.LoginAsync("tomsmith", "SuperSecretPassword!");

            // this checks the URL after login
            Assert.Contains("/secure", page.Url, StringComparison.OrdinalIgnoreCase);

            Assert.True(await secureAreaPage.IsLoadedAsync());
        }

        [Fact]
        public async Task Invalid_credentials_should_display_error_message()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page, BaseUrl);
            var secureAreaPage = new SecureAreaPage(page, BaseUrl);

            await loginPage.GotoAsync();
            await loginPage.LoginAsync("invalidUser", "invalidPassword");
                
            var flash = await loginPage.GetFlashMessageAsync();
            Assert.Contains("Your username is invalid!", flash, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Invalid_username_should_display_error_message()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page, BaseUrl);
            var secureAreaPage = new SecureAreaPage(page, BaseUrl);

            await loginPage.GotoAsync();
            await loginPage.LoginAsync("invalidUser", password);
                
            var flash = await loginPage.GetFlashMessageAsync();
            Assert.Contains("Your username is invalid!", flash, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Invalid_password_should_display_error_message()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page, BaseUrl);
            var secureAreaPage = new SecureAreaPage(page, BaseUrl);

            await loginPage.GotoAsync();
            await loginPage.LoginAsync(userName, "invalidPassword");
                
            var flash = await loginPage.GetFlashMessageAsync();
            Assert.Contains("Your password is invalid!", flash, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Logout_should_redirect_to_login_page()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page, BaseUrl);
            var secureAreaPage = new SecureAreaPage(page, BaseUrl);

            await loginPage.GotoAsync();
            await loginPage.LoginAsync("tomsmith", "SuperSecretPassword!");

            Assert.True(await secureAreaPage.IsLoadedAsync()); // first True that is currently failing

            await secureAreaPage.LogoutAsync();

            Assert.Contains("/login", page.Url, StringComparison.OrdinalIgnoreCase);
            Assert.True(await page.IsVisibleAsync("#username"));
        }


    }
}
