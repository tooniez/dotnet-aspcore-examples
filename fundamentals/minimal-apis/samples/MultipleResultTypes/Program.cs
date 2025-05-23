using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<List<Book>>((sp) =>
{
    return new(){
        new Book(1, "Testing Dot", "Carson Alexander"),
        new Book(2, "Learn Linq", "Meredith Alonso"),
        new Book(3, "Generics", "Arturo Anand"),
        new Book(4, "Testing the Mic", "Gytis Barzdukas"),
        new Book(5, "Drop the Dot", "Van Li"),
    };
});
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

// <snippet_multiple_result_types>
app.MapGet("/book/{id}", Results<Ok<Book>, NotFound> 
    (int id, List<Book> bookList) =>
    {
        return bookList.FirstOrDefault((i) => i.Id == id) is Book book
        ? TypedResults.Ok(book)
        : TypedResults.NotFound();
    });
// </snippet_multiple_result_types>

// <snippet_single_result_type>
app.MapGet("/books", (List<Book> bookList) => TypedResults.Ok(bookList));
// </snippet_single_result_type>

app.Run();

record Book(int Id, string Title, string Author);
