using System;

namespace LoyaltyPrime.Models.Bases.CommonEntities
{
    public abstract class BaseCreationModel : BaseModel, IBaseCreationTimeModel
    {
        public DateTime CreateDateTime { get; set; }
    }
}