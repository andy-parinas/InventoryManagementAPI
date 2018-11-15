using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationHelperTools.Data
{
    class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var databaseServer = args[0];
            var databaseName = args[1];
            var user = args[2];
            var password = args[3];

            var connectionString = string.Format("Server={0};Database={1};User={2};Password={3};",
                databaseServer, databaseName, user, password);

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            Console.WriteLine(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
