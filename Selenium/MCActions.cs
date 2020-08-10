using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace SeleniumTools.Selenium
{
    public class MCActions
    {
        private protected Element WaitForElementDisplayed(Element element)
        {
            try
            {
                Driver.Wait.Until(_ => element.Displayed);
                return element;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Element never got displayed");
                throw;
            }
        }

        private protected Element WaitForElementEnabled(Element element)
        {
            try
            {
                Driver.Wait.Until(_ => element.Enabled);
                return element;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Element never got enabled");
                throw;
            }
        }

        private protected Element Click(Element element, bool elementShouldBeDisplayed = true)
        {
            if (elementShouldBeDisplayed) WaitForElementDisplayed(element);
            WaitForElementEnabled(element);
            element.Click();
            return element;
        }

        private protected Element Click(By by)
        {
            var element = FindElement(by);
            Click(element);
            return element;
        }

        private protected Element ScrollToElement(Element element)
        {
            Driver.ExecuteJs("arguments[0].scrollIntoView(true);", element);
            Driver.Sleep(500);  // wait until scroll is finished
            return element;
        }

        public void ScrollContainerToTop(Element container)
        {
            Driver.ExecuteJs(
                $"document.querySelector('{container.Locator()}').scrollBy(0, -1 * document.querySelector('{container.Locator()}').scrollTop)"
                );
            Driver.Sleep(500);  // wait until scroll is finished
        }

        public void ScrollContainerToBottom(Element container)
        {
            Driver.ExecuteJs(
                $"document.querySelector('{container.Locator()}').scrollBy(0, document.querySelector('{container.Locator()}').scrollHeight)"
                );
            Driver.Sleep(500);  // wait until scroll is finished
        }

        public void ScrollToElementInsideElement(Element container, Element target)
        {
            var containerHeight = Int32.Parse(Driver.ExecuteJs($"return document.querySelector('{container.Locator()}').scrollHeight;"));
            var scrolledBy = 0;
            iterate();

            void iterate()
            {
                if (scrolledBy > containerHeight) throw new Exception("Element not found in container");

                if (!target.Displayed)
                {
                    Driver.ExecuteJs($"document.querySelector('{container.Locator()}').scrollBy(0,100);");
                    Driver.Sleep(500);  // wait until scroll is finished
                    scrolledBy += 100;
                    iterate();
                }
            }
        }

        private protected Element SendKeys(Element element, string text)
        {
            WaitForElementDisplayed(element);
            WaitForElementEnabled(element)
                .SendKeys(text);
            return element;
        }

        private protected Element SendKeys(By by, string text)
        {
            var element = FindElement(by);
            SendKeys(element, text);
            return element;
        }

        // Some tests check if an element is disabled (e.g. a save button on a page is disabled if another field is empty).
        // Some elements do not respond to element.clear(), so they will not be disabled.
        // By sending explicit deletes these elements respond and are diabled.
        private protected void Clear(By by)
        {
            var element = FindElement(by);
            WaitForElementDisplayed(element);
            WaitForElementEnabled(element);

            var text = GetValue(element);

            for (var i = 0; i < text.Length; i++)
            {
                SendDelete(element);
            }
        }

        private protected void SendDelete(Element element) =>
            element.SendKeys(Keys.Backspace);

        private protected Element FindElement(By by)
        {
            try
            {
                return new Element(by);
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(500);
                return new Element(by);
            }
        }

        private protected IList<Element> FindElements(By by)
        {
            try
            {
                return Driver.Current.FindElements(by).Select(element => new Element(element, by)).ToList();
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(500);
                return Driver.Current.FindElements(by).Select(element => new Element(element, by)).ToList();
            }
        }

        private protected bool ElementHasClass(By by, string className) =>
            FindElement(by).GetAttribute("class").Contains(className);

        private protected bool ElementExists(By by)
        {
            try
            {
                Driver.SetImplicitTimeout(1);
                FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            finally
            {
                Driver.ResetImplicitTimeout();
            }
        }

        private protected string GetValue(Element element) =>
            element.GetAttribute("value");

        private protected string GetInnerText(Element element) =>
            element.GetAttribute("innerText");

        private protected void OpenUrl(string url)
        {
            Console.WriteLine("Open url " + url);
            Driver.GoTo(url);
        }

        private protected string CurrentUrl() =>
            Driver.Current.Url;
    }
}
