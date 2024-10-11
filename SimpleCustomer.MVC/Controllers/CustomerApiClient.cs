using Newtonsoft.Json;
using SimpleCustomer.WebAPI.Data.Models;
using SimpleCustomer.WebAPI.Repository;
using System.Text;
using SimpleCustomer.MVC.Models;

namespace SimpleCustomer.MVC.Controllers
{
    public class CustomerApiClient : ICustomerRepository
    {
        private readonly HttpClient _httpClient;

        public CustomerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<long> AddCustomerAsync(Customer customer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/customers", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);
            var responseObj = JsonConvert.DeserializeObject<CustomerIdResponse>(json);
            return responseObj.Id;
        }

        public async Task<bool> EditCustomer(Customer customer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/customers/{customer.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/customers/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/customers/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Customer>(json);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var response = await _httpClient.GetAsync("api/customers");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Customer>>(json);
        }
    }
}
public class CustomerIdResponse
{
    public long Id { get; set; }
}
