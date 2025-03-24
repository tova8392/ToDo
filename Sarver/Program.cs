using Microsoft.EntityFrameworkCore;
using TodoApi;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// הוספת חיבור למסד נתונים
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ToDoDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))
    ));

// הגדרת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// הפעלת CORS
app.UseCors("AllowAllOrigins");

// הפעלת Swagger רק במצב פיתוח
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Routes
app.MapGet("/", async (ToDoDbContext db) => await db.Items.ToListAsync())
    .WithName("GetAllItems")
    .WithTags("Items");

app.MapGet("/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
    return item != null ? Results.Ok(item) : Results.NotFound();
})
    .WithName("GetItemById")
    .WithTags("Items");

app.MapPost("/addItem", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/addItem/{item.Id}", item);
})
    .WithName("AddItem")
    .WithTags("Items");

app.MapPut("/updateItem/{id}", async (ToDoDbContext db, int id, Item item) =>
{
    var i = await db.Items.FindAsync(id);
    if (i == null) return Results.NotFound();
    
    i.IsComplete = item.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(i);
})
    .WithName("UpdateItem")
    .WithTags("Items");

app.MapDelete("/removeItem/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
    if (item == null) return Results.NotFound();
    
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
    .WithName("DeleteItem")
    .WithTags("Items");

app.Run();
