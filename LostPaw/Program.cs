using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LostPaw.Data;
using LostPaw.Models;
using LostPaw.Hubs;
using LostPaw.AppConfig;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<LostPawDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<LostPawDbContext>();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
});

builder.Services.Configure<GoogleConfig>(builder.Configuration.GetSection(GoogleConfig.SectionName));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseWebSockets();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");
app.WarmupDbContext();
app.Run();


public static class Extensions
{
    public static void WarmupDbContext(this WebApplication app)
    {
        using var context = new LostPawDbContext(app.Configuration.GetConnectionString("DefaultConnection"));
        var _ = context.Model;
    }
}
