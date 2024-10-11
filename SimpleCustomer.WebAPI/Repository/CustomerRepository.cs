using Microsoft.EntityFrameworkCore;
using SimpleCustomer.WebAPI.Data;
using SimpleCustomer.WebAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
namespace SimpleCustomer.WebAPI.Repository
{
    public class CustomerRepository: ICustomerRepository
    {   
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> AddCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
            }

            _context.customers.Add(customer);

            try
            {
                await _context.SaveChangesAsync();
                return customer.Id;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the customer.", ex);
            }

        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                var existingCustomer = await GetCustomerByIdAsync(id);
                if (existingCustomer != null)
                {
                    _context.Remove(existingCustomer);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public async Task<bool> EditCustomer(Customer customer)
        {
            try
            {
                _context.customers.Update(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the customer.", ex);
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.customers.FindAsync(id);
        }
    }
}
