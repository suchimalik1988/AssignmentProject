using OpenQA.Selenium;

namespace SeleniumLoginTests.Pages
{
    /// <summary>
    /// Page Object for the secure area shown after a successful login
    /// (https://the-internet.herokuapp.com/secure).
    /// </summary>
    public class SecureAreaPage : BasePage
    {
        private static readonly By FlashMessage = By.Id("flash");
        private static readonly By LogoutButton = By.CssSelector("a.button.secondary");

        public SecureAreaPage(IWebDriver driver) : base(driver) { }

        public string GetFlashMessageText() => WaitForVisible(FlashMessage).Text;

        public bool IsDisplayed() => Driver.Url.Contains("/secure");

        public LoginPage Logout()
        {
            WaitForClickable(LogoutButton).Click();
            return new LoginPage(Driver);
        }
    }
}
