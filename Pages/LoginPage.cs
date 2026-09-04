using OpenQA.Selenium;

namespace SeleniumLoginTests.Pages
{
    /// <summary>
    /// Page Object for https://the-internet.herokuapp.com/login
    /// Encapsulates all locators and interactions for the login page so
    /// tests never talk to Selenium directly.
    /// </summary>
    public class LoginPage : BasePage
    {
        private const string Url = "https://the-internet.herokuapp.com/login";

        // Locators
        private static readonly By UsernameField = By.Id("username");
        private static readonly By PasswordField = By.Id("password");
        private static readonly By LoginButton = By.CssSelector("button[type='submit']");
        private static readonly By FlashMessage = By.Id("flash");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public LoginPage NavigateTo()
        {
            Driver.Navigate().GoToUrl(Url);
            return this;
        }

        public LoginPage EnterUsername(string username)
        {
            var field = WaitForVisible(UsernameField);
            field.Clear();
            field.SendKeys(username);
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            var field = WaitForVisible(PasswordField);
            field.Clear();
            field.SendKeys(password);
            return this;
        }

        public SecureAreaPage ClickLogin()
        {
            WaitForClickable(LoginButton).Click();
            return new SecureAreaPage(Driver);
        }

        /// <summary>
        /// Convenience method chaining the full login flow in one call.
        /// </summary>
        public SecureAreaPage LoginAs(string username, string password)
        {
            NavigateTo();
            EnterUsername(username);
            EnterPassword(password);
            return ClickLogin();
        }

        /// <summary>
        /// Reads the flash message without navigating away — useful for
        /// negative test cases (invalid credentials) that stay on this page.
        /// </summary>
        public string GetFlashMessageText() => WaitForVisible(FlashMessage).Text;
    }
}
