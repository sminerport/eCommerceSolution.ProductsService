using eCommerce.API.Middleware;
using FluentValidation.AspNetCore;
using eCommerce.ProductServices.DataAccessLayer;
using eCommerce.ProductServices.BusinessLogicLayer;
using eCommerce.ProductsMicroService.API.APIEndpoints;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(lb => { lb.AddConsole(); lb.AddDebug(); });

// Add infrastructure services
builder.Services.AddDataAccessLayer(builder.Configuration);
// Add core services to the container.
builder.Services.AddBusinessLogicLayer();
// Add controllers to the service collection
builder.Services.AddControllers();

// FluentValidations
builder.Services.AddFluentValidationAutoValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandlingMiddleware();
}

// Routing
app.UseRouting();

// Enable CORS
app.UseCors();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Auth
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapProductAPIEndpoints();

// Controller routes
app.MapControllers();

app.Run();