namespace GRC_NewClientPortal.Models
{
    public class ClienthomeModel
    {
        public string Signon { get; set; }
        public string LName { get; set; }
        public string Password { get; set; }
        public string MFACode { get; set; }

        public string Message { get; set; }
        public string ErrorMessage { get; set; }

        // This flag controls whether to show login or MFA panel
        public bool ShowMFA { get; set; } = false;
        public string? BrowserVersion { get; internal set; }
    }
}
