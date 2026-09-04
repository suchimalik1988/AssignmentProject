using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumLoginTests.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver, TimeSpan? timeout = null)
        {
            Driver = driver;
            Wait = new WebDriverWait(Driver, timeout ?? TimeSpan.FromSeconds(10));

            Wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException));
        }

        protected IWebElement WaitForVisible(By locator) =>
            Wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed ? element : null;
            });

        protected IWebElement WaitForClickable(By locator) =>
            Wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            });
    }
}