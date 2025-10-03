using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AVritmica.BD.Data;
using AVritmica.Server.Repositorio;

//-----------------------------------------------------------------------------------------------------------------
//Configuracion de los servicios en el constructor de la aplicacion
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(
   x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));  // Local instance MySQL83

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ICarritoRepositorio, CarritoRepositorio>();
builder.Services.AddScoped<ICarritoProductoRepositorio, CarritoProductoRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<ICompraRepositorio, CompraRepositorio>();
builder.Services.AddScoped<ICompraDetalleRepositorio, CompraDetalleRepositorio>();
builder.Services.AddScoped<IConsultaRepositorio, ConsultaRepositorio>();
builder.Services.AddScoped<IPagoRepositorio, PagoRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IStockMovimientoRepositorio, StockMovimientoRepositorio>();
//builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

//-----------------------------------------------------------------------------------------------------------------
//Construccion de la aplicacion
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
