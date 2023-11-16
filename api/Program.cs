using System;
using System.Collections.Generic;

// web app stuff
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();

List<jsonListItem> todo_list = new ();

app.UseCors(policy => 
    policy
        .AllowAnyOrigin()
            .AllowAnyMethod()
                .AllowAnyHeader()
);


// get
app.MapGet("/", () => {
    return todo_list;
    });

// post
app.MapPost("/", (jsonListItem recieved) => {
    Console.WriteLine(recieved.name);
    Console.WriteLine(recieved.description);
    // this is where the API uses the data/posts the data
    todo_list.Add(recieved);

    return new {
        message = " post recieved"
    };
});

app.Run();


// my data structures
class RecievedJSON {
    public string text { get; set;}
}

public class jsonListItem {
    public string name { get; set;}
    public string description { get; set;}
}