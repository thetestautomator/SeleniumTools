using System.Security;

namespace SeleniumTools.Config
{
    public class TestUser
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
    }
}