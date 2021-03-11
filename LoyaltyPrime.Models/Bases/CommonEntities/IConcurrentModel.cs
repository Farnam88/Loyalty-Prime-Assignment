using System;

namespace LoyaltyPrime.Models.Bases.CommonEntities
{
    public interface IConcurrentModel
    {
        TimeSpan RowVersion { get; set; }
    }
}