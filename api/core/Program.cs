using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;


// web app stuff
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();


// get stuff from database storage jsons

string clientsJSON = File.ReadAllText("../memory/clients.json");
Console.WriteLine(clientsJSON);


// no firewall yeeet
app.UseCors(policy => 
    policy
        .AllowAnyOrigin()
            .AllowAnyMethod()
                .AllowAnyHeader()
);




/*
// get
app.MapGet("/", () => {
    return todo_list;
    });
*/

/*
// post
app.MapPost("/", (jsonListItem recieved) => {
    // Console.WriteLine(recieved.name);
    // Console.WriteLine(recieved.description);
    // this is where the API uses the data/posts the data
    todo_list.Add(recieved);

//     return new {
//         message = " post recieved"
//     };
// });

*/




app.MapGet("/token", (tokenString token) => {
    
});


app.MapPost("/newcontract", (contractEntry recieved) => {

});







app.Run();



// functions
// async spectatorJSON getToken(string token) {



//     string token_contents = JsonSerializer.Serialize()

// }



// Data Structures
public class tokenString {
    public string token {get; set;}
} 

public class contractEntry {
    public string token {get; set;}
    public string alias {get; set;}
    public int pop_corns {get; set;}
    public bool QFS {get; set;}
    public bool DES {get; set;}
    public bool TLD {get; set;}
    public bool CIA {get; set;}
    public bool EPS {get; set;}
    public bool IFS {get; set;}
}

public class spectatorJSON {
    public string alias {get; set;}
    public int renown {get; set;}
    public string favoriteteam {get; set;}
        
    public inventoryJSON inventory {get; set;}

    public int pop_corns {get; set;}

    public bool QFS {get; set;}
    public bool DES {get; set;}
    public bool TLD {get; set;}
    public bool CIA {get; set;}
    public bool EPS {get; set;}
    public bool IFS {get; set;}
}

public class inventoryJSON {
    public itemJSON slot1 {get; set;}
    public itemJSON slot2 {get; set;}
    public itemJSON slot3 {get; set;}
    public itemJSON slot4 {get; set;}
    public itemJSON slot5 {get; set;}
    
}

public class itemJSON {
    public string name {get; set;}
    public string img {get; set;}
    public string desc {get; set;}
}