using SeleniumLoginTests.Pages;
using Xunit;

namespace SeleniumLoginTests.Tests
{
    /// <summary>
    /// UI automation suite for the-internet.herokuapp.com/login.
    /// A fresh ChromeDriver instance is created for every test (via the
    /// constructor) and torn down afterwards (via Dispose), so tests never
    /// leak browser state into one another.
    /// </summary>
    public class LoginTests : IDisposable
    {
        private readonly ChromeDriverFixture _fixture;
        private readonly LoginPage _loginPage;

        private const string ValidUsername = "tomsmith";
        private const string ValidPassword = "SuperSecretPassword!";
        private const string ExpectedSuccessMessage = "You logged into a secure area!";

        public LoginTests()
        {
            _fixture = new ChromeDriverFixture();
            _loginPage = new LoginPage(_fixture.Driver);
        }

        [Fact]
        [Trait("Category", "UI")]
        [Trait("Feature", "Login")]
        public void Login_WithValidCredentials_DisplaysSuccessMessage()
        {
            // Arrange / Act
            SecureAreaPage secureArea = _loginPage.LoginAs(ValidUsername, ValidPassword);

            // Assert
            Assert.True(secureArea.IsDisplayed(), "Expected to be redirected to the secure area after login.");
            Assert.Contains(ExpectedSuccessMessage, secureArea.GetFlashMessageText());
        }

        [Fact]
        [Trait("Category", "UI")]
        [Trait("Feature", "Login")]
        public void Login_WithInvalidPassword_DisplaysErrorMessage()
        {
            // Act
            _loginPage.NavigateTo();
            _loginPage.EnterUsername(ValidUsername);
            _loginPage.EnterPassword("WrongPassword123!");
            _loginPage.ClickLogin();

            // Assert — invalid creds keep the user on the login page
            Assert.Contains("Your password is invalid!", _loginPage.GetFlashMessageText());
        }

        [Fact]
        [Trait("Category", "UI")]
        [Trait("Feature", "Login")]
        public void Login_WithUnknownUsername_DisplaysErrorMessage()
        {
            // Act
            _loginPage.NavigateTo();
            _loginPage.EnterUsername("not_a_real_user");
            _loginPage.EnterPassword(ValidPassword);
            _loginPage.ClickLogin();

            // Assert
            Assert.Contains("Your username is invalid!", _loginPage.GetFlashMessageText());
        }

        [Fact]
        [Trait("Category", "UI")]
        [Trait("Feature", "Login")]
        public void Login_WithEmptyCredentials_DisplaysErrorMessage()
        {
            // Act
            _loginPage.NavigateTo();
            _loginPage.ClickLogin();

            // Assert
            Assert.Contains("Your username is invalid!", _loginPage.GetFlashMessageText());
        }

        public void Dispose()
        {
            _fixture.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
