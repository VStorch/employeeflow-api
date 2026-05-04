using EmployeeFlow.Data;
using EmployeeFlow.Middleware;
using EmployeeFlow.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<EmployeeService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();