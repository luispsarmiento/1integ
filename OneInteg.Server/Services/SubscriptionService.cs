using OneInteg.Server.Domain.Entities;
using OneInteg.Server.Domain.Repositories;
using OneInteg.Server.Domain.Services;

namespace OneInteg.Server.Services
{
    public class SubscriptionService : BaseService<Subscription>, ISubscriptionService
    {
        public SubscriptionService(IBaseRepository<Subscription> repository) : base(repository)
        {
        }

        public Task<string> GetCheckoutUrl(Tenant tenant, Customer customer, string planReference)
        {
            throw new NotImplementedException();
        }
    }
}
