using EmployeeFlow.DTOs.Employees;
using EmployeeFlow.Helpers;
using EmployeeFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<EmployeeResponse>> Create(CreateEmployeeRequest dto)
        {
            var companyId = User.GetCompanyId();

            var employee = await _service.CreateAsync(companyId, dto);
            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetAll(
            [FromQuery] int? departmentId,
            [FromQuery] int? roleId)
        {
            var companyId = User.GetCompanyId();

            var employees = await _service.GetAllAsync(companyId, departmentId, roleId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id)
        {
            var companyId = User.GetCompanyId();

            var employee = await _service.GetByIdAsync(id, companyId);
            if (employee is null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> Update(int id, UpdateEmployeeRequest dto)
        {
            var companyId = User.GetCompanyId();

            var updatedEmployee = await _service.UpdateAsync(id, companyId, dto);
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var companyId = User.GetCompanyId();

            await _service.DeleteAsync(id, companyId);
            return NoContent();
        }
    }
}