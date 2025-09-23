
using OneInteg.Server.Domain.Repositories;

namespace OneInteg.Server.DataAccess
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(OneIntegDbContext context) : base(context)
        {
        }
    }
}
