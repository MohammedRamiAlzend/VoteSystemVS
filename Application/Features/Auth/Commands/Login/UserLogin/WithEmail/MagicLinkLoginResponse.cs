namespace Application.Features.Auth.Commands.Login.UserLogin.WithEmail
{
    public class MagicLinkLoginResponse
    {
        public string Token { get; set; }
        public int VoteSessionId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
