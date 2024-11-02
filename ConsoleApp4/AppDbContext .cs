using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        public async Task<List<UserCompanyDTO>> GetUsersWithCompaniesAsync()
        {
            return await Users
                .FromSqlRaw("EXEC")
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByNamePatternAsync(string namePattern)
        {
            return await Users
                .FromSqlInterpolated($"EXEC {namePattern}")
                .ToListAsync();
        }

        public async Task<double> GetAverageUserAgeAsync()
        {
            var averageAgeParam = new SqlParameter
            {
                ParameterName = "@AverageAge",
                SqlDbType = System.Data.SqlDbType.Float,
                Direction = System.Data.ParameterDirection.Output
            };

            await Database.ExecuteSqlRawAsync("EXEC ", averageAgeParam);

            return (double)averageAgeParam.Value;
        }
    }
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<User> Users { get; set; }
    }
}
