using Application.Features.VoteGroup.Common;
using Application.Features.VoteGroup.Queries.GetAll;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.VoteGroup.Queries.GetAll.ForSession
{
    public class VotingProfile : Profile
    {
        public VotingProfile()
        {
            // Vote -> VoteDTO
            CreateMap<Vote, VoteDTO>();

            // VoteQuestionOption -> QuestionOptionDTO
            CreateMap<VoteQuestionOption, QuestionOptionDTO>()
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes));

            // VoteQuestion -> QuestionDTO
            CreateMap<VoteQuestion, QuestionDTO>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            // VoteSession -> VotesForSessionDTO
            CreateMap<VoteSession, VotesForSessionDTO>()
                .ForMember(dest => dest.VoteSessionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
        }
    }
}
