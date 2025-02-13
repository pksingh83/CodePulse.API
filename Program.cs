using CodePulse.API.Data;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interace;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.addsin

//builder.Services.AddAuthentication("").AddJwtBearer("", options => {
//    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//    };
//});

//builder.Services.AddAuthentication("").AddOAuth("", options =>
//{
//    //options.ClaimsIssuer = new[] { };
//});

// configure cookies as below
//builder.Services.Configure<CookiePolicyOptions>(options => {
//    options.MinimumSameSitePolicy = SameSiteMode.Strict;
//    options.Secure = CookieSecurePolicy.Always;
//});

//builder.Services.AddCookiePolicy(options => {
//    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
//    options.MinimumSameSitePolicy = SameSiteMode.Strict;
//    options.Secure = CookieSecurePolicy.Always;
//    options.CheckConsentNeeded = context => true;
//});

//builder.Services.AddSession(options => 
//{
//    options.IOTimeout = new TimeSpan(1000);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//var claim = new List<Claim>
//{
//    new Claim(ClaimTypes.Role, ""),
//};


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));
});

//builder.Configuration.

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//builder.WebHost.ConfigureKestrel(options => { options. })

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //app.UseExceptionHandler("/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// enforece cookie policy
//app.UseCookiePolicy();
//app.UseSession();

//app.Map()

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next(); // Call the next middleware
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});


//app.UseCookiePolicy();

//app.UseRouting();
//app.UseRateLimiter();
//app.UseRequestLocalization();


app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseSession();
//app.UseResponseCompression();
//app.UseResponseCaching();

app.MapControllers();

//app.MapFallbackToFile("/app/index.html");

app.Run();
