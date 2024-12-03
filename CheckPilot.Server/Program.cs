global using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CheckPilot.Server.Data;
using CheckPilot.Models;
using CheckPilot.Server.Repository;
using CheckPilot.Server.Service;

namespace CheckPilot.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CheckPilot.Server", policy =>
                {
                    //policy.WithOrigins()
                    //      .AllowAnyHeader()
                    //      .AllowAnyMethod()
                    //      .AllowCredentials()
                    //      .WithExposedHeaders("Authorization");

                   policy.AllowAnyOrigin()  // Permite acceso desde cualquier dominio
                      .AllowAnyHeader()  // Permite cualquier encabezado
                      .AllowAnyMethod()  // Permite cualquier método HTTP (GET, POST, etc.)
                      .WithExposedHeaders("Authorization");
                });
            });

            builder.Services.AddHttpClient<SapService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "CheckPilot.Server",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("aplicacion_FFACSA_CHECKPILOT_ClaveSecreta"))
                    };
                });

            builder.Services.AddHttpClient<SapService>(client =>
            {
                client.BaseAddress = new Uri("https://172.16.50.45:50000/b1s/v1/"); 
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddAuthorization();

            #region Repositorios

            builder.Services.AddScoped<IUserRepository<User, int>, UserRepository>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddScoped<IPhotoRepository<InvoicePhoto, int>, InvoicePhotoRepository>();

            #endregion


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLocalization();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CheckPilot.Server");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });


            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    var token = context.Request.Headers["Authorization"].ToString();
                    Console.WriteLine($"Token recibido en el servidor: {token}");
                }
                else
                {
                    Console.WriteLine("No se recibió el encabezado Authorization.");
                }
                await next();
            });

            app.Run();

        }
    }
}