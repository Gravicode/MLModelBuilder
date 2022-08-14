using ModelBuilder.Web.Data;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Blazored.Toast;
using ModelBuilder.Tools;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using ModelBuilder.Web.Helpers;
using MudBlazor;
using ModelBuilder.Models;
using ModelBuilder.Web.Services;

var builder = WebApplication.CreateBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//end swagger
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// ******
// BLAZOR COOKIE Auth Code (begin)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
// BLAZOR COOKIE Auth Code (end)
// ******
// ******
// BLAZOR COOKIE Auth Code (begin)
// From: https://github.com/aspnet/Blazor/issues/1554
// HttpContextAccessor
//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddTransient<AzureBlobHelper>();
builder.Services.AddSingleton<UserProfileService>();
builder.Services.AddTransient<MLModelService>();
builder.Services.AddTransient<UserProfileService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET, PATCH, DELETE, PUT, POST, OPTIONS"));
});
var configBuilder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false);
IConfiguration Configuration = configBuilder.Build();



builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

MailService.MailUser = Configuration["MailSettings:MailUser"];
MailService.MailPassword = Configuration["MailSettings:MailPassword"];
MailService.MailServer = Configuration["MailSettings:MailServer"];
MailService.MailPort = int.Parse(Configuration["MailSettings:MailPort"]);
MailService.SetTemplate(Configuration["MailSettings:TemplatePath"]);
MailService.SendGridKey = Configuration["MailSettings:SendGridKey"];
MailService.UseSendGrid = true;


SmsService.UserKey = Configuration["SmsSettings:ZenzivaUserKey"];
SmsService.PassKey = Configuration["SmsSettings:ZenzivaPassKey"];
SmsService.TokenKey = Configuration["SmsSettings:TokenKey"];



builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 128 * 1024; // 1MB
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    //swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// ******
// BLAZOR COOKIE Auth Code (begin)
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
// BLAZOR COOKIE Auth Code (end)
// ******

app.UseCors(x => x
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true) // allow any origin  
.AllowCredentials());               // allow credentials 

// BLAZOR COOKIE Auth Code (begin)
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
// BLAZOR COOKIE Auth Code (end)

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var db = new ModelBuilderDB();
db.Database.EnsureCreated();

//inference API
app.MapPost("/batch-inference", async (InferenceModelParam data) =>
{
    try
    {
        BatchInferenceService svc = new BatchInferenceService(data.ModelId);
        var res = await svc.Inference(data);        
        return Results.Ok(res);
    }
    catch (Exception ex)
    {
        return Results.Problem( ex.ToString());
    }  
}).WithName("BatchInference");


app.Run();

