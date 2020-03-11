using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Interface
{
    interface ICustomerService
    {
        Task<List<Customer>> GetCustomers();
        Task<bool> CreateCustomer(Customer customer);
        Task<bool> EditCustomer(string id, Customer customer);
        Task<Customer> SingleCustomer(string id);
        Task<bool> DeleteCustomer(string id);
    }
}
