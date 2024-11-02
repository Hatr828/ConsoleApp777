using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer("")
                    .Options;

            using (var context = new AppDbContext(options))
            {
                var usersWithCompanies = await context.GetUsersWithCompaniesAsync();
                Console.WriteLine("Rezult: ");
                foreach (var user in usersWithCompanies)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, Age: {user.Age}, CompanyId: {user.CompanyId}, CompanyName: {user.CompanyName}");
                }

                string namePattern = "Tom";
                var usersByNamePattern = await context.GetUsersByNamePatternAsync(namePattern);
                Console.WriteLine();
                foreach (var user in usersByNamePattern)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, Age: {user.Age}");
                }

                double averageAge = await context.GetAverageUserAgeAsync();
                Console.WriteLine($"averageAge");
                Console.ReadLine();
            }
        }
    }
}
