using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleCustomer.MVC.Models;
using SimpleCustomer.WebAPI.Data.Models;
using SimpleCustomer.WebAPI.Repository;
using System.Text;
namespace SimpleCustomer.MVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        //Get: /Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            var customerViewModels = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FullName = c.FullName,
                Gender = c.Gender,
                Birthdate = c.Birthdate,
                EmailAddress = c.EmailAddress,
                PermanentAddress = c.PermanentAddress,
                PhoneNumber = c.PhoneNumber
            });
            return View(customerViewModels);
        }

        // Details
        public async Task<IActionResult> Details(int id)
        {

            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            var customerViewModel = new CustomerViewModel()
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Gender = customer.Gender,
                Birthdate = customer.Birthdate,
                EmailAddress = customer.EmailAddress,
                PermanentAddress = customer.PermanentAddress,
                PhoneNumber = customer.PhoneNumber
            };
            if(customer == null)
            {
                return NotFound();
            }

            return View("CustomerDetail", customerViewModel);
        }

        // GET
        public ActionResult Create()
        {
            return View("CreateCustomer");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel customer)
        {
            if (!ModelState.IsValid)
                return View("CreateCustomer", customer);
            var customerEntity = new Customer()
            {
                FullName = customer.FullName,
                Birthdate = customer.Birthdate,
                Gender = customer.Gender,
                EmailAddress = customer.EmailAddress,
                PermanentAddress = customer.PermanentAddress,
                PhoneNumber = customer.PhoneNumber,
            };
            try
            {
                long customerId = await _customerRepository.AddCustomerAsync(customerEntity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Unable to create customer. Please try again.");
                return View("CreateCustomer", customer);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            var customerViewModel = new CustomerViewModel()
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Gender = customer.Gender,
                Birthdate = customer.Birthdate,
                EmailAddress = customer.EmailAddress,
                PermanentAddress = customer.PermanentAddress,
                PhoneNumber = customer.PhoneNumber
            };
            if (customer == null)
            {
                return View("Error");
            }
            return View("EditCustomer", customerViewModel);
            
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customer)
        {
            if (id != customer.Id)
            {
                return BadRequest(); 
            }
            if (!ModelState.IsValid)
                return View(customer);

            var customerEntity = new Customer()
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Birthdate = customer.Birthdate,
                Gender = customer.Gender,
                EmailAddress = customer.EmailAddress,
                PermanentAddress = customer.PermanentAddress,
                PhoneNumber = customer.PhoneNumber
            };
            bool updateSuccessful = await _customerRepository.EditCustomer(customerEntity);
            if (updateSuccessful)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Unable to update customer. Please try again.");
            return View(customer);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            var customerEntity = new CustomerViewModel()
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Birthdate = customer.Birthdate,
                Gender = customer.Gender,
                EmailAddress = customer.EmailAddress,
                PermanentAddress = customer.PermanentAddress,
                PhoneNumber = customer.PhoneNumber
            };
            if (customer != null)
            {
                return View("DeleteCustomer", customerEntity);
            }
            return View("Error");
        }
        // POST: Customers/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(CustomerViewModel customer)
        {
            try
            {
                await _customerRepository.DeleteCustomerAsync(customer.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to delete customer. Please try again.");
                return View("Error");
            }
        }
    }
}