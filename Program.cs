namespace OsnoveCSharp6_API3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var blogovi = new List<Blog> {
                new Blog {Naslov = "Prvi post", Telo = "Tekst prvi post"},
                new Blog {Naslov = "Drugi post", Telo = "Tekst drugi post"}
            };

            app.MapGet("/", () => "Hello World!");
            // Ruta ka svim blogovima:
            app.MapGet("/blogovi", () => { return blogovi; });
            // Ruta ka konkretnom blogu:
            app.MapGet
                ("/blogovi/{blogID}", (int blogID) => {
                if (blogID < 0 || blogID >= blogovi.Count)
                {
                    // Da bi sve radilo moramo da koristimo metode iz klase Results kako bi vratili kroz anonimnu metodu tacno odredjen tip podatka
                    return Results.NotFound();
                }
                else
                {
                    
                    return Results.Ok(blogovi[blogID]);     
                }
            });
            // Ruta za dodavanje postova na blogu:
            app.MapPost
                (
                "/blogovi", (Blog noviBlog) =>
                {
                    blogovi.Add(noviBlog);
                    return Results.Created($"/blogovi/{blogovi.Count - 1}", noviBlog);
                }
                );
            // Ruta za uklanjanje posta sa bloga:
            app.MapDelete
                (
                "/blogovi/{blogID}", (int blogID) =>
                {
                    if (blogID < 0 || blogID >= blogovi.Count)
                    {
                        return Results.NotFound();
                    }
                    else
                    {
                        blogovi.RemoveAt(blogID);
                        return Results.NoContent();
                    }
                }
                );
            // Ruta za editovanje posta
            app.MapPut
                (
                "/blogovi/{blogID}", (int blogID, Blog blog) =>
                {
                    if (blogID < 0 || blogID >= blogovi.Count)
                    {
                        return Results.NotFound();
                    }
                    else
                    {
                        blogovi[blogID] = blog;
                        return Results.Ok();
                    }
                }
                );
            app.Run();
        }
        public class Blog
        {
            public required string Naslov { get; set;  }
            public required string Telo {  get; set; }
        }
    }
}
