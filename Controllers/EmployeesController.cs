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
        public async Task<IActionResult> Create(CreateEmployeeRequest dto)
        {
            var employee = await _service.CreateAsync(dto);
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int companyId,
            [FromQuery] int? departmentId,
            [FromQuery] int? roleId)
        {
            var employees = await _service.GetAllAsync(companyId, departmentId, roleId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int companyId)
        {
            var employee = await _service.GetByIdAsync(id, companyId);
            if (employee is null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromQuery] int companyId, UpdateEmployeeRequest dto)
        {
            var updatedEmployee = await _service.UpdateAsync(id, companyId, dto);
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int companyId)
        {
            await _service.DeleteAsync(id, companyId);
            return NoContent();
        }
    }
}