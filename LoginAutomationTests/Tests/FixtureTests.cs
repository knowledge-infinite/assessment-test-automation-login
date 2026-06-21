using Microsoft.Playwright;
using Xunit;

namespace HMCTS_LoginAutomationTests.Tests
{
    public class FixtureTests : IAsyncLifetime
    {
        public IPlaywright Playwright { get; private set; } = default!;
        public IBrowser Browser { get; private set; } = default!;

        public IBrowser GetBrowser()
        {
            return Browser;
        }

        // Implements IAsyncLifetime.InitializeAsync()
        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
        }

       
        public async Task DisposeAsync()
        {
            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }
}
