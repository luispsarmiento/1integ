namespace OneInteg.Server.Domain.Entities
{
    public class Tenant
    {
        public Guid TenantId {  get; set; }
        public string Name { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
