using bookstore.Helpers;
using bookstore.Mapper;
using bookstore.Models;
using bookstore.Repositories.BookRepository;
using bookstore.Repositories.BorrowerBooksRepository;
using bookstore.Repositories.BorrowerRepository;
using bookstore.Services.BookService;
using bookstore.Services.PhotoService;
using bookstore.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookstore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddDbContext<AppDbContext>(
    options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingConfig>());

            // Repository layer DI
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBorrowerRepository, BorrowerRepository>();
            builder.Services.AddScoped<IBorrowerBooksRepository, BorrowerBooksRepository>();

            // Service layer DI
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<IBookService, BookService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
