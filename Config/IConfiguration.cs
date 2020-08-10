using System.Collections.Generic;
using SeleniumTools.Selenium;

namespace SeleniumTools.Config
{
    public interface IConfiguration
    {
        Browser Browser { get; }
        string Url { get; }
        Dictionary<string, TestUser> Users { get; }
        int TimeOut { get; }
        public Dictionary<string, string> TenantIds { get; }
    }
}