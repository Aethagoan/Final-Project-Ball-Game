using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;


// web app stuff
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();


// get stuff from database storage jsons
var clients = JsonObject.Parse(File.ReadAllText("../memory/clients.json"));
var items = JsonObject.Parse(File.ReadAllText("../memory/items.json"));


// Console.WriteLine(clients["exampletoken"]); // this is how we get values out


// no firewall yeeet
app.UseCors(policy => 
    policy
        .AllowAnyOrigin()
            .AllowAnyMethod()
                .AllowAnyHeader()
);




// get
app.MapGet("/random", () => {
    return new { message = "hey this is working"};
    });



app.MapPost("/newcontract", async (contractEntry recieved) => {
    //we should get the right thing in the right format, so here goes nothing?
    
    //update program's copy
    clients = JsonObject.Parse(File.ReadAllText("../memory/clients.json"));


    // Console.WriteLine("new contract recieved!");
    spectatorJSON newclient = new spectatorJSON();

    newclient.alias = recieved.alias;
    newclient.renown = 0;




    newclient.pop_corns = recieved.pop_corns;
    newclient.QFS = recieved.QFS;
    newclient.DES = recieved.DES;
    newclient.TLD = recieved.TLD;
    newclient.CIA = recieved.CIA;
    newclient.EPS = recieved.EPS;
    newclient.IFS = recieved.IFS;


    string newToken = generate_random_token();

    // Console.WriteLine(JsonSerializer.Serialize(newclient));

    string clientsstring = JsonSerializer.Serialize(clients);
    string newclientstring = JsonSerializer.Serialize(newclient);

    // Console.WriteLine(newclientstring);

    newclientstring = newclientstring.Replace("\"inventory\":null,", "\"inventory\": { \"slot1\":\"\",\"slot2\":\"\",\"slot3\":\"\",\"slot4\":\"\",\"slot5\":\"\"},");

    clientsstring = clientsstring[0..^1]; // remove the last } to add back in later.
    clientsstring += ","+  "\"" + newToken + "\"" + ":" + newclientstring + "}";

    // Console.WriteLine(clientsstring);

    //write!
    File.WriteAllText("../memory/clients.json", clientsstring);

    return new {token = newToken};
});


app.MapPost("/token", (tokenString recievedtoken) => {
    return JsonSerializer.Serialize(getToken(recievedtoken.token));
});













app.Run();











// functions
JsonNode getToken(string token) {

    clients = JsonObject.Parse(File.ReadAllText("../memory/clients.json"));

    return clients[token];
}

string generate_random_token() {

    string tokenCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    string returnme = "";

    for (int i = 0; i < 20; i++){
        returnme += tokenCharacters[new Random().Next(tokenCharacters.Length)];
    }

    return returnme;
}







// Data Structures
public class tokenString {
    public string token {get; set;}
} 

public class contractEntry {
    public string alias {get; set;}
    public long pop_corns {get; set;}
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

    public long pop_corns {get; set;}

    public bool QFS {get; set;}
    public bool DES {get; set;}
    public bool TLD {get; set;}
    public bool CIA {get; set;}
    public bool EPS {get; set;}
    public bool IFS {get; set;}

    public spectatorJSON() {}
}

public class inventoryJSON {
    public string slot1 {get; set;}
    public string slot2 {get; set;}
    public string slot3 {get; set;}
    public string slot4 {get; set;}
    public string slot5 {get; set;}

    public inventoryJSON(){
        slot1 = "";
        slot2 = "";
        slot3 = "";
        slot4 = "";
        slot5 = "";
    }

}

public class itemJSON {
    public string name {get; set;}
    public string img {get; set;}
    public string desc {get; set;}

    public itemJSON() {
        name = "";
        img = "";
        desc = "";
    }

}