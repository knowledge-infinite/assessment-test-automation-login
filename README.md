This automation test solution focuses on the login (positive and negative scenarios) and logout flows, demonstrating clean test design, 
Page Object Model (POM), and the use of fixtures.


Tech stack used:
.NET (C#) test project

[Playwright for .NET] for browser automation

xUnit as the test framework

Target application: https://the-internet.herokuapp.com/login (Form Authentication)

How to run the tests

1. Install Playwright CLI and browser binaries (once)
From the test project folder:

bash
dotnet tool install --global Microsoft.Playwright.CLI
playwright install
If the CLI was already installed:

bash
dotnet tool update --global Microsoft.Playwright.CLI
playwright install
This downloads the Chromium/Chrome binaries that Playwright uses to run tests.

2. Run tests
From the test project folder:

bash
dotnet test
By default the tests are configured to run in headless mode for speed and to reflect how they would run in CI.

To watch the browser interaction locally, set Headless = false in PlaywrightFixture before running dotnet test.

Project structure:

HMCTS_LoginAutomationTests/
  ├── Tests/
  │   └── BasePageTests.cs
      └── FixtureTests.cs
      └── LoginTests.cs
  ├── Pages Objects/
  │   ├── LoginPage.cs
  │   └── SecureAreaPage.cs
 

      
Pages/ – Page Object classes encapsulating locators and UI behaviour.
Tests/ – xUnit tests that express business scenarios in terms of the page objects.
TestInfrastructure/ – Cross‑cutting infrastructure such as the shared Playwright fixture.



Design and architecture decisions
1. Playwright + xUnit, not BDD feature files
   
The task only requires working tests and an explanation of the approach, not Gherkin/Reqnroll integration, so the tests are code‑first:

Using xUnit keeps the stack small and idiomatic for .NET and Playwright.

Readable test names (e.g. Valid_credentials_should_display_secure_area_page) and Given/When/Then‑style comments provide BDD flavour without the overhead of .feature files.

This keeps the solution easy to run, fast, and straightforward to explain in an interview while still showing design thinking.

2. Page Object Model (POM)
The test code is kept thin by moving UI details into Page Objects:

LoginPage

Navigates to /login.

Enters username/password.

Clicks the Login button.

Exposes the flash message text used by negative tests.

SecureAreaPage

Verifies that the secure area is loaded (URL contains /secure and the “Secure Area” heading is visible).

Clicks the Logout button.

Benefits:

Tests output behaviour, not selectors (loginPage.LoginAsync(...) instead of repeating #username, #password, etc.).

If the UI changes, most updates are localised to the Page Objects, keeping tests maintainable.

3. Shared Playwright fixture with isolation per test
The tests use an xUnit class fixture (PlaywrightFixture) to manage expensive resources.

PlaywrightFixture implements IAsyncLifetime interface to launch the browser once per test class and close it when all tests are done.
Each test calls NewPageAsync() which creates a new browser context and page, so cookies and storage do not leak between tests, matching 
Playwright’s test isolation guidance.

This strategy balances:

Performance – the browser is reused across tests instead of launching per test.
Isolation – each test still runs in its own fresh context, making failures easier to debug and preventing cascading issues.

4. Headless by default, headed for debugging
   
In CI and normal dotnet test runs, the browser runs in headless mode to maximise speed and avoid UI dependencies.
For local exploration or demo, switching Headless = false in PlaywrightFixture lets you observe the flow visually.
This mirrors a realistic workflow: headless in pipelines, headed when debugging.

6. Grouping of tests by feature
   
All login‑related tests are stored in a single LoginTests class:

Positive path:
Valid_credentials_should_display_secure_area_page

Negative paths:

Invalid_username_should_display_error_message
Invalid_password_should_display_error_message
Invalid_credentials_should_display_error_message

Post‑condition behaviour:
Logout_should_redirect_to_login_page
Tests are grouped by feature (Login) rather than by outcome (positive/negative), which is a common organisation pattern and keeps the suite discoverable.

Test scenarios covered

Positive scenario: Valid login
Given I am on the login page
When I log in with tomsmith / SuperSecretPassword!
Then I am redirected to /secure and see the “Secure Area” message

Negative scenarios:
Invalid username, valid password → error message shown, stay on /login
Valid username, invalid password → error message shown, stay on /login
Invalid username and password → generic invalid credentials error

Logout behaviour:
Given I am logged into the secure area
When I click Logout
Then I am redirected back to the login page and the login form is visible

Extensibility:
The structure is intentionally small which makes it easy to extend.
New flows can be added by creating more Page Objects (e.g. ForgotPasswordPage) and test classes.
Additional browsers (Firefox, WebKit) or devices can be added by tweaking the Launch options or by introducing configuration.

Parallel execution can be enabled at the xUnit level; each test already uses its own context, which supports safe parallelism.

Notes:
The target application is a public demo site; credentials and messages are test‑only and not sensitive.
Browser warning pop‑ups (e.g. “password found in a data breach”) come from Chrome’s own security features and are deliberately ignored, 
since they are not part of the application under test.
