using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace SeleniumTools.Selenium
{
    public class Element : IWebElement
    {

        private readonly IWebElement _element;
        public IWebElement Current => _element ?? throw new System.NullReferenceException("Element is null");

        public By By { get; private set; }

        public Element(IWebElement element, By by)
        {
            _element = element;
            By = by;
        }

        public Element(By by)
        {
            _element = Driver.Current.FindElement(by);
            By = by;
        }

        public string Locator()
        {
            var parts = By.ToString().Split(": ");
            if (parts[0].Contains("ClassName")) return $".{parts[1]}";
            if (parts[0].Contains("Id")) return $"#{parts[1]}";
            return parts[1];
        }

        public string TagName => Current.TagName;

        public string Text => Current.Text;

        public bool Enabled => Current.Enabled;

        public bool Selected => Current.Selected;

        public Point Location => Current.Location;

        public Size Size => Current.Size;

        public bool Displayed => Current.Displayed;

        public void Clear()
        {
            Current.Clear();
        }

        public void Click()
        {
            Current.Click();
        }

        public IWebElement FindElement(By by)
        {
            return Current.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Current.FindElements(by);
        }

        public string GetAttribute(string attributeName)
        {
            return Current.GetAttribute(attributeName);
        }

        public string GetCssValue(string propertyName)
        {
            return Current.GetCssValue(propertyName);
        }

        public string GetProperty(string propertyName)
        {
            return Current.GetProperty(propertyName);
        }

        public void SendKeys(string text)
        {
            Current.SendKeys(text);
        }

        public void Submit()
        {
            Current.Submit();
        }
    }
}