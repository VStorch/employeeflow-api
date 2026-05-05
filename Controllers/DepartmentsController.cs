using EmployeeFlow.DTOs.Department;
using EmployeeFlow.Helpers;
using EmployeeFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<DepartmentResponse>> Create(CreateDepartmentRequest dto)
        {
            var companyId = User.GetCompanyId();

            var department = await _service.CreateAsync(companyId, dto);
            return Ok(department);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAll()
        {
            var companyId = User.GetCompanyId();

            var departments = await _service.GetByCompanyAsync(companyId);
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentResponse>> GetById(int id)
        {
            var companyId = User.GetCompanyId();

            var department = await _service.GetByIdAsync(id, companyId);

            if (department is null)
                return NotFound();

            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentResponse>> Update(int id, UpdateDepartmentRequest dto)
        {
            var companyId = User.GetCompanyId();

            var updatedDepartment = await _service.UpdateAsync(id, companyId, dto);
            return Ok(updatedDepartment);
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