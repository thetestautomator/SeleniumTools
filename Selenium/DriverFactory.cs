using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace SeleniumTools.Selenium
{
    public static class DriverFactory
    {

        private static EdgeOptions GetEdgeOptions()
        {
            var options = new EdgeOptions();
            options.UseChromium = true;
            return options;
        }

        public static IWebDriver Build(Browser browser) => browser switch
        {
            Browser.CHROME => new ChromeDriver(),
            Browser.FIREFOX => new FirefoxDriver(),
            Browser.INTERNET_EXPORER => new InternetExplorerDriver(),
            Browser.EDGE => new EdgeDriver(GetEdgeOptions()),
            _ => throw new System.Exception("No valid browser")
        };
    }
}