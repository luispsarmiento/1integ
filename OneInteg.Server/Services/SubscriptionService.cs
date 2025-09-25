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
        public SubscriptionService(ISubscriptionRepository repository, ICustomerRepository customerRepository) : base(repository)
        {
            this.repository = repository;
            this.customerRepository = customerRepository;
        }

        public async Task<string> GetCheckoutUrl(Customer customer, string planReference)
        {
            try
            {
                if (!IsValidCustomer(customer.Email))
                {
                    return string.Empty;
                }

                var _customer = (await customerRepository.Find(doc => doc.Email == customer.Email)).FirstOrDefault();

                if (_customer == null) 
                {
                    customer.CustomerId = Guid.NewGuid();
                    customer.CreateAt = DateTime.Now;
                    customer.UpdateAt = DateTime.Now;
                    await customerRepository.Add(customer);
                }

                var checkoutUrl = planReference switch
                {
                    "test" => "https://www.mercadopago.com.mx/subscriptions/checkout?preapproval_plan_id=a9251f2f03d74de9bdbeeb1d811d1bf7",
                    "68d48ce89097c6a994042c0b" => "",
                    _ => ""
                };

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
