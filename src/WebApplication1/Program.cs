var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Map("/test", applicationBuilder =>
{
    applicationBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});
app.MapGet("/", () => "Hello World!").WithName("ttt");
app.MapGet("/links", (LinkGenerator generator) =>
{
    var link = generator.GetPathByName("products", new { id = 5 });
    return link;
});
app.MapGet("/products/{id:int}", (int id) => $"Product id: {id}").WithName("products");
app.MapControllers();
app.Run();