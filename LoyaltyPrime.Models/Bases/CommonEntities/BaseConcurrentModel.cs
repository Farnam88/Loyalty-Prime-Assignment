using System;

namespace LoyaltyPrime.Models.Bases.CommonEntities
{
    public abstract class BaseConcurrentModel : BaseModel, IConcurrentModel, IBaseCreationTimeModel
    {
        public TimeSpan RowVersion { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}