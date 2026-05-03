using Microsoft.AspNetCore.Mvc;
using EmployeeFlow.Services;
using EmployeeFlow.DTOs.Role;

namespace EmployeeFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequest dto)
        {
            var result = await _roleService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _roleService.GetByCompanyAsync(companyId);
            return Ok(result);
        }
    }
}