namespace OneInteg.Server.Domain.Entities;

public class Customer
{
    public Guid CustomerId { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime CreateAt { get; set; }
}