using Application.Features.Auth.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login.UserLogin.WithEmail
{
    public record UserLoginWithMagicLinkCommand(string Token) : IRequest<Result<MagicLinkLoginResponse>>;

    public class UserLoginWithMagicLinkCommandHandler : IRequestHandler<UserLoginWithMagicLinkCommand, Result<MagicLinkLoginResponse>>
    {
        private readonly IUnitOfWork _repo;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<UserLoginWithMagicLinkCommandHandler> _logger;

        public UserLoginWithMagicLinkCommandHandler(
            IUnitOfWork repo,
            IJwtTokenService jwtTokenService,
            ILogger<UserLoginWithMagicLinkCommandHandler> logger)
        {
            _repo = repo;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<Result<MagicLinkLoginResponse>> Handle(UserLoginWithMagicLinkCommand request, CancellationToken cancellationToken)
        {
            var tokenResult = await _repo.VoteSessionMagicLinkTokenRepository.GetByFilterAsync(x => x.Token == request.Token, include: i => i.Include(x => x.AttendanceUser).ThenInclude(x => x.User));
            if (tokenResult.IsError || tokenResult.Value == null)
            {
                _logger.LogWarning("Invalid or missing magic link token: {Token}", request.Token);
                return Error.NotFound("Invalid or expired magic link");
            }
            var magicLinkToken = tokenResult.Value;
            if (magicLinkToken.IsUsed || magicLinkToken.ExpiredAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Magic link token expired or already used: {Token}", request.Token);
                return Error.Validation("Magic link expired or already used");
            }
            var user = magicLinkToken.AttendanceUser.User;
            var token = _jwtTokenService.GenerateToken(user.Id.ToString(), "User", user.FullName, user.PhoneNumber, user.Email);
            magicLinkToken.IsUsed = true;
            await _repo.VoteSessionMagicLinkTokenRepository.UpdateAsync(magicLinkToken);
            await _repo.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User {UserId} logged in with magic link token", user.Id);
            return new MagicLinkLoginResponse
            {
                Token = token,
                VoteSessionId = magicLinkToken.VoteSessionId,
                UserName = user.FullName,
                Email = user.Email
            };
        }
    }
}
