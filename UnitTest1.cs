using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;

namespace MovieCatalogProject
{
    [TestFixture]
    public class MovieCatalogTests
    {

        private string BaseUrl = "http://moviecatalog-env.eba-ubyppecf.eu-north-1.elasticbeanstalk.com/";
        private static string? lastCreatedMovieTitle;
        private static string? lastCreatedMovieDescription;
        private IWebDriver driver;
        private Actions actions;

        [OneTimeSetUp]
        public void SetUp()
        {
            var chromeOptions = new ChromeOptions();


            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(chromeOptions);


            actions = new Actions(driver);

            driver.Navigate().GoToUrl(BaseUrl);

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


            driver.FindElement(By.XPath("//li[@class='nav-item'][3]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[@id='loginBtn']")).Click();
            driver.FindElement(By.Id("form2Example17")).SendKeys("test@test1.com");
            driver.FindElement(By.Id("form2Example27")).SendKeys("123456");
            driver.FindElement(By.Id("form2Example27")).SendKeys(Keys.Enter);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }


        [Test, Order(1)]
        public void AddMovieWithoutTitleTest()
        {
            
            var addMovieButton = driver.FindElement(By.XPath("//li[@class='nav-item'][2]"));
            addMovieButton.Click();

            var titleInputField = driver.FindElement(By.XPath("//input[@id='form2Example17' and @name='Title']"));
            titleInputField.SendKeys("");

            var addButton = driver.FindElement(By.XPath("//button[@class='btn warning']"));
            addButton.Click();

            var toastErrorMsg = driver.FindElement(By.XPath("//div[@class='toast-message']"));
            Assert.That(toastErrorMsg.Text.Trim(), Is.EqualTo("The Title field is required."));

        }

        [Test, Order(2)]
        public void AddMovieWithoutDescriptionTest()
            {
            var addMovieButton = driver.FindElement(By.XPath("//li[@class='nav-item'][2]"));
            addMovieButton.Click();

            var titleInputField = driver.FindElement(By.XPath("//input[@id='form2Example17' and @name='Title']"));
            titleInputField.SendKeys("Movie Title 1");

            var descriptionInputField = driver.FindElement(By.XPath("//textarea[@id='form2Example17']"));
            descriptionInputField.SendKeys("");

            var addButton = driver.FindElement(By.XPath("//button[@class='btn warning']"));
            addButton.Click();

            var toastErrorMsg = driver.FindElement(By.XPath("//div[@class='toast-message']"));
            Assert.That(toastErrorMsg.Text.Trim(), Is.EqualTo("The Description field is required."));
        }

        [Test, Order(3)]
        public void AddRandomTitleMovieWithRandomDescriptionTest()
        {
            lastCreatedMovieTitle = "MOVIE: " + GenerateRandomString(5);
            lastCreatedMovieDescription = "Description is:" + GenerateRandomString(10);

            driver.Navigate().GoToUrl($"{BaseUrl}Catalog/Add");
            
            driver.FindElement(By.CssSelector("input#form2Example17")).SendKeys(lastCreatedMovieTitle);
            Thread.Sleep(3500);
            driver.FindElement(By.CssSelector("textarea#form2Example17")).SendKeys(lastCreatedMovieDescription);

            var addButton = driver.FindElement(By.XPath("//button[@class='btn warning']"));
            addButton.Click();

            NavigateToLastPage();

            VerifyLastMovieTitle(lastCreatedMovieTitle);
        }

        [Test, Order(4)]
        public void EditOfLastAddedMovieTest()
        {
            NavigateToLastPage();
            Thread.Sleep(3500);
                        
            var movies = driver.FindElements(By.CssSelector(".col-lg-4"));
            var lastMovieTitleElement = movies.Last();
            var editButtonLastMovie = lastMovieTitleElement.FindElement(By.CssSelector("a.btn.btn-outline-success[href*='/Movie/Edit']"));
            Thread.Sleep(3500);
            editButtonLastMovie.Click();

            lastCreatedMovieTitle = "EDITED " + lastCreatedMovieTitle; // Update the lastMovieTitle
            var titleInputField = driver.FindElement(By.CssSelector("input#form2Example17"));
            titleInputField.Clear();
            titleInputField.SendKeys(lastCreatedMovieTitle);
            Thread.Sleep(3500);

            var saveChangesButton = driver.FindElement(By.CssSelector("button.btn.warning[type='submit']"));
            actions.MoveToElement(saveChangesButton).Perform();
            saveChangesButton.Click();
            Thread.Sleep(3500);

            var toastErrorMsg = driver.FindElement(By.XPath("//div[@class='toast-message']"));
            Assert.That(toastErrorMsg.Text.Trim(), Is.EqualTo("The Movie is edited successfully!"), "The movie was not edited successfully.");
        }

        [Test, Order(5)]
        public void MarkLastAddedMovieasWatchedTest()
        {
            NavigateToLastPage();            
            
            var movies = driver.FindElements(By.XPath("//div[@class='col-lg-4']"));
            var lastMovieMarkElement = movies.Last();
            var lastMovieMarkButton = lastMovieMarkElement.FindElement(By.XPath("//a[@class='btn btn-info']"));
            lastMovieMarkButton.Click();

            driver.Navigate().GoToUrl($"{BaseUrl}Catalog/Watched#watched");

            NavigateToLastPage();

            VerifyLastMovieTitle(lastCreatedMovieTitle);
        }

        [Test, Order(6)]
        public void DeleteLastMovieTest()
        {
            var allMoviesButton = driver.FindElement(By.XPath("//a[contains(@href,'Catalog/All#all')]"));
            allMoviesButton.Click();

            var pages = driver.FindElements(By.XPath("//a[@class='page-link']"));
            var lastPage = pages.Last();
            lastPage.Click();

            var movies = driver.FindElements(By.XPath("//div[@class='col-lg-4']"));
            var deleteButtonLastMovie = driver.FindElement(By.XPath("//a[@class='btn btn-danger']"));
            deleteButtonLastMovie.Click();

            driver.FindElement(By.XPath("//button[@class='btn warning']")).Click();

            var toastMessage = driver.FindElement(By.XPath("//div[@class='toast-message']"));
            Assert.That(toastMessage.Text.Trim(), Is.EqualTo("The Movie is deleted successfully!"));
        }


        private string GenerateRandomString(int length)
        {
            // Generate a random string of specified length
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void NavigateToLastPage()
        {
            // Find the pagination elements to navigate to the last page
            var paginationItems = driver.FindElements(By.CssSelector("ul.pagination li.page-item"));
            var lastPageItem = paginationItems.Last();
            actions.MoveToElement(lastPageItem).Perform();

            // Click the link of the actual last page
            var lastPageLink = lastPageItem.FindElement(By.CssSelector("a.page-link"));
            lastPageLink.Click();
        }

        private void VerifyLastMovieTitle(string expectedTitle)
        {
            // Re-locate the movie elements on the last page
            var movies = driver.FindElements(By.CssSelector(".col-lg-4"));
            var lastMovieElement = movies.Last();
            var lastMovieElementTitle = lastMovieElement.FindElement(By.CssSelector("h2"));

            // Verify that the last movie title matches the expected value
            string actualMovieTitle = lastMovieElementTitle.Text.Trim();
            Assert.That(actualMovieTitle, Is.EqualTo(expectedTitle), "The last movie title does not match the expected value.");
        }
    }
}