using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
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

    public async Task<Result<Success>> SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        return Result.Success;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
