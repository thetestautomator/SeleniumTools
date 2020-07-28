using System.Security;

namespace SeleniumTools.Config
{
    public class User
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
    }
}