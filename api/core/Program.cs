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


app.MapPost("/token", async (tokenString recievedtoken) => {
    return JsonSerializer.Serialize(getToken(recievedtoken.token));
});


app.MapGet("/teams", async () => {
    return (File.ReadAllText("../memory/teams.json"));
});


app.MapGet("/game", async () => {
    return (File.ReadAllText("../memory/gamestate.json"));
});





// executes in the background
Task.Run(() => RunGame());





app.Run();



// game loop? needs to run async.
Task RunGame(){

    gameState game_state = JsonSerializer.Deserialize<gameState>(File.ReadAllText("../backups/gamestate.json"));

    var All_Team_Data = JsonObject.Parse(File.ReadAllText("../memory/teams.json"));

    // repeat this indefinately.
    while (true){
        game("Team 1", "Team 2");
        game("Team 2", "Team 1");
    }

    return Task.CompletedTask;

    async void game(string team1name, string team2name){
        game_state = JsonSerializer.Deserialize<gameState>(File.ReadAllText("../backups/gamestate.json"));

        game_state.HomeTeam = team1name;
        game_state.AwayTeam = team2name;

        var Team_1 = All_Team_Data[team1name];
        var Team_2 = All_Team_Data[team2name];

        LinkedList<JsonNode> Team_1_Batters = new LinkedList<JsonNode>();
        LinkedList<JsonNode> Team_2_Batters = new LinkedList<JsonNode>();
        LinkedList<JsonNode> Team_1_Fielders = new LinkedList<JsonNode>();
        LinkedList<JsonNode> Team_2_Fielders = new LinkedList<JsonNode>();

        for(int i = 1; i < 12; i++){
            Team_1_Batters.AddLast(Team_1[$"Player{i}"]);
            Team_1_Fielders.AddLast(Team_1[$"Player{i}"]);
            Team_2_Batters.AddLast(Team_2[$"Player{i}"]);
            Team_2_Fielders.AddLast(Team_2[$"Player{i}"]);
        }
        
        LinkedListNode<JsonNode>Team_1_Batter_Up = Team_1_Batters.First;
        LinkedListNode<JsonNode>Team_1_Pitcher_Up = Team_1_Fielders.First;
        LinkedListNode<JsonNode>Team_2_Batter_Up = Team_2_Batters.First;
        LinkedListNode<JsonNode>Team_2_Pitcher_Up = Team_2_Fielders.First;

        
        // maybe a line here that is while gamestate.inning.count is < 10?
        game_state.inning.count++;
        game_state.inning.orientation = "top";
        halfinning(Team_1, Team_1_Batter_Up, Team_2, Team_2_Pitcher_Up);
        game_state.inning.orientation = "bottom";
        halfinning(Team_2, Team_2_Batter_Up, Team_1, Team_1_Pitcher_Up);

        void halfinning(JsonNode inteam, LinkedListNode<JsonNode> batter, JsonNode outteam, LinkedListNode<JsonNode> pitcher){
            
            File.WriteAllText("../memory/gamestate.json", JsonSerializer.Serialize(game_state));

            //while outs < 3
            round();

            void round() {

                // Console.WriteLine(JsonSerializer.Serialize(batter.Value["bat_score"]));
                game_state.firstbasepitcher = JsonSerializer.Serialize(pitcher.Value["alias"]);
                game_state.secondbasepitcher = JsonSerializer.Serialize(pitcher.Next != null ? pitcher.Next.Value["alias"] : pitcher.List.First.Value["alias"]);
                game_state.batter = JsonSerializer.Serialize(batter.Value["alias"]);
                write_to_play(JsonSerializer.Serialize(batter.Value["alias"]) + " lines up to bat.");
                
                pitcher = pitcher.Next != null ? pitcher.Next : pitcher.List.First;
                batter = batter.Next != null ? batter.Next : batter.List.First;

                void pitch() {
                    // until 3 strikes, 4 balls, or a hit (break)
                    int thrown = 0;
                    while(true){
                        if (thrown >= 6){
                            // rotate pitchers
                            pitcher = pitcher.Next ?? pitcher.List.First;
                            write_to_play("The pitchers are rotating.");
                            game_state.firstbasepitcher = JsonSerializer.Serialize(pitcher.Value["alias"]);
                            game_state.secondbasepitcher = JsonSerializer.Serialize(pitcher.Next != null ? pitcher.Next.Value["alias"] : pitcher.List.First.Value["alias"]);
                        }

                        if (game_state.strikes >= 3){
                            // out
                            // rotate batters, break
                            batter = batter.Next ?? batter.List.First;
                        }

                        if (game_state.balls >= 4) {
                            // walk
                        }

                        // pitch
                        if (game_state.onbase == "first"){
                            // pitcher on second throws (pitcher.next)
                        }

                        if (game_state.onbase == "second"){
                            // pitcher on first throws (pitcher)
                        }
                    }
                    //

                }

            }

            void write_to_play(string message) {
                game_state.play = message;
                File.WriteAllText("../memory/gamestate.json", JsonSerializer.Serialize(game_state));
                Thread.Sleep(5*1000); // wait 5 seconds
            }
            
        }

    }

    
}






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

public class gameState {
    public string HomeTeam {get; set;}
    public string AwayTeam {get; set;}
    public currentinning inning {get; set;}
    public int HomeScore {get; set;} 
    public int AwayScore {get; set;}
    public int outs {get; set;}
    public int strikes {get; set;}
    public int balls {get; set;}
    public string batter {get; set;}
    public string onbase {get; set;}
    public string firstbasepitcher {get; set;}
    public string secondbasepitcher {get; set;}
    public string play {get; set;}
}

public class currentinning {
    public string orientation {get; set;}
    public int count {get; set;}
}