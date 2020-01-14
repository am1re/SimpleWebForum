using App.API.Authorization;
using App.API.Errors;
using App.BusinessLogic.Identity;
using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Services;
using App.BusinessLogic.Validators.Forum;
using App.BusinessLogic.Validators.Identity.Role;
using App.BusinessLogic.Validators.Identity.User;
using App.BusinessLogic.Validators.Post;
using App.BusinessLogic.Validators.Thread;
using App.Data;
using App.Data.Contexts;
using App.Data.Entities.Identity;
using App.Data.Interfaces;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Reflection;

namespace App.API
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
            services.AddDbContext<ForumDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("App.Data")));

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ForumDbContext>()
                .AddUserManager<ForumUserManager>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:5000";
                o.Audience = "forum.api";
                o.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                var authPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                authPolicyBuilder = authPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = authPolicyBuilder.Build();

                options.AddPolicy("ModeratorOfForum",
                    policy => policy.Requirements.Add(new ModeratorOfForumRequirement())
                );

                options.AddPolicy("EditPost",
                    policy => policy.Requirements.Add(new EditPostRequirement())
                );

                options.AddPolicy("EditThread",
                    policy => policy.Requirements.Add(new EditThreadRequirement())
                );
            });

            #region uncomment for disabling model state filter
            //services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
            #endregion
            #region uncomment for SwaggerFluentValidation
            //services.AddMvc().AddFluentValidation(fv =>
            //{
            //    fv.RegisterValidatorsFromAssemblyContaining<ForumResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreateForumResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdateForumResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<RoleResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreateUserResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdateUserResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UserResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreatePostResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<PostResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdatePostResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreateThreadResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<ThreadResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdateThreadResourceValidator>();
            //}); 
            #endregion

            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "ForumApp API" });
                //x.AddFluentValidationRules();
                //x.MapType<Microsoft.AspNetCore.Mvc.ProblemDetails>(() => new OpenApiSchema { });
            });

            services.AddAutoMapper(
                typeof(App.BusinessLogic.Mapping.MappingProfile),
                typeof(App.API.Mapping.MappingProfile)
            );

            services.AddSingleton<IAuthorizationHandler, ModeratorOfForumHandler>();
            services.AddScoped<IAuthorizationHandler, EditPostHandler>();
            services.AddScoped<IAuthorizationHandler, EditThreadHandler>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserSerivce>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IForumService, ForumService>();
            services.AddTransient<IThreadService, ThreadService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(options => options.AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .AllowAnyOrigin());
            }
            else
            {
                //app.UseMiddleware<ExceptionHandler>();
                app.UseCors(options => options.AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .WithOrigins("http://localhost:4200/")); // (e.g. https://mydomain.com)
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ForumApp API");
            });

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
