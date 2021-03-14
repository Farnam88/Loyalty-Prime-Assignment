using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.SearchServices.Dto;

namespace LoyaltyPrime.Services.Common.Base
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            CreateMap<Member, MemberSearchDro>()
                .ForMember(des => des.Id, op => op.MapFrom(src => src.Id))
                .ForMember(des => des.Name, op => op.MapFrom(src => src.Name))
                .ForMember(des => des.Address, op => op.MapFrom(src => src.Address))
                .ForMember(des => des.Accounts,
                    op => op.MapFrom(src => src.Accounts));

            CreateMap<Account, AccountSearchDto>()
                .ForMember(des => des.AccountId, op => op.MapFrom(src => src.Id))
                .ForMember(des => des.Balance, op => op.MapFrom(src => src.Balance))
                .ForMember(des => des.Status, op => op.MapFrom(src => src.AccountStatus.ToString().ToUpper()))
                .ForMember(des => des.CompanyName, op => op.MapFrom(src => src.Company.Name));
        }
    }
}