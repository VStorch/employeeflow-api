using EmployeeFlow.DTOs.Role;
using EmployeeFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _service;

        public RolesController(RoleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequest dto)
        {
            var role = await _service.CreateAsync(dto);
            return Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int companyId)
        {
            var roles = await _service.GetByCompanyAsync(companyId);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int companyId)
        {
            var role = await _service.GetByIdAsync(id, companyId);

            if (role is null)
                return NotFound();

            return Ok(role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromQuery] int companyId, UpdateRoleRequest dto)
        {
            var updatedRole = await _service.UpdateAsync(id, companyId, dto);
            return Ok(updatedRole);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int companyId)
        {
            await _service.DeleteAsync(id, companyId);
            return NoContent();
        }
    }
}