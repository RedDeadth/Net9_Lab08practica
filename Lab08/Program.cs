using Lab08.Data;
using Lab08.Repositories;
using Lab08.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con PostgreSQL
builder.Services.AddDbContext<LINQExampleContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Unit of Work (que crea los repositorios internamente)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registrar Repositorios también para uso directo si se necesita
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

// Registrar Servicios
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService, ProductService>(); 
builder.Services.AddScoped<IOrderService, OrderService>();


// Add services to the container.
builder.Services.AddControllers();

// Configurar Swagger con documentación detallada
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "LINQ Lab API", 
        Version = "v1",
        Description = "API para ejercicios de LINQ con PostgreSQL usando Repository Pattern y Unit of Work",
        Contact = new OpenApiContact
        {
            Name = "Lab08 - LINQ Exercises"
        }
    });
    
    // Habilitar comentarios XML si existen
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LINQ Lab API V1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz (http://localhost:5000)
    c.DocumentTitle = "LINQ Lab API Documentation";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();