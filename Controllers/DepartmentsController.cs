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
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _service.GetByCompanyAsync(companyId);
            return Ok(result);
        }
    }
}