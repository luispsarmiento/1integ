using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OneInteg.Server.Domain.Entities;

namespace OneInteg.Server.DataAccess
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
    }

    public class Tenant : BaseEntity
    {
        public Guid TenantId { get; set; }
        public string Name { get ; set ; }
        public DateTime UpdateAt { get ; set ; }
        public DateTime CreateAt { get ; set ; }
    }

    public class Customer : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid TenantId { get ; set ; }
        public string ExternalReference { get ; set ; }
        public string Email { get ; set ; }
        public DateTime UpdateAt { get ; set ; }
        public DateTime CreateAt { get ; set ; }
    }

    public class Subscription : BaseEntity
    {
        public Guid SubscriptionId { get; set; }
        public Guid TenantId { get; set; }
        public Guid CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
        public string Reference { get; set; }
        public string PlanReference { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
