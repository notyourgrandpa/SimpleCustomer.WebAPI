using SimpleCustomer.WebAPI.Data.Models;
namespace SimpleCustomer.WebAPI.Repository
{
    public interface ICustomerRepository
    {
        Task <long> AddCustomerAsync(Customer customer);
        Task <bool> EditCustomer(Customer customer);
        Task <bool> DeleteCustomerAsync(int id);
        Task <Customer> GetCustomerByIdAsync(int id);
        Task <IEnumerable<Customer>> GetAllCustomersAsync();
    }
}
