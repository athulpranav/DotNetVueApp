
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mock product data
var products = new List<Product>();
var categories = new[] { "Electronics", "Books", "Clothing", "Toys", "Home" };
var rnd = new Random();
for (int i = 1; i <= 25; i++)
{
    var cat = categories[rnd.Next(categories.Length)];
    products.Add(new Product
    {
        Id = i,
        Name = $"Product {i}",
        Price = Math.Round(rnd.NextDouble() * 100 + 10, 2),
        Category = cat,
        ImageUrl = $"https://placehold.co/400x400?text=Product+{i}",
        IsInStock = rnd.Next(2) == 1
    });
}

// GET /api/products?search=&category=&minPrice=&maxPrice=&page=&pageSize=
app.MapGet("/api/products", (
    [FromQuery] string? search,
    [FromQuery] string? category,
    [FromQuery] double? minPrice,
    [FromQuery] double? maxPrice,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10
) =>
{
    var query = products.AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
    if (!string.IsNullOrWhiteSpace(category))
        query = query.Where(p => p.Category == category);
    if (minPrice.HasValue)
        query = query.Where(p => p.Price >= minPrice.Value);
    if (maxPrice.HasValue)
        query = query.Where(p => p.Price <= maxPrice.Value);
    var total = query.Count();
    var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    return Results.Ok(new { total, items });
});

// GET /api/products/{id}
app.MapGet("/api/products/{id:int}", ([FromRoute] int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null) return Results.NotFound();
    return Results.Ok(product);
});

// GET /api/products/{id}/related
app.MapGet("/api/products/{id:int}/related", ([FromRoute] int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null) return Results.NotFound();
    var related = products.Where(p => p.Category == product.Category && p.Id != id)
        .OrderBy(_ => rnd.Next()).Take(4).ToList();
    return Results.Ok(related);
});

app.Run();

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsInStock { get; set; }
}
