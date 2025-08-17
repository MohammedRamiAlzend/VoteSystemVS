using Domain.Common;
using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repos = [];

    private readonly AppDbContext _context;
    private IRepository<Admin> _adminRepository;
    private IRepository<AttendanceUser> _attendanceUserRepository;
    private IRepository<OTPCode> _otpCodeRepository;
    private IRepository<SystemLog> _systemLogRepository;
    private IRepository<User> _userRepository;
    private IRepository<Vote> _voteRepository;
    private IRepository<VoteQuestion> _voteQuestionRepository;
    private IRepository<VoteQuestionOption> _voteQuestionOptionRepository;
    private IRepository<VoteSession> _voteSessionRepository;
    private IRepository<VoteSessionMagicLinkToken> _voteSessionMagicLinkTokenRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<Admin> AdminRepository => _adminRepository ??= new Repository<Admin>(_context);
    public IRepository<AttendanceUser> AttendanceUserRepository => _attendanceUserRepository ??= new Repository<AttendanceUser>(_context);
    public IRepository<OTPCode> OTPCodeRepository => _otpCodeRepository ??= new Repository<OTPCode>(_context);
    public IRepository<SystemLog> SystemLogRepository => _systemLogRepository ??= new Repository<SystemLog>(_context);
    public IRepository<User> UserRepository => _userRepository ??= new Repository<User>(_context);
    public IRepository<Vote> VoteRepository => _voteRepository ??= new Repository<Vote>(_context);
    public IRepository<VoteQuestion> VoteQuestionRepository => _voteQuestionRepository ??= new Repository<VoteQuestion>(_context);
    public IRepository<VoteQuestionOption> VoteQuestionOptionRepository => _voteQuestionOptionRepository ??= new Repository<VoteQuestionOption>(_context);
    public IRepository<VoteSession> VoteSessionRepository => _voteSessionRepository ??= new Repository<VoteSession>(_context);
    public IRepository<VoteSessionMagicLinkToken> VoteSessionMagicLinkTokenRepository => _voteSessionMagicLinkTokenRepository ??= new Repository<VoteSessionMagicLinkToken>(_context);

    public async Task<Result<Success>> SaveChangesAsync(CancellationToken token)
    {
        try
        {
            await _context.SaveChangesAsync(token);
            return Result.Success;
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
    {
        if (_repos.ContainsKey(typeof(TEntity)))
            return (IRepository<TEntity>)_repos[typeof(TEntity)];
        IRepository<TEntity> newRepo = new Repository<TEntity>(_context);
        _repos.Add(typeof(TEntity), newRepo);
        return newRepo;
    }
}
