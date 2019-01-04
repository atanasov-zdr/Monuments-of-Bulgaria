namespace MB
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using AutoMapper;

    using ReflectionIT.Mvc.Paging;

    using Common;
    using Common.Utilities;
    using Data;
    using Mapping;
    using Models;
    using Services.Contracts.Hotels;
    using Services.Contracts.Monuments;
    using Services.Contracts.Oblasts;
    using Services.Contracts.Trips;
    using Services.Hotels;
    using Services.Monuments;
    using Services.Oblasts;
    using Services.Trips;
    using ViewModels.Hotels;
    using ViewModels.Hotels.HotelComments;
    using ViewModels.Monuments;
    using ViewModels.Monuments.MonumentComments;
    using ViewModels.Oblasts;
    using ViewModels.Trips;
    using CloudinaryDotNet;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MbDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies());

            services.AddIdentity<MbUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<MbDbContext>();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });
            
            services.AddMvc(opts => 
            {
                opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper();

            AutoMapperConfig.RegisterMappings(
                typeof(OblastSeedViewModel).Assembly,
                typeof(MonumentAllViewModel).Assembly,
                typeof(MonumentCommentViewModel).Assembly,
                typeof(HotelAllViewModel).Assembly,
                typeof(HotelCommentViewModel).Assembly,
                typeof(TripMyViewModel).Assembly);
            
            services.AddPaging(opt => opt.ViewName = "Pager");

            services.AddScoped<UserStore<MbUser>>();
            
            var account = new Account(GlobalConstants.CloudinaryName,
                GlobalConstants.CloudinaryKey, GlobalConstants.CloudinarySecret);
            services.AddScoped(x => new Cloudinary(account));
            services.AddScoped<ImagesUploader>();

            services.AddScoped<IOblastsService, OblastsService>();
            services.AddScoped<IMonumentsService, MonumentsService>();
            services.AddScoped<IMonumentReviewsService, MonumentReviewsService>();
            services.AddScoped<IMonumentCommentsService, MonumentCommentsService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IHotelReviewsService, HotelReviewsService>();
            services.AddScoped<IHotelCommentsService, HotelCommentsService>();
            services.AddScoped<ITripsService, TripsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<MbDbContext>();                
                MbDbContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
