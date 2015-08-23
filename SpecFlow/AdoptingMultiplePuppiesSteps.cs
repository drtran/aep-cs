using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using Module1;

namespace SpecFlow
{
    [Binding]
    public class AdoptingMultiplePuppiesSteps
    {
        private string homePage;

        [Given(@"that I am at the website ""(.*)""")]
        public void GivenThatIAmAtTheWebsite(string url)
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            homePage = driver.Url;
            ScenarioContext.Current.Add("driver", driver);
            Assert.AreEqual("Sally's Puppy Adoption Agency", driver.Title);
        }

        [When(@"I adopt for these pets,")]
        public void WhenIAdoptForThesePets(TechTalk.SpecFlow.Table petNames)
        {
            IWebDriver driver = ScenarioContext.Current["driver"] as IWebDriver;

            for (int i = 0; i < petNames.RowCount; i++)
            {
                ReadOnlyCollection<IWebElement> nameLabels = driver.FindElements(By.ClassName("name"));
                ReadOnlyCollection<IWebElement> viewDetailsButtons = driver.FindElements(By.XPath("//input[@value='View Details']"));

                var petName = petNames.Rows[i][0];
                Console.WriteLine("taking care of " + petName);
                for (int j = 0; j < nameLabels.Count; j++)
                {
                    if (nameLabels[j].Text == petName)
                    {
                        Console.WriteLine("FOUND: " + petName);
                        viewDetailsButtons[j].Click();
                        IWebElement adoptMeButton = driver.FindElement(By.XPath("//input[@value='Adopt Me!']"));
                        adoptMeButton.Click();
                        if (petNames.RowCount == i + 1)
                        {
                            IWebElement completeTheAdoptionButton = driver.FindElement(By.XPath("//input[@value='Complete the Adoption']"));
                            completeTheAdoptionButton.Click();
                        }
                        else
                        {
                            IWebElement adoptAnotherButton = driver.FindElement(By.XPath("//input[@value='Adopt Another Puppy']"));
                            adoptAnotherButton.Click();
                        }
                        break;
                    }
                }

            }

        }

        [When(@"I pay for the adoption using this type of payment:")]
        public void WhenIPayForTheAdoptionUsingThisTypeOfPayment(Table table)
        {
            var paymentInfo = new PaymentInfo
            {
                paymentType = table.Rows[0][0],
                orderName = table.Rows[0][1],
                orderAddress = table.Rows[0][2],
                orderEmail = table.Rows[0][3]
            };

            IWebDriver driver = ScenarioContext.Current["driver"] as IWebDriver;
            IWebElement orderNameField = driver.FindElement(By.Id("order_name"));
            IWebElement orderAddressField = driver.FindElement(By.Id("order_address"));
            IWebElement orderEmailField = driver.FindElement(By.Id("order_email"));
            IWebElement orderPayTypeElement = driver.FindElement(By.Id("order_pay_type"));
            IWebElement placeOrderButton = driver.FindElement(By.XPath("//input[@value='Place Order']"));
            orderNameField.SendKeys(paymentInfo.orderName);
            orderAddressField.SendKeys(paymentInfo.orderAddress);
            orderEmailField.SendKeys(paymentInfo.orderEmail);
            SelectElement select = new SelectElement(orderPayTypeElement);
            foreach (IWebElement option in select.Options)
            {
                if (option.Text == paymentInfo.paymentType)
                {
                    option.Click();
                    break;
                }
            }
            placeOrderButton.Click();
        }

        [Then(@"I should be back at the main page with a thank you note, ""(.*)""")]
        public void ThenIShouldBeBackAtTheMainPageWithAThankYouNote(string thankyouMsg)
        {
            IWebDriver driver = ScenarioContext.Current["driver"] as IWebDriver;
            IWebElement noticeLabel = driver.FindElement(By.Id("notice"));
            Assert.AreEqual(thankyouMsg, noticeLabel.Text);
            driver.Dispose();
        }
    }
}
