using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AiMoodCompanion.Api.Data;
using AiMoodCompanion.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"] ?? "YourSuperSecretKeyHere12345678901234567890")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Services
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<MoodAnalysisService>();
builder.Services.AddSingleton<MLMoodService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger'ı her zaman aktif et (Development ve Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AiMoodCompanion API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Middleware ile otomatik browser açma
app.Use(async (context, next) =>
{
    if (app.Environment.IsDevelopment() && context.Request.Path == "/")
    {
        // Ana sayfa isteği geldiğinde Swagger'a yönlendir
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});

// Otomatik olarak browser'da Swagger'ı aç
if (app.Environment.IsDevelopment())
{
    // Uygulama başladıktan sonra browser'ı aç
    var hostLifetime = app.Services.GetService<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();
    if (hostLifetime != null)
    {
        hostLifetime.ApplicationStarted.Register(() =>
        {
            Task.Delay(2000).ContinueWith(_ =>
            {
                try
                {
                    // Tam Swagger URL'ini kullan
                    var url = "http://localhost:5000/swagger/index.html";
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch
                {
                    // Browser açılamazsa sessizce devam et
                }
            });
        });
    }
}

app.Run();
