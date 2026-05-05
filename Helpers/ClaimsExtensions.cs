using System.Security.Claims;

namespace EmployeeFlow.Helpers
{
    public static class ClaimsExtensions
    {
        public static int GetCompanyId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("companyId");

            if (claim is null)
                throw new UnauthorizedAccessException("Invalid token: companyId not found");

            return int.Parse(claim.Value);
        }
    }
}