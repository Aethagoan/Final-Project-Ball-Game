var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();

app.UseCors(policy => 
    policy
        .AllowAnyOrigin()
            .AllowAnyMethod()
                .AllowAnyHeader()
);

app.MapGet("/", () => {
    return new {
        title = "Hello World!", 
        message = "test"
        };
    });

app.MapPost("/", (RecievedJSON recieved) => {
    Console.WriteLine(recieved.text);
    return new {
        message = "recieved"
    };
});

app.Run();

class RecievedJSON {
    public string text { get; set;}
}