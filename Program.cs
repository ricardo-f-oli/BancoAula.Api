using BancoAula.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("Jwt");
var chave = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddControllers();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<BancoContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(chave)
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Padrao")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Cole o token JWT do login."
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

    o.OperationFilter<AuthorizeOperationFilter>();
});

builder.Services.AddScoped<IContaService, ContaService>();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();
app.UseMiddleware<ErroMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public class AuthorizeOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
{
    public void Apply(OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
    {
        var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var exigeAuth = metadata.OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>().Any()
                        && !metadata.OfType<Microsoft.AspNetCore.Authorization.IAllowAnonymous>().Any();
        if (!exigeAuth) return;

        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>()
        });
    }
}
