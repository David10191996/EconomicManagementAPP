using EconomicManagementAPP.Services;
using EconomicManagementAPP.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorieAccountTypes, RepositorieAccountTypes>();
builder.Services.AddTransient<IRepositorieUser, RepositorieUser>();
builder.Services.AddTransient<IRepositorieOperationTypes, RepositorieOperationTypes>();
builder.Services.AddTransient<IRepositorieAccounts, RepositorieAccounts>();
builder.Services.AddTransient<IRepositorieCategories, RepositorieCategories>();
builder.Services.AddTransient<IRepositorieTransactions, RepositorieTransactions>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout=TimeSpan.FromMinutes(10);
});

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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
