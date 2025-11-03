using FabricWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Fabric details
string server = "6yhelygfwkyubovlpdcabb67qm-sq4iyhf5s7pe5kiegzkfmsfjfi.datawarehouse.fabric.microsoft.com";
string database = "AdventureWorksLT_Mirrored";

// Add services to the container.
builder.Services.AddSingleton(new FabricDbService(server, database));
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var app = builder.Build();

// ✅ Enable session BEFORE routing
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
