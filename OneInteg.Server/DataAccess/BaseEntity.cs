using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OneInteg.Server.Domain.Entities;
using MongoDB.Bson.Serialization.Options;

namespace OneInteg.Server.DataAccess
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
    }

    public class Tenant : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid TenantId { get; set; }
        public string Name { get ; set ; }
        public TenantSettings Settings { get; set; }
        public DateTime UpdateAt { get ; set ; }
        public DateTime CreateAt { get ; set ; }
    }

    public class Customer : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid CustomerId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid TenantId { get ; set ; }
        public string Email { get ; set ; }
        public DateTime UpdateAt { get ; set ; }
        public DateTime CreateAt { get ; set ; }
    }

    public class Subscription : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid SubscriptionId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid TenantId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
        public string Reference { get; set; }
        public string PlanReference { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
    }

    public class Plan : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid TenantId { get; set; }
        public string PlanReference { get; set; }
        public string Data { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
