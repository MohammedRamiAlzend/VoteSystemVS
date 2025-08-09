using Domain.Common.Results;
using Domain.Entities;
using Domain.Common; // Added for Entity constraint
using Infrastructure.Repositories; // Added for IRepository

namespace Infrastructure.Repositories.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity; // New generic method

    IRepository<Admin> AdminRepository { get; }
    IRepository<AttendanceUser> AttendanceUserRepository { get; }
    IRepository<OTPCode> OTPCodeRepository { get; }
    IRepository<SystemLog> SystemLogRepository { get; }
    IRepository<User> UserRepository { get; }
    IRepository<Vote> VoteRepository { get; }
    IRepository<VoteQuestion> VoteQuestionRepository { get; }
    IRepository<VoteQuestionOption> VoteQuestionOptionRepository { get; }
    IRepository<VoteSession> VoteSessionRepository { get; }
    Task<Result<Success>> SaveChangesAsync(CancellationToken token);
}
