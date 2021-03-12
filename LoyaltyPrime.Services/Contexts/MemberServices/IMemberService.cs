﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;

namespace LoyaltyPrime.Services.Contexts.MemberServices
{
    public interface IMemberService
    {
        Task<IList<MemberDto>> Members();
    }
}