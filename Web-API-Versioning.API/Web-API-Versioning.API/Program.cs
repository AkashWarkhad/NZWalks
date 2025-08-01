using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Threading.Tasks;
using Web_API_Versioning.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//111111111111111111111111111111111111 Cofigure Api Versioning 111111111111111111111111111111111111
// Configuration to add the Api Versioning
builder.Services.AddApiVersioning(opt=>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});


//111111111111111111111111111111111111 Cofigure Api Versioning on Swagger 111111111111111111111111111111111111
// Configures API Explorer to group Swagger docs by version (e.g., v1) and automatically insert the API version into route URLs.
builder.Services.AddVersionedApiExplorer(opt => 
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the Swagger options for version
// This ensures Swagger will generate a separate UI and JSON file for each API version defined in your controllers.
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// After App Builds then We want to register them for all versions in the Swagger
var versionDescriptionProvider = 
    app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Registering all version into swagger to show it on the Swagger document 
    //In Swagger UI, you’ll see a version selector (v1, v2...) to explore each version separately.
    app.UseSwaggerUI(opt =>
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
