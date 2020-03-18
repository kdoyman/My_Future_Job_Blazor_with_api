using System.Collections.Generic;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService) { _customerService = customerService; }


        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Customer>> Get() =>
            _customerService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCustomer")]
        public ActionResult<Customer> Get(string id)
        {
            var custo = _customerService.Get(id);

            if (custo == null)
            {
                return NotFound();
            }

            return custo;
        }

        [HttpPost]
        public ActionResult<Customer> Create(Customer custo)
        {
            _customerService.Create(custo);

            return CreatedAtRoute("GetCustomer", new { id = custo.Id.ToString() }, custo);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Customer custoIn)
        {
            var custo = _customerService.Get(id);

            if (custo == null)
            {
                return NotFound();
            }

            _customerService.Update(id, custoIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var custo = _customerService.Get(id);

            if (custo == null)
            {
                return NotFound();
            }

            _customerService.Remove(custo.Id);

            return NoContent();
        }

    }
}