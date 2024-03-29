﻿namespace IdentityServer4.Quickstart.UI
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
        public bool AutomaticRedirectAfterSignOut { get; set; }
        public string LogoutId { get; set; }
        public bool TriggerExternalSignOut
        {
            get { return ExternalAuthenticationScheme != null; }
        }
        public string ExternalAuthenticationScheme { get; set; }
    }
}