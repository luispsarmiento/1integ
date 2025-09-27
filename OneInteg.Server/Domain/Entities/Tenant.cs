namespace OneInteg.Server.Domain.Entities
{
    public class Tenant
    {
        public Guid TenantId {  get; set; }
        public string Name { get; set; }
        public TenantSettings Settings { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
    }

    public class TenantSettings
    {
        public string BackUrl { get; set; }
        public string WebhookSecretKey { get; set; }
        public string WebhookUrl { get; set; }
    }
}
