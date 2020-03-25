using System;
using a2_s3644668_s3643929.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace a2_s3644668_s3643929.Data
{
    public static class SeedLogin
    {

        public static void Seed_Login(this ModelBuilder builder)
        {

            builder.Entity<Login>().HasData(

               new Login
               {
                   UserID = 12345678,
                   CustomerID = 2100,
                   Password = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                   ModifyDate = DateTime.UtcNow
                   


               },
               new Login
               {
                   UserID = 38074569,
                   CustomerID = 2200,
                   Password = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                   ModifyDate = DateTime.UtcNow

               },
               new Login
               {
                   UserID = 17963428,
                   CustomerID = 2300,
                   Password = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                   ModifyDate = DateTime.UtcNow
                   
               }

               ); ;

        }
    }
}
