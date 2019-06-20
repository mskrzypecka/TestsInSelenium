using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace TestsInSelenium
{
    class Tests
    {
        // plik był za duży, cała solucja jest na Git'cie:
        // https://github.com/mskrzypecka/TestsInSelenium
        // Pozdrawiam
        // M.Skrzypecka

        IWebDriver driver;
        
        private const string JUNIOR = "Junior";
        private const string GNOME = "Skrzat";
        private const string YOUNG = "Mlodzik";
        private const string ADULT = "Dorosly";
        private const string SENIOR = "Senior";

        private const string NO_QUALIFICATION = "Brak kwalifikacji";
        private const string ERROR = "Blad danych";

        private const string NAME = "Malgorzata";
        private const string SURNAME = "Skrzypecka";

        [SetUp]
        public void StartBrowser()
        {
            driver = new ChromeDriver(@"C:\Users\Malgorzata\Downloads\chromedriver_win32");
            driver.Url = @"https://lamp.ii.us.edu.pl/~mtdyd/zawody/";
            var Wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            Wait.Until(c => c.FindElement(By.Id("formma")));
        }

        [TearDown]
        public void CloseBrowser()
            => driver.Close();

        [Test]
        public void NoQualification_9()
            => _run(9, false, false, NO_QUALIFICATION);

        [Test]
        public void Gnome_10()
            => _run(10, true, true, GNOME);
        
        [Test]
        public void Gnome_12()
            => _run(11, true, true, GNOME);

        [Test]
        public void Young_12()
            => _run(12, true, true, YOUNG);

        [Test]
        public void Young_13()
            => _run(13, true, true, YOUNG);

        [Test]
        public void Junior_14()
            => _run(14, true, true, JUNIOR);

        [Test]
        public void Junior_17()
            => _run(17, true, true, JUNIOR);

        [Test]
        public void Adult_18()
            => _run(18, false, false, ADULT);

        [Test]
        public void Adult_64()
            => _run(64, false, false, ADULT);

        [Test]
        public void Senior_65_Doctor()
            => _run(65, false, true, SENIOR);

        [Test]
        public void Junior_15()
            => _run(15, true, true, JUNIOR);

        [Test]
        public void Error_15_NoParent()
            => _run(15, false, true, ERROR);
        
        [Test]
        public void Error_15_NoParentAndDoctor()
            => _run(15, false, false, ERROR);

        [Test]
        public void Senior_70_NoParent()
            => _run(70, false, true, SENIOR);

        [Test]
        public void Error_70_NoDoctor()
            => _run(70, false, false, ERROR);

        private void _run(int age, bool isParentalPermission, bool isDoctorsPermission, string expectedResult)
        {
            driver.FindElement(By.Id("inputEmail3")).SendKeys(NAME);
            driver.FindElement(By.Id("inputPassword3")).SendKeys(SURNAME);
            driver.FindElement(By.Id("dataU")).SendKeys(_getBirthDayDateAsString(age));

            if (isParentalPermission)
                driver.FindElement(By.Id("rodzice")).Click();

            if (isDoctorsPermission)
                driver.FindElement(By.Id("lekarz")).Click();

            driver.FindElement(By.CssSelector("button")).Click();

            Assert.AreEqual(expectedResult, _getMessage());
        }

        private string _getBirthDayDateAsString(int age) 
            => DateTime.Now.AddYears(-(age + 1)).ToString("dd-MM-yyyy");

        private string _getMessage()
        {
            try
            {
                driver.SwitchTo().Alert().Dismiss();

                string message = driver.SwitchTo().Alert().Text;

                driver.SwitchTo().Alert().Dismiss();

                return message;
            }
            catch (NoAlertPresentException e)
            {
                return e.Message;
            }
        }
    }
}
