using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCTS_LoginAutomationTests.PageObjects
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly string _baseUrl;

        public LoginPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public Task GotoAsync() =>
            _page.GotoAsync($"{_baseUrl}/login");

        public async Task LoginAsync(string username, string password)
        {
            await _page.GetByLabel("Username").FillAsync(username);   // label-based
            await _page.GetByLabel("Password").FillAsync(password);   // label-based
            await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        }

        public Task<string> GetFlashMessageAsync() =>
            _page.InnerTextAsync("#flash");
    }
}
