namespace _1Integ.Server.Entities;

public class Subscription
{
    public Guid SubscriptionId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public string Reference { get; set; }
    public string PlanReference { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime CreateAt { get; set; }
}