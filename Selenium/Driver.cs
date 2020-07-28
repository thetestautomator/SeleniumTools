using System;
using System.Threading;
using OpenQA.Selenium;
using SeleniumTools.Config;
using TestPlanTools.Extensions;

namespace SeleniumTools.Selenium
{
    public class Driver
    {
        [ThreadStatic]
        private static IWebDriver _driver;

        [ThreadStatic]
        public static Wait Wait;

        [ThreadStatic]
        public static LocalStorage LocalStorage;

        [ThreadStatic]
        public static SessionStorage SessionStorage;

        [ThreadStatic]
        private static IConfiguration _config;

        [ThreadStatic]
        public static BrowserTabs BrowserTabs;

        private static string _baseUrl;

        public static IWebDriver Current => _driver ?? throw new NullReferenceException("WebDriver is null");

        public static string BaseUrl => _baseUrl ?? throw new NullReferenceException("BaseUrl is null");

        public static string Title => Current.Title;

        public static void Init(IConfiguration config)
        {
            _config = config;
            _driver = DriverFactory.Build(config.Browser);
            Wait = new Wait(config.TimeOut);
            SetImplicitTimeout(config.TimeOut);
            SessionStorage = new SessionStorage();
            LocalStorage = new LocalStorage();
            _baseUrl = config.Url;
            BrowserTabs = new BrowserTabs(_baseUrl);
        }

        public static void Quit()
        {
            Current.Quit();
            Current.Dispose();
        }

        public static void GoTo(string path, string description = null)
        {
            var url = "";
            if (path.StartsWith("http"))
                url = path;
            else
                url = _baseUrl.UrlAppend(path);
            Current.Navigate().GoToUrl(url);
            BrowserTabs.ChangeCurrentUrl(url, description);
        }

        public static void SetImplicitTimeout(int timeOutInSeconds) =>
            Current.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutInSeconds);

        public static void ResetImplicitTimeout() =>
            Current.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.TimeOut);

        public static string ExecuteJs(string command, object[] parameters) =>
        ((IJavaScriptExecutor)Current).ExecuteScript(command, parameters).ToString();

        public static string ExecuteJs(string command)
        {
            try
            {
                return ((IJavaScriptExecutor)Current).ExecuteScript(command).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetUserAgent() =>
            ExecuteJs("return navigator.userAgent");

        public static void Sleep(int millis) =>
            Thread.Sleep(millis);
    }
}
