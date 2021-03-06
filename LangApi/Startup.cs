using LangApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

namespace LangApi
{
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
            services.AddControllers();

            var _langs = new List<Lang>() {
                new Lang {Id = "c", Year = 1972},
                new Lang {Id = "cpp", Year = 1985}
            };
            services.AddSingleton(_langs);

            //------  "Bearer" scheme
            services.AddAuthentication()
                .AddCookie(cfg => { cfg.SlidingExpiration = true; })
                .AddJwtBearer(ControlFlowGraph =>
                {
                    ControlFlowGraph.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = LangTokenOptions.Issuer,
                        ValidAudience = LangTokenOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LangTokenOptions.Key))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
