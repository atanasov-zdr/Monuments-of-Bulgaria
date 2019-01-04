namespace MB.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    
    using Newtonsoft.Json;

    using Common;
    using Data;
    using Mapping;
    using Models.Hotels;
    using Models.Monuments;
    using Models.Oblasts;
    using ViewModels.Oblasts;

    public static class MbDbContextSeeder
    {
        private const string MonumentDefaultName = "Monument";
        private const string HotelDefaultName = "Hotel";
        private const string MonumentDefaultImageUrl = 
            "https://res.cloudinary.com/dpoafu9y0/image/upload/v1546636699/monuments/default.jpg";
        private const string HotelDefaultImageUrl = 
            "https://res.cloudinary.com/dpoafu9y0/image/upload/v1546636605/hotels/default.jpg";

        public static void Seed(MbDbContext dbContext, IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedRoles(roleManager);
            
            SeedOblasts(dbContext);

            SeedMonuments(dbContext);

            SeedHotels(dbContext);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in GlobalConstants.Roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    IdentityResult result = roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    
                    if (!result.Succeeded)
                        throw new Exception(String.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
                }
            }
        }

        private static void SeedOblasts(MbDbContext dbContext)
        {
            string fileContent = File.ReadAllText(GlobalConstants.OblastsInfoViewPath);
            var oblastsModels = JsonConvert.DeserializeObject<List<OblastSeedViewModel>>(fileContent);

            IQueryable<Oblast> oblasts = oblastsModels.AsQueryable().To<Oblast>();
            foreach (var oblast in oblasts)
            {
                if (!dbContext.Oblasts.Any(x => x.Name == oblast.Name))
                {
                    dbContext.Oblasts.Add(oblast);
                }
            }

            dbContext.SaveChanges();
        }

        private static void SeedMonuments(MbDbContext dbContext)
        {
            if (dbContext.Monuments.Any())
                return;

            int start = dbContext.Oblasts.OrderBy(x => x.Id).First().Id;
            int end = dbContext.Oblasts.OrderBy(x => x.Id).Last().Id;
            int counter = 1;
            for (int i = start; i <= end; i++)
            {
                var monument = new Monument
                {
                    Name = MonumentDefaultName + counter++,
                    OblastId = i,
                    ImageUrl = MonumentDefaultImageUrl,
                };

                dbContext.Monuments.Add(monument);
            }

            dbContext.SaveChanges();
        }

        private static void SeedHotels(MbDbContext dbContext)
        {
            if (dbContext.Hotels.Any())
                return;

            int start = dbContext.Oblasts.OrderBy(x => x.Id).First().Id;
            int end = dbContext.Oblasts.OrderBy(x => x.Id).Last().Id;
            int counter = 1;
            for (int i = start; i <= end; i++)
            {
                var hotel = new Hotel
                {
                    Name = HotelDefaultName + counter++,
                    OblastId = i,
                    ImageUrl = HotelDefaultImageUrl,
                };

                dbContext.Hotels.Add(hotel);
            }

            dbContext.SaveChanges();
        }
    }
}
