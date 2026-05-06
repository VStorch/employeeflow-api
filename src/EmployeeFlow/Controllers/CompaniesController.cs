using EmployeeFlow.DTOs.Company;
using EmployeeFlow.Helpers;
using EmployeeFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _service;

        public CompaniesController(CompanyService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CompanyResponse>> Create(CreateCompanyRequest dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMyCompany), null, result);
        }

        [HttpGet("me")]
        public async Task<ActionResult<CompanyResponse>> GetMyCompany()
        {
            var companyId = User.GetCompanyId();

            var result = await _service.GetByIdAsync(companyId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<CompanyResponse>> Update(UpdateCompanyRequest dto)
        {
            var companyId = User.GetCompanyId();

            var result = await _service.UpdateAsync(companyId, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            var companyId = User.GetCompanyId();

            var success = await _service.DeleteAsync(companyId);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}