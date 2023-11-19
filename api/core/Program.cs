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
app.MapGet("/random", () =>
{
    return new { message = "hey this is working" };
});



app.MapPost("/newcontract", async (contractEntry recieved) =>
{
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
    clientsstring += "," + "\"" + newToken + "\"" + ":" + newclientstring + "}";

    // Console.WriteLine(clientsstring);

    //write!
    File.WriteAllText("../memory/clients.json", clientsstring);

    return new { token = newToken };
});


app.MapPost("/token", async (tokenString recievedtoken) =>
{
    return JsonSerializer.Serialize(getToken(recievedtoken.token));
});


app.MapGet("/teams", async () =>
{
    return (File.ReadAllText("../memory/teams.json"));
});


app.MapGet("/game", async () =>
{
    return (File.ReadAllText("../memory/gamestate.json"));
});





// executes in the background
Task.Run(() => RunGame());





app.Run();



// game loop? needs to run async.
Task RunGame()
{

    gameState game_state = JsonSerializer.Deserialize<gameState>(File.ReadAllText("../backups/gamestate.json"));

    var All_Team_Data = JsonObject.Parse(File.ReadAllText("../memory/teams.json"));

    // repeat this indefinately.
    while (true)
    {
        game("Team 1", "Team 2");
        game("Team 2", "Team 1");
    }

    return Task.CompletedTask;

    async void game(string team1name, string team2name)
    {
        Console.WriteLine("Game has started");
        game_state = JsonSerializer.Deserialize<gameState>(File.ReadAllText("../backups/gamestate.json"));

        game_state.HomeTeam = team1name;
        game_state.AwayTeam = team2name;

        var Team_1 = All_Team_Data[team1name];
        var Team_2 = All_Team_Data[team2name];

        LinkedList<JsonNode> Team_1_Batters = new LinkedList<JsonNode>();
        LinkedList<JsonNode> Team_2_Batters = new LinkedList<JsonNode>();
        LinkedList<JsonNode> Team_1_Fielders = new LinkedList<JsonNode>();
        LinkedList<JsonNode> Team_2_Fielders = new LinkedList<JsonNode>();

        for (int i = 1; i < 12; i++)
        {
            Team_1_Batters.AddLast(Team_1[$"Player{i}"]);
            Team_1_Fielders.AddLast(Team_1[$"Player{i}"]);
            Team_2_Batters.AddLast(Team_2[$"Player{i}"]);
            Team_2_Fielders.AddLast(Team_2[$"Player{i}"]);
        }

        LinkedListNode<JsonNode> Team_1_Batter_Up = Team_1_Batters.Last;
        LinkedListNode<JsonNode> Team_1_Pitcher_Up = Team_1_Fielders.First;
        LinkedListNode<JsonNode> Team_2_Batter_Up = Team_2_Batters.Last;
        LinkedListNode<JsonNode> Team_2_Pitcher_Up = Team_2_Fielders.First;

        // we do the last on batters so it goes over to the first on startup

        // maybe a line here that is while gamestate.inning.count is < 10?
        while (game_state.inning.count < 10)
        {
            game_state.inning.count++;
            game_state.inning.orientation = "top";
            halfinning(team1name, Team_1_Batter_Up, team2name, Team_2_Pitcher_Up);
            game_state.inning.orientation = "bottom";
            halfinning(team2name, Team_2_Batter_Up, team1name, Team_1_Pitcher_Up);
        }


        void halfinning(string inteam, LinkedListNode<JsonNode> batter, string outteam, LinkedListNode<JsonNode> pitcher)
        {
            // outs reset
            game_state.outs = 0;



            File.WriteAllText("../memory/gamestate.json", JsonSerializer.Serialize(game_state));

            //while outs < 3
            while (true)
            {
                if (game_state.outs >= 3)
                {
                    write_to_play("Out 3! Changing sides. . .");
                    break;
                }
                round();
            }

            return;


            void round()
            {
                // strikes, balls reset
                game_state.strikes = 0;
                game_state.balls = 0;

                game_state.firstbasepitcher = JsonSerializer.Serialize(pitcher.Value["alias"]);
                game_state.secondbasepitcher = JsonSerializer.Serialize(pitcher.Next != null ? pitcher.Next.Value["alias"] : pitcher.List.First.Value["alias"]);
                
                
                batter = batter.Next != null ? batter.Next : batter.List.First;
                game_state.batter = JsonSerializer.Serialize(batter.Value["alias"]);
                game_state.onbase = "first";

                pitch();
                return;


                void pitch()
                {
                    write_to_play(JsonSerializer.Serialize(batter.Value["alias"]) + " lines up to bat.");

                    // until 3 strikes, 4 balls, or a hit (break)
                    int thrown = 0;
                    while (true)
                    {
                        if (thrown >= 6)
                        {
                            // rotate pitchers
                            pitcher = pitcher.Next ?? pitcher.List.First;
                            write_to_play("The pitchers are rotating.");
                            game_state.firstbasepitcher = JsonSerializer.Serialize(pitcher.Value["alias"]);
                            game_state.secondbasepitcher = JsonSerializer.Serialize(pitcher.Next != null ? pitcher.Next.Value["alias"] : pitcher.List.First.Value["alias"]);
                            thrown = 0;
                        }

                        if (game_state.strikes >= 3)
                        {
                            // out
                            game_state.outs++;
                            write_to_play(JsonSerializer.Serialize(batter.Value["alias"]) + " struck out.");
                            // rotate batters, break
                            write_to_play("Batters rotating.");
                            batter = batter.Next != null ? batter.Next : batter.List.First;
                            return;
                        }

                        if (game_state.balls >= 4)
                        {
                            // walk
                            write_to_play("Ball 4. " + JsonSerializer.Serialize(batter.Value["alias"]) + " walks.");
                            advance();
                            continue;
                        }

                        // pitch
                        if (game_state.onbase == "first")
                        {
                            // pitcher on second throws (pitcher.next)
                            JsonNode nextpitcher = pitcher.Next != null ? pitcher.Next.Value : pitcher.List.First.Value;
                            thrown++;

                            // if hurl ball returns false, break. otherwise continue.

                            if (!hurl_ball(nextpitcher, batter.Value))
                            {
                                break;
                            }
                            continue;

                        }

                        if (game_state.onbase == "second")
                        {
                            // pitcher on first throws (pitcher)
                            thrown++;

                            // if hurl ball returns false, break. otherwise continue.

                            if (!hurl_ball(pitcher.Value, batter.Value))
                            {
                                break;
                            }
                            continue;


                        }

                        bool hurl_ball(JsonNode thepitcher, JsonNode thebatter)
                        {

                            write_to_play(JsonSerializer.Serialize(thepitcher["alias"]) + " hurls the ball down the pitch. . .");

                            float pitchmod = gen_rand();

                            float pitchscore = float.Parse(JsonSerializer.Serialize(pitcher.Value["throw_score"]));

                            if (pitchmod * pitchscore < 1)
                            {
                                // wild
                                game_state.balls++;
                                write_to_play(JsonSerializer.Serialize(thepitcher["alias"]) + " throws wild. Ball.");

                                return true;
                            }

                            float batmod = gen_rand();

                            float batscore = float.Parse(JsonSerializer.Serialize(batter.Value["bat_score"]));

                            float hit_total = batmod * batscore;

                            if (hit_total <= pitchmod * pitchscore)
                            {
                                // strike
                                game_state.strikes++;
                                write_to_play(JsonSerializer.Serialize(thepitcher["alias"]) + " Throws a strike!");

                                return true;
                            }

                            if (hit_total > pitchmod * pitchscore)
                            {
                                //hit

                                int target = new Random().Next(pitcher.List.Count);
                                JsonNode feilder = pitcher.List.ElementAt<JsonNode>(target);

                                write_to_play("Ball hit towards " + JsonSerializer.Serialize(feilder["alias"]) + "!");

                                float runmod = gen_rand();

                                float runscore = float.Parse(JsonSerializer.Serialize(feilder["run_score"]));

                                float run_total = runmod * runscore;

                                if (hit_total <= run_total)
                                {
                                    //caught. // return false
                                    game_state.outs++;
                                    write_to_play(JsonSerializer.Serialize(feilder["alias"]) + " caught the ball. " + JsonSerializer.Serialize(thebatter["alias"]) + " is OUT!");
                                    return false;
                                }

                                if (hit_total > run_total)
                                {
                                    // not caught
                                    write_to_play(JsonSerializer.Serialize(feilder["alias"]) + " doesn't catch the ball!");
                                    write_to_play(JsonSerializer.Serialize(feilder["alias"]) + " throws the ball to " + (game_state.onbase == "first" ? "second!" : "first!"));
                                    float throwmod = gen_rand();

                                    float throw_score = float.Parse(JsonSerializer.Serialize(feilder["throw_score"]));

                                    float slidemod = gen_rand();

                                    float slidescore = float.Parse(JsonSerializer.Serialize(thebatter["run_score"]));

                                    if (throwmod * throw_score >= slidemod * slidescore * 2)
                                    {
                                        // out at the base // return false
                                        game_state.outs++;
                                        game_state.strikes = 0;
                                        write_to_play(JsonSerializer.Serialize(thebatter["alias"]) + " out at " + (game_state.onbase == "first" ? "second." : "first."));
                                        return false;
                                    }
                                    else
                                    {
                                        // advanced.
                                        advance();
                                        write_to_play("Safe! " + JsonSerializer.Serialize(thebatter["alias"]) + " now at " + game_state.onbase + ".");
                                        return true;
                                    }


                                }



                            }
                            // failsafe
                            return false;

                        }

                    }

                    
                    void advance()
                    {
                        if (game_state.onbase == "first")
                        {
                            game_state.onbase = "second";
                        }
                        else if (game_state.onbase == "second")
                        {
                            game_state.onbase = "first";
                        }
                        game_state.strikes = 0;
                        game_state.balls = 0;
                        if (inteam == game_state.HomeTeam)
                        {
                            game_state.HomeScore++;
                        }
                        if (inteam == game_state.AwayTeam)
                        {
                            game_state.AwayScore++;
                        }
                    }

                }

            }

            float gen_rand()
            {
                return (float)new Random().Next(1, 4);
            }

            void write_to_play(string message)
            {
                game_state.play = message;
                File.WriteAllText("../memory/gamestate.json", JsonSerializer.Serialize(game_state));
                Thread.Sleep(5 * 1000); // wait 5 seconds
            }

        }

    }


}






// functions
JsonNode getToken(string token)
{

    clients = JsonObject.Parse(File.ReadAllText("../memory/clients.json"));

    return clients[token];
}

string generate_random_token()
{

    string tokenCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    string returnme = "";

    for (int i = 0; i < 20; i++)
    {
        returnme += tokenCharacters[new Random().Next(tokenCharacters.Length)];
    }

    return returnme;
}







// Data Structures
public class tokenString
{
    public string token { get; set; }
}

public class contractEntry
{
    public string alias { get; set; }
    public long pop_corns { get; set; }
    public bool QFS { get; set; }
    public bool DES { get; set; }
    public bool TLD { get; set; }
    public bool CIA { get; set; }
    public bool EPS { get; set; }
    public bool IFS { get; set; }
}

public class spectatorJSON
{
    public string alias { get; set; }
    public int renown { get; set; }
    public string favoriteteam { get; set; }

    public inventoryJSON inventory { get; set; }

    public long pop_corns { get; set; }

    public bool QFS { get; set; }
    public bool DES { get; set; }
    public bool TLD { get; set; }
    public bool CIA { get; set; }
    public bool EPS { get; set; }
    public bool IFS { get; set; }

    public spectatorJSON() { }
}

public class inventoryJSON
{
    public string slot1 { get; set; }
    public string slot2 { get; set; }
    public string slot3 { get; set; }
    public string slot4 { get; set; }
    public string slot5 { get; set; }

    public inventoryJSON()
    {
        slot1 = "";
        slot2 = "";
        slot3 = "";
        slot4 = "";
        slot5 = "";
    }

}

public class itemJSON
{
    public string name { get; set; }
    public string img { get; set; }
    public string desc { get; set; }

    public itemJSON()
    {
        name = "";
        img = "";
        desc = "";
    }

}

public class gameState
{
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public currentinning inning { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public int outs { get; set; }
    public int strikes { get; set; }
    public int balls { get; set; }
    public string batter { get; set; }
    public string onbase { get; set; }
    public string firstbasepitcher { get; set; }
    public string secondbasepitcher { get; set; }
    public string play { get; set; }
}

public class currentinning
{
    public string orientation { get; set; }
    public int count { get; set; }
}