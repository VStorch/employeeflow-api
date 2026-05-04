using EmployeeFlow.DTOs.Department;
using EmployeeFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _service;

        public DepartmentsController(DepartmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentRequest dto)
        {
            var department = await _service.CreateAsync(dto);
            return Ok(department);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int companyId)
        {
            var departments = await _service.GetByCompanyAsync(companyId);
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int companyId)
        {
            var department = await _service.GetByIdAsync(id, companyId);

            if (department is null)
                return NotFound();

            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromQuery] int companyId, UpdateDepartmentRequest dto)
        {
            var updatedDepartment = await _service.UpdateAsync(id, companyId, dto);
            return Ok(updatedDepartment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int companyId)
        {
            await _service.DeleteAsync(id, companyId);
            return NoContent();
        }
    }
}