using System;
using System.Collections.Generic;
using System.Linq;
using TestPlanTools.Extensions;

namespace SeleniumTools.Selenium
{
    public class BrowserTabs
    {
        [ThreadStatic]
        public readonly List<BrowserTab> Tabs;

        public BrowserTabs(string url)
        {
            Tabs = new List<BrowserTab>();
            AddBrowserTab(Driver.Current.CurrentWindowHandle, url, "Initial tab", true, true);
        }

        public void OpenNewTab(string url, string description = null)
        {
            Driver.ExecuteJs("window.open();");
            foreach (var tab in Tabs)
                tab.Selected = false;
            var newTab = AddBrowserTab(Driver.Current.WindowHandles[Driver.Current.WindowHandles.Count - 1], url, description);
            Console.WriteLine("HANDLES " + ToJson());
            SwitchToTab(newTab.Handle, SwitchTabType.Handle);
            Driver.GoTo(url);
        }

        public void SwitchToInitialTab()
        {
            var handle = Tabs.First(tab => tab.IsInitialTab).Handle;
            Switch(handle);
        }

        // If close is true the reason for the switch is the closing of a tab => no tab with Selected == true exists
        private void Switch(string handle, bool close = false)
        {
            if (!close) Tabs.First(t => t.Selected).Selected = false;
            Driver.Current.SwitchTo().Window(handle);
            Tabs.First(t => t.Handle.Equals(handle)).Selected = true;
        }

        public void SwitchToTab(int index, bool close = false)
        {
            if (index >= Tabs.Count) throw new Exception("Try to access not existing tab");
            Switch(Tabs[index].Handle, close);
        }

        public void SwitchToTab(string value, SwitchTabType switchTabType)
        {
            var handle = "";

            switch (switchTabType)
            {
                case SwitchTabType.Url:
                    handle = Tabs.First(tab => tab.CurrentUrl == value).Handle;
                    break;
                case SwitchTabType.Handle:
                    handle = Tabs.First(tab => tab.Handle == value).Handle;
                    break;
                case SwitchTabType.Description:
                    handle = Tabs.First(tab => tab.Description == value).Handle;
                    break;
            }
            if (String.IsNullOrEmpty(handle)) throw new Exception($"No tab found for {switchTabType} - {value}");

            Switch(handle);
        }

        private BrowserTab AddBrowserTab(string handle, string url, string description = null, bool selected = true, bool isInitialTab = false)
        {
            var tab = new BrowserTab
            {
                Handle = handle,
                CurrentUrl = url,
                Selected = true,
                Description = description,
                IsInitialTab = isInitialTab
            };
            Tabs.Add(tab);
            return tab;
        }

        public void ChangeCurrentUrl(string url, string description = null)
        {
            var tab = Tabs.First(t => t.Selected);
            tab.CurrentUrl = url;
            if (!String.IsNullOrEmpty(description)) tab.Description = description;
        }

        public void CloseCurrentTab()
        {
            Driver.Current.Close();
            var removedTab = Tabs.First(t => t.Selected);
            Tabs.Remove(removedTab);
            SwitchToTab(0, true);
            Tabs.First(t => t.Handle.Equals(Driver.Current.CurrentWindowHandle)).Selected = true;
        }

        public string ToJson() =>
            Tabs.ToJson();
    }
}