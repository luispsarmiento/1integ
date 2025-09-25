using MongoDB.Bson.IO;
using MongoDB.Bson;
using OneInteg.Server.DataAccess;
using OneInteg.Server.Domain.Repositories;
using OneInteg.Server.Domain.Services;
using System.Net.Mail;
using System.Numerics;

namespace OneInteg.Server.Services
{
    public partial class SubscriptionService : BaseService<Subscription>, ISubscriptionService
    {
        protected readonly ISubscriptionRepository repository;

        protected readonly ICustomerRepository customerRepository;
        protected readonly IPlanRepository planRepository;
        public SubscriptionService(ISubscriptionRepository repository, ICustomerRepository customerRepository, IPlanRepository planRepository) : base(repository)
        {
            this.repository = repository;
            this.customerRepository = customerRepository;
            this.planRepository = planRepository;
        }

        public async Task<string> GetCheckoutUrl(Customer customer, string planReference)
        {
            try
            {
                if (!IsValidCustomer(customer.Email))
                {
                    return string.Empty;
                }

                var _customer = (await customerRepository.Find(doc => doc.Email == customer.Email &&
                                                                      doc.TenantId == customer.TenantId)).FirstOrDefault();

                if (_customer == null) 
                {
                    customer.CustomerId = Guid.NewGuid();
                    customer.CreateAt = DateTime.Now;
                    customer.UpdateAt = DateTime.Now;
                    await customerRepository.Add(customer);
                }

                var plan = (await planRepository.Find(doc => doc.PlanReference == planReference &&
                                                             doc.TenantId == customer.TenantId)).FirstOrDefault();

                if (plan == null || (plan.Data is null))
                {
                    return string.Empty;
                }
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(plan.Data);
                var checkoutUrl = Convert.ToString(data.init_point);

                return checkoutUrl;
            }
            catch (Exception ee)
            {
                throw;
            }
        }
        private bool IsValidCustomer(string customer)
        {
            try
            {
                var mailAddress = new MailAddress(customer);
                return true; // Format is valid
            }
            catch (FormatException)
            {
                return false; // Format is invalid
            }
        }
    }
}
