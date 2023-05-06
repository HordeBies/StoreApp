using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Data;
using Store.Models;
using Store.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IConfigurationSection configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext db;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db, IConfiguration configuration)
        {
            this.configuration = configuration.GetRequiredSection("DbInitializer");
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
        }

        public async Task Initialize()
        {
            try
            {
                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception e)
            {

            }
            if (!await roleManager.RoleExistsAsync(Role.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Customer));
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
                await roleManager.CreateAsync(new IdentityRole(Role.Company));
                
                await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "oa.mehmetdmrc@gmail.com",
                    Email = "oa.mehmetdmrc@gmail.com",
                    Name = "Mehmet Demirci",
                    PhoneNumber = "5436036810",
                    StreetAddress = "Çankaya",
                    City = "Ankara",
                    PostalCode = "06530"
                }, configuration["AdminPassword"]);

                var user = await db.ApplicationUsers.FirstOrDefaultAsync(r => r.Email == "oa.mehmetdmrc@gmail.com");
                await userManager.AddToRoleAsync(user, Role.Admin);
            }
        }
    }
}
