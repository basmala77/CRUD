using CRUD_Operations.Data;
using CRUD_Operations.Middelware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryPatternWithUOW.Core.Interface;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.EF;
using RepositoryPatternWithUOW.EF.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b=>b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
//builder.Services.AddTransient(typeof(IBaseRepository<>),typeof(BaseRepository<>));    
builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();   
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ProfileIloggerMiddelware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
