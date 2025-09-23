using OneInteg.Server.Domain.Entities;
using System.Text.Json;
using System.Text;

namespace OneInteg.Server.Services
{
    public class MercadoPagoPaymentProvider : IPaymentProvider
    {
        protected string BaseUri;
        protected string Key;
        protected string Secret;
        protected string BaseCurrency;
        public MercadoPagoPaymentProvider()
        {
            this.BaseUri = string.Empty;//the value come from a tenant configuration
            this.Key = string.Empty;//the value come from a tenant configuration
            this.Secret = string.Empty;//the value come from a tenant configuration
            this.BaseCurrency = string.Empty;//the value come from a tenant configuration
        }
        public async Task<Subscription> CreateSubscription(Customer customer, Subscription data)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.BaseUri);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Secret}");

            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                preapproval_plan_id = data.PlanReference,
                payer_email = customer.Email,
                card_token_id = data.PaymentMethodId,
                status = "authorized"
            }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(
                "preapproval",
                jsonContent);

            return data;
        }

        public Task<Subscription> GetSubscription(Subscription data)
        {
            throw new NotImplementedException();
        }

        public Task<Subscription?> HandleSubscription(Subscription data)
        {
            throw new NotImplementedException();
        }
    }
}
