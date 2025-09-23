
using OneInteg.Server.Domain.Repositories;

namespace OneInteg.Server.DataAccess
{
    public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(OneIntegDbContext context) : base(context)
        {
        }
    }
}
