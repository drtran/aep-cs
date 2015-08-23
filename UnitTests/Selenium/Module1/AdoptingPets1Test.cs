using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace UnitTests.Selenium.Module1
{
    [TestFixture]
    class AdoptingPets1Test
    {
        private const string ONLINE_PUPPIES_ADOPTION_SITE = "http://puppies.herokuapp.com";
        private const string LOCAL_PUPPIES_ADOPTION_SITE = "http://localhost:3000";
        private const string PHANTOMJS_DRIVER_URL = "http://localhost:8910";
        private const string REMOTE_DRIVER_URL = "http://localhost:4444/wd/hub";
        private IWebDriver driver;

        [Test]
        public void ShouldAdoptWithFirefoxDriver()
        {
            driver = new FirefoxDriver();
            Adopt_a_pet();
        }

        [Test]
        public void ShouldAdoptWithChromeDriver()
        {
            driver = new ChromeDriver();
            Adopt_a_pet();
        }

        private void Adopt_a_pet()
        {

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl(ONLINE_PUPPIES_ADOPTION_SITE);

            ReadOnlyCollection<IWebElement> names = driver.FindElements(By.XPath("//div[@class='name']"));
            ReadOnlyCollection<IWebElement> values = driver.FindElements(By.XPath("//input[@value='View Details']"));

            int index = 0;
            foreach (IWebElement name in names)
            {
                if (name.Text.Equals("Brook"))
                {
                    values[index].Click();
                    break;
                }
                index++;
            }

            IWebElement adoptMe = driver.FindElement(By.XPath("//input[@value='Adopt Me!']"));
            adoptMe.Click();

            IWebElement completeTheAdoption = driver.FindElement(By.XPath("//input[@value='Complete the Adoption']"));
            completeTheAdoption.Click();

            IWebElement orderName = driver.FindElement(By.XPath("//input[@id='order_name']"));
            orderName.SendKeys("Cheezy");

            IWebElement orderAddr = driver.FindElement(By.XPath("//textarea[@id='order_address']"));
            orderAddr.SendKeys("123 Main Street");

            IWebElement orderEmail = driver.FindElement(By.XPath("//input[@id='order_email']"));
            orderEmail.SendKeys("cheezy@example.com");

            IWebElement paymentType = driver.FindElement(By.XPath("//select[@id='order_pay_type']"));
            SelectElement select = new SelectElement(paymentType);

            foreach (IWebElement option in select.Options)
            {
                if (option.Text.Equals("Check"))
                {
                    option.Click();
                    break;
                }
            }

            IWebElement commit = driver.FindElement(By.XPath("//input[@name='commit']"));
            commit.Click();

            IWebElement thankYouNote = driver.FindElement(By.XPath("//p[@id='notice']"));
            Assert.AreEqual("Thank you for adopting a puppy!", thankYouNote.Text);
        }


        /** SetUp & TearDown code **/
        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }
    }
}
