using Microsoft.EntityFrameworkCore;
using MinimalAPICatalogo.Context;
using MinimalAPICatalogo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultCOnnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

var app = builder.Build();

//Inicio dos Endpoints de Categoria
app.MapGet("/categorias", async (AppDbContext _context) => await _context.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (AppDbContext _context, int id) =>
{
    return await _context.Categorias.FindAsync(id)
        is Categoria categoria
            ? Results.Ok(categoria)
            : Results.NotFound($"Categoria com id {id} não pôde ser localizada");
});

app.MapPost("/categorias", async (AppDbContext _context, Categoria categoria) =>
{
    _context.Categorias.Add(categoria);
    await _context.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapPut("/categorias/{id:int}", async (AppDbContext _context, Categoria categoria, int id) =>
{
    if (categoria.CategoriaId != id) return Results.BadRequest("O id informado no header é diferento do body");

    var categoriaDB = await _context.Categorias.FindAsync(id);
    if (categoriaDB is null) return Results.NotFound($"Nenhuma categoria com o id {id} foi encontrada na base de dados");

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await _context.SaveChangesAsync();
    return Results.Ok(categoria);
});

app.MapDelete("/categorias/{id:int}", async (AppDbContext _context, int id) =>
{
    var categoria = await _context.Categorias.FindAsync(id);
    if (categoria is null) return Results.NotFound($"Categoria com id {id} não encontrada");

    _context.Categorias.Remove(categoria);
    await _context.SaveChangesAsync();

    return Results.NoContent();
});
//Fim dos Endpoints de Categoria

//Inicio dos Endpoints de Produto
app.MapGet("/produtos", async (AppDbContext _context) => await _context.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (AppDbContext _context, int id) =>
{
    return await _context.Produtos.FindAsync(id)
        is Produto produto
            ? Results.Ok(produto)
            : Results.NotFound($"Produto com id {id} não foi encotrado");
});

app.MapPost("/produtos", async (AppDbContext _context, Produto produto) =>
{
    _context.Produtos.Add(produto);
    await _context.SaveChangesAsync();
    return Results.Ok(produto);
});

app.MapPut("/produtos/{id:int}", async (AppDbContext _context, Produto produto, int id) =>
{
    if (produto.ProdutoId != id) return Results.BadRequest("O id informado no header é diferente do body");

    var produtoDB = await _context.Produtos.FindAsync(id);
    if (produtoDB is null) return Results.NotFound($"O produto com id {id} não pôde ser encontrado na base de dados");

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao = produto.Descricao;
    produtoDB.Preco = produto.Preco;
    produtoDB.Imagem = produto.Imagem;
    produtoDB.Compra = produto.Compra;
    produtoDB.Estoque = produto.Estoque;
    produtoDB.CategoriaId = produto.CategoriaId;


    await _context.SaveChangesAsync();
    return Results.Ok(produtoDB);
});

app.MapDelete("/produtos/{id:int}", async (AppDbContext _context, int id) =>
{
    var produto = await _context.Produtos.FindAsync(id);
    if (produto is null) return Results.NotFound($"O produto com id {id} não pôde ser encontrado na base de dados");

    _context.Produtos.Remove(produto);
    await _context.SaveChangesAsync();
    return Results.NoContent();
});
//Fim dos Endpoints de Produto

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
