using Microsoft.EntityFrameworkCore;
using Villa_API.Dto;
using Villa_API.Repository;
using Villa_API.Repository.IRepository;
using Villa_API.Store;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// agregar servicio para base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// mapeador de objetos
builder.Services.AddAutoMapper(typeof(MappingConfig));
// se agrega servicio de Villa repository
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
// se agrega servicio de NumeroVilla repository
builder.Services.AddScoped<INumeroVillaRepository, NumeroVillaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
