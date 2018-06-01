using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Infrastructure.AspNet.Identity;
using WebApp.Models;
using WebApp.Services;
using WebApp.DAL;
using System.Diagnostics;

namespace WebApp
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
			services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
			services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();

			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddDefaultTokenProviders();

			// Add application services.
			services.AddTransient<IEmailSender, EmailSender>();

			services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
			services.AddTransient<IGroupJoiningRequestRepository, GroupJoiningRequestRepository>();
			services.AddScoped<ICourseRepository, CourseRepository>();
			services.AddScoped<IFileResourceRepository, FileResourceRepository>();
			services.AddScoped<ILecturerRepository, LecturerRepository>();
			services.AddScoped<IProjectChoiceRepository, ProjectChoiceRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IProposalRepository, ProposalRepository>();
			services.AddTransient<IStudentRepository, StudentRepository>();
			services.AddSingleton(Configuration);
			services.AddMvc();
			// Adds a default in-memory implementation of IDistributedCache.
			services.AddDistributedMemoryCache();

			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromSeconds(10);
				options.Cookie.HttpOnly = true;
			});
            

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseSession();
			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			CreateRoles(serviceProvider).Wait();
		}

		private async Task CreateRoles(IServiceProvider serviceProvider)
		{
			//initializing custom roles 
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			string[] roleNames = { "Admin", "Manager", "Member" };
			IdentityResult roleResult;

			foreach (var roleName in roleNames)
			{
				var roleExist = await RoleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					//create the roles and seed them to the database: Question 1
					roleResult = await RoleManager.CreateAsync(new ApplicationRole(roleName));
				}
			}

			//Here you could create a super user who will maintain the web app
			var poweruser = new ApplicationUser
			{

				UserName = "s12345",
				Email = "leslieharland@outlook.com",
				userType = new Lecturer
				{
          
					staff_id = "s12345",
					contact_number = "90055705",
					course_id = 1,
					lecturer_id = 1,
				admin = true,
				email_address = "leslieharland@outlook.com",
				full_name = "Leslie Harland"
				}
			};


			var student = new ApplicationUser
			{

				UserName = "p1334339",
				Email = "xyxy54.13@ichat.sp.edu.sg.com",
				userType = new Student
				{
				email_address = "xyxy54.13@ichat.sp.edu.sg.com",
				full_name = "Leslie",
				completed_module = true,
                
					admin_number = "p1334339",
					semester = 1,
					course_id = 1,
					student_id = 1,
					year = 2018
				}
			};

			//Ensure you have these values in your appsettings.json file
			string userPWD = "P@ssw0rd";
			var _user = await UserManager.FindByEmailAsync("leslieharland@outlook.com");

			if (_user != null)
			{
				var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
				Debug.WriteLine("Suceeded" + createPowerUser.Succeeded);
				if (createPowerUser.Succeeded)
				{
					//here we tie the new user to the role
					await UserManager.AddToRoleAsync(poweruser, "Admin");

				}
			}


			_user = await UserManager.FindByEmailAsync("xyxy54.13@ichat.sp.edu.sg");

			if (_user != null)
			{
				var createPowerUser = await UserManager.CreateAsync(student, userPWD);
				if (createPowerUser.Succeeded)
				{
					//here we tie the new user to the role
					await UserManager.AddToRoleAsync(poweruser, "Member");

				}
			}

		}
	}
}
