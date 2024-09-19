namespace BugNetCore.Services.DataServices.Settings
{
    public class GitHubSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }

        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInformationEndpoint { get; set; }
    }
}
