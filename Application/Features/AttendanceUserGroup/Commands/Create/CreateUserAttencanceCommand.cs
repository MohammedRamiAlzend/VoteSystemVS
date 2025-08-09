using Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AttendanceUserGroup.Commands.Create
{
    public record CreateUserAttencanceCommand(int VoteSessionId, int UserId) : IRequest<Result<Created>>;
}
