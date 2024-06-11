namespace MyGarden_API.ViewModels
{
    public class AuthResponseViewModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
