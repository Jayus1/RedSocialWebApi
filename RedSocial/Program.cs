using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RedSocial.Data;
using RedSocial.Servicios;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUsuarioData,UsuarioData>();
builder.Services.AddTransient<IPostsData,PostsData>();
builder.Services.AddTransient<IComentariosData,ComentariosData>();
builder.Services.AddTransient<IReaccionesData,ReaccionesData>();
builder.Services.AddScoped<ITokenService,TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuer=false,
        ValidateAudience= false,
        ValidateLifetime=true,
        //ValidIssuer= builder.Configuration["JWT:Issuer"],
        //ValidAudience= builder.Configuration["JWT:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    }

);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
