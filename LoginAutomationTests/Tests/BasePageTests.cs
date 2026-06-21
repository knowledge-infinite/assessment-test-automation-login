using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCTS_LoginAutomationTests.Tests
{
    public abstract class BasePageTests : IClassFixture<FixtureTests>
    {
        protected readonly IBrowser Browser;
        protected const string BaseUrl = "https://the-internet.herokuapp.com/";

        protected BasePageTests(FixtureTests fixture)
        {
            Browser = fixture.Browser;
        }

        protected async Task<IPage> NewPageAsync()
        {
            var context = await Browser.NewContextAsync();
            return await context.NewPageAsync();
        }
    }
}
