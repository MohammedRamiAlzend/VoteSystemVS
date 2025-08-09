using Application.Features.VoteSessionGroup.Common;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteSessionGroup
{
    public class VoteSessionMapper :  Profile
    {
        public VoteSessionMapper()
        {
            CreateMap<VoteSession, VoteSessionDto>();
        }
    }
}
