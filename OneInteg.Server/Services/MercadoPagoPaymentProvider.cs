﻿using OneInteg.Server.Domain.Entities;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using OneInteg.Server.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace OneInteg.Server.Services
{
    public class MercadoPagoPaymentProvider : IPaymentProvider
    {
        protected readonly ICustomerRepository _customerRepository;
        protected readonly ISubscriptionRepository _subscriptionRepository;

        protected string BaseUri;
        protected string AccessToken;
        public MercadoPagoPaymentProvider(ICustomerRepository customerRepository, ISubscriptionRepository subscriptionRepository, string baseUri, string accessToken)
        {
            this._customerRepository = customerRepository;
            this._subscriptionRepository = subscriptionRepository;

            this.BaseUri = baseUri;
            this.AccessToken = accessToken;
        }
        public async Task<Subscription?> HandleBackUrlSubscription(Guid tenantId, string preapprovalId, string customerEmail)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.BaseUri);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.AccessToken}");

            using HttpResponseMessage response = await httpClient.GetAsync($"preapproval/{preapprovalId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            var _response = JsonConvert.DeserializeObject<PreapprovalResponse>(json);

            using var customerHttpClient = new HttpClient();
            customerHttpClient.BaseAddress = new Uri($"{this.BaseUri}v1/");
            customerHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.AccessToken}");

            using HttpResponseMessage customerResponse = await customerHttpClient.GetAsync($"customers/{_response.payer_id}");
            string email = customerEmail;
            if (customerResponse.IsSuccessStatusCode && string.IsNullOrEmpty(email))
            {
                var customerJson = await customerResponse.Content.ReadAsStringAsync();
                var _customerResponse = JsonConvert.DeserializeObject<CustomerResponse>(customerJson);
                email = Convert.ToString(_customerResponse?.email ?? "");
            }

            var customer = this._customerRepository.Find(c => c.Email == email && c.TenantId == tenantId).Result.FirstOrDefault();

            if (customer == null) 
            {
                return null;
            }

            var subscription = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                TenantId = customer.TenantId,
                CustomerId = customer.CustomerId,
                PaymentMethodId = _response.payment_method_id,
                Reference = preapprovalId,
                PlanReference = _response.preapproval_plan_id,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
            };

            await this._subscriptionRepository.Add(new DataAccess.Subscription
            {
                SubscriptionId = subscription.SubscriptionId,
                TenantId = subscription.TenantId,
                CustomerId = subscription.CustomerId,
                PaymentMethodId = subscription.PaymentMethodId,
                Reference = subscription.Reference,
                PlanReference = subscription.PlanReference,
                CreateAt = subscription.CreateAt,
                UpdateAt = subscription.UpdateAt
            });

            return subscription;
        }
    }

    class PreapprovalResponse
    {
        public string payer_id { get; set; }
        public string payment_method_id { get; set; }
        public string preapproval_plan_id { get; set; }
    }

    class CustomerResponse
    {
        public string email { get; set; }
    }
}
