using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumLoginTests.Tests
{
    /// <summary>
    /// IDisposable fixture that spins up a fresh ChromeDriver instance and
    /// guarantees it's quit/disposed even if a test fails or throws.
    /// Implements xUnit's IClassFixture pattern so it can be shared or
    /// per-test depending on how it's wired into the test class.
    /// </summary>
    public class ChromeDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; }

        public ChromeDriverFixture()
        {
            var options = new ChromeOptions();

            // Uncomment for CI / headless execution:
            // options.AddArgument("--headless=new");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; // rely on explicit waits only
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
