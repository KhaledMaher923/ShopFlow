
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopFlow.Data;
using ShopFlow.Features.AddToCart;

namespace ShopFlow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ShopFlowDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining(typeof(Program)));
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
            builder.Services.AddScoped<IProductCatalog, EfProductCatalog>();
            builder.Services.AddScoped<ICartStore, EfCartStore>();

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
