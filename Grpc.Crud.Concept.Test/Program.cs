using Grpc.Crud.Concept.Test.Middlewere;
using Grpc.Crud.Concept.Test.Repositories;
using Grpc.Crud.Concept.Test.Services;
using GrpcServiceCrud.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ErrorHandelingInterceptor>();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ErrorHandelingInterceptor>();
});

builder.Services.AddScoped<IProductService, ProductServiceImplementation>();

builder.Services.AddDbContext<DbContext, DbContextClass>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Grpc.Crud.Concept.Test")), ServiceLifetime.Transient);

builder.Services.AddAutoMapper(typeof(Program)); 



var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ProductService>();


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();