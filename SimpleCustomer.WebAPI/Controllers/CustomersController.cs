using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCustomer.WebAPI.Data;
using SimpleCustomer.WebAPI.Data.Models;
using SimpleCustomer.WebAPI.Repository;

namespace SimpleCustomer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository repository)
        {
            //_context = context;
            _customerRepository = repository;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> Getcustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (customer == null || customer.Id != id)
            {
                return BadRequest("Customer ID mismatch.");
            }

            var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Birthdate = customer.Birthdate;
            existingCustomer.Gender = customer.Gender;
            existingCustomer.PermanentAddress = customer.PermanentAddress;
            existingCustomer.EmailAddress = customer.EmailAddress;
            existingCustomer.PhoneNumber = customer.PhoneNumber;

            var updated = await _customerRepository.EditCustomer(existingCustomer);
            if (!updated)
            {
                return StatusCode(500, "An error occurred while updating the customer.");
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCustomer(Customer customer)
        {
            if (customer == null)
                return BadRequest("Invalid customer data.");

            var customerId = await _customerRepository.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(PostCustomer), new { id = customerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            await _customerRepository.DeleteCustomerAsync(customer.Id);
            return NoContent();
        }
    }
}
