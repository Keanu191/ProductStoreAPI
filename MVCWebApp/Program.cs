/*
 * NOTE TO LECTURER:
 * THE FRONTEND WAS DONE WITH CODE TAKEN FROM THIS TUTORIAL: https://www.yogihosting.com/aspnet-core-identity-mongodb/
 */

/*
 * 4/12/2024:

 * Query the object ID to the roles table and then pick up if the name is equals to admin or not then load up the admin interface
 */
using Microsoft.AspNetCore.Authentication;
using MVCWebApp.Models;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// hard code the connection string as the tutorial uses docker, thanks jacky boi for the suggestion
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            "mongodb+srv://dbuser:123@cluster0.k7wz9.mongodb.net/", "ProductsDB"
        );


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

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
