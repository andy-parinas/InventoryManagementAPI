using AplicationHelperTools.Data;
using System;
using System.Linq;

namespace AplicationHelperTools
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Database Utility Application");
            Console.WriteLine("============================");
            Console.WriteLine("Please Select the Following: ");
            Console.WriteLine("Enter 1: Create Database User.");



            var config = new string[]
            {
                "localhost", "InventoryDB", "sa", "InventoryManagement(!)"
            };

            using(var context = new AppDbContextFactory().CreateDbContext(config))
            {
                Console.WriteLine("Running inside the context");

                var users = context.Users.ToList();

                foreach(var user in users)
                {
                    Console.WriteLine(user.FirstName);
                    Console.WriteLine(user.LastName);
                }

            }



        }


        private bool CreateApplicationUser(string username, string password)
        {


            return true;
        }
    }
}
