using GRC_NewClientPortal.Models.Domain;
using GRC_NewClientPortal.Models.GRCEmail;
using GRC_NewClientPortal.Models.Security;
//using GRC_NewClientPortal.Models.GRCEmail.ENTSendEmailManager;

var builder = WebApplication.CreateBuilder(args);

// Example if you’re using EF Core
//builder.Services.AddDbContext<MyDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDataSource")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
// Add services for session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<Logon>();
//builder.Services.AddSingleton<ENTSsendEmailManager>();

SystemAvailability.Initialize(builder.Configuration);

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

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=ClientHome}/{id?}");

app.Run();