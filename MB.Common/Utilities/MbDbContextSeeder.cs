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
    using Models.Hotels;
    using Models.Monuments;
    using Models.Oblasts;

    public static class MbDbContextSeeder
    {
        private const string MonumentDefaultName = "Monument";
        private const string HotelDefaultName = "Hotel";
        private const string MonumentDefaultImageUrl = 
            "https://res.cloudinary.com/dpoafu9y0/image/upload/v1546636699/monuments/default.jpg";
        private const string HotelDefaultImageUrl = 
            "https://res.cloudinary.com/dpoafu9y0/image/upload/v1546636605/hotels/default.jpg";
        private const string DefaultDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec id nisi aliquam purus laoreet suscipit. Sed aliquam magna est, in accumsan nunc lacinia in. Nulla pretium, leo in cursus congue, lectus quam vestibulum nibh, ac sollicitudin dui erat quis magna. Praesent tincidunt velit risus, in malesuada ante molestie ut. Proin elementum arcu eu ante fermentum, ut tempus dui hendrerit. Aliquam tempor nisl non est malesuada facilisis. Cras eget odio sit amet lacus commodo semper posuere vitae lorem. Quisque eleifend scelerisque orci ut viverra. In feugiat sit amet urna ut consequat. Sed at iaculis purus. Sed et malesuada diam. Sed in orci eu mi sollicitudin finibus. Pellentesque dictum ullamcorper volutpat. Integer nisi magna, pretium non egestas nec, laoreet nec sem. Maecenas ipsum magna, tristique sed ligula at, ullamcorper faucibus felis. Nulla non dui sit amet urna volutpat laoreet.Duis at felis metus.Aliquam mollis est ut metus blandit dignissim.Curabitur pellentesque consequat nulla non pharetra. Curabitur suscipit lacus nisl, eget faucibus erat venenatis ac.Pellentesque quis convallis mi, vitae aliquam est.Nullam a consequat urna. Interdum et malesuada fames ac ante ipsum primis in faucibus.Etiam in blandit tellus. Cras sit amet venenatis metus.In a lobortis velit. Lorem ipsum dolor sit amet, consectetur adipiscing elit.Duis dignissim nulla eu urna tincidunt, id laoreet tortor molestie. Sed lobortis nibh dui, a aliquet eros faucibus sed. Morbi mi quam, malesuada sed ultrices nec, posuere a leo.Fusce tempus turpis vitae blandit aliquam. Proin urna est, pellentesque vel porta at, maximus vel mi.Integer ut libero ut orci efficitur facilisis sit amet sed metus.Praesent faucibus mollis aliquet. Nam nunc ligula, placerat sed pulvinar in, ullamcorper a dolor.Morbi pellentesque metus in nulla mattis, sed sollicitudin odio sollicitudin. Nulla velit ex, volutpat ac urna et, lobortis accumsan nibh.Pellentesque vitae eros luctus, venenatis nibh eget, fringilla tortor. Praesent euismod non dolor vel consectetur. Curabitur nisl odio, facilisis congue tellus sed, hendrerit pretium lacus.Nulla sit amet maximus mi, sit amet venenatis ex.";
        private const string DefaultHotelAddress = "Ivan Vazov str, ";
        private const string DefaultHotelPhoneNumber = "0856445187";

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
            var oblasts = JsonConvert.DeserializeObject<List<Oblast>>(fileContent);
            
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
                if (dbContext.Oblasts.Any(x => x.Id == i))
                {
                    var monument = new Monument
                    {
                        Name = MonumentDefaultName + counter++,
                        OblastId = i,
                        ImageUrl = MonumentDefaultImageUrl,
                        Description = DefaultDescription,
                    };

                    dbContext.Monuments.Add(monument);
                }
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
                if (dbContext.Oblasts.Any(x => x.Id == i))
                {
                    int stars = (counter % 5) + 1;
                    string address = DefaultHotelAddress + counter;
                    var hotel = new Hotel
                    {
                        Name = HotelDefaultName + counter++,
                        OblastId = i,
                        ImageUrl = HotelDefaultImageUrl,
                        Description = DefaultDescription,
                        Address = address,
                        PhoneNumber = DefaultHotelPhoneNumber,
                        Stars = stars,
                    };

                    dbContext.Hotels.Add(hotel);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
