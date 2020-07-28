namespace SeleniumTools.Selenium
{
    public class BrowserTab
    {
        public string Handle { get; set; }
        public string CurrentUrl { get; set; }
        public bool Selected { get; set; }
        public string Description { get; set; }
        public bool IsInitialTab { get; set; }
    }
}