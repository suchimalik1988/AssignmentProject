# SeleniumLoginTests

UI automation suite for `https://the-internet.herokuapp.com/login`, built with
**Selenium WebDriver + xUnit + C#**, following the **Page Object Model (POM)**.

## Prerequisites

- Visual Studio 2022 with .NET SDK 8
- Google Chrome (latest)
- (Optional) Postman — not required for this suite, listed per session prerequisites

NuGet packages (already referenced in `SeleniumLoginTests.csproj`, restore on first build):

- Microsoft.NET.Test.Sdk
- Selenium.WebDriver
- Selenium.WebDriver.ChromeDriver
- xunit
- xunit.runner.visualstudio

## Project structure

```
SeleniumLoginTests/
├── SeleniumLoginTests.csproj
├── Pages/
│   ├── BasePage.cs          # Shared explicit-wait helpers
│   ├── LoginPage.cs         # Locators + actions for /login
│   └── SecureAreaPage.cs    # Locators + actions for /secure (post-login)
└── Tests/
    ├── ChromeDriverFixture.cs  # Creates/disposes a ChromeDriver per test
    └── LoginTests.cs           # Test scenarios
```

## How to run

**Visual Studio:**
1. Open `SeleniumLoginTests.csproj` (or a solution containing it).
2. Build the solution (NuGet packages restore automatically).
3. Open **Test Explorer** (`Test` → `Test Explorer`) and click **Run All**.

**CLI:**
```bash
dotnet restore
dotnet test
```

## Test scenarios covered

| Test | Scenario | Expected result |
|---|---|---|
| `Login_WithValidCredentials_DisplaysSuccessMessage` | `tomsmith` / `SuperSecretPassword!` | Redirected to secure area, "You logged into a secure area!" shown |
| `Login_WithInvalidPassword_DisplaysErrorMessage` | Valid username, wrong password | "Your password is invalid!" shown |
| `Login_WithUnknownUsername_DisplaysErrorMessage` | Unknown username | "Your username is invalid!" shown |
| `Login_WithEmptyCredentials_DisplaysErrorMessage` | Empty fields, click Login | "Your username is invalid!" shown |

## Design notes

- **Page Object Model**: `LoginPage` and `SecureAreaPage` own all locators and
  interactions; `LoginTests` only orchestrates and asserts — no raw
  `IWebElement`/`By` calls in the test class.
- **Explicit waits only**: `BasePage` wraps `WebDriverWait` so tests aren't
  flaky from implicit-wait/explicit-wait mixing.
- **Isolation**: each test gets its own `ChromeDriver` instance (created in
  the constructor, disposed in `Dispose`), so no state or cookies leak
  between tests.
- **Headless/CI**: uncomment the `--headless=new` argument in
  `ChromeDriverFixture` to run in CI pipelines without a visible browser.
