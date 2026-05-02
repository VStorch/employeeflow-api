using EmployeeFlow.DTOs;
using EmployeeFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeesController(EmployeeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDTO dto)
        {
            var employee = await _service.CreateAsync(dto);
            return Ok(employee);
        }
    }
}