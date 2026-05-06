using AutoMapper;
using EmployeeFlow.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeFlow.Mappings;
using Microsoft.Data.Sqlite;

namespace EmployeeFlow.Tests.Common
{
    public abstract class TestBase : IDisposable
    {
        protected readonly AppDbContext Context;
        protected readonly IMapper Mapper;

        private readonly SqliteConnection _connection;

        protected TestBase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new AppDbContext(options);
            Context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(EmployeeProfile).Assembly);
            });

            Mapper = config.CreateMapper();
        }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Close();
        }
    }
}