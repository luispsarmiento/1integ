
using OneInteg.Server.Domain.Repositories;

namespace OneInteg.Server.DataAccess
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OneIntegDbContext context) : base(context)
        {
        }
    }
}
