using System.Text.Json;
using System.Text.Json.Nodes;


// web app stuff
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();

// no firewall yeeet
app.UseCors(policy =>
    policy
        .AllowAnyOrigin()
            .AllowAnyMethod()
                .AllowAnyHeader()
);



//if the directory doesn't exist, make it
if (!Directory.Exists("../memory"))
{
    Directory.CreateDirectory("../memory");
}

// delete stuff if it exists, if it doesn't it'll throw an exception so I'll catch that and just move on.
try
{
    File.Delete("../memory/clients.json");
    File.Delete("../memory/gamestate.json");
    File.Delete("../memory/items.json");
    File.Delete("../memory/teams.json");
}
catch (Exception e)
{
    Console.WriteLine("expecting a file not found error:" + e.Message);
}

// create files that don't exist yet
if (!File.Exists("../memory/gamestate.json"))
{
    File.WriteAllText("../memory/gamestate.json", "{\"HomeTeam\": \"\", \"AwayTeam\": \"\" ,\"inning\": {\"orientation\": \"top\",\"count\": 0},\"HomeScore\": 0,\"AwayScore\": 0,\"outs\": 0,\"strikes\": 0,\"balls\": 0,\"batter\": \"\",\"onbase\": \"first\",\"firstbasepitcher\": \"\",\"secondbasepitcher\": \"\",\"play\": \"Looks Like the Field is empty\",\"throws\": 0}");
}

if (!File.Exists("../memory/clients.json"))
{
    File.WriteAllText("../memory/clients.json", "{\"exampletoken\": { \"alias\": \"exampleman\", \"renown\": 0, \"favoriteteam\": \"team-name\", \"inventory\": {  \"slot1\": \"\",  \"slot2\": \"\",  \"slot3\": \"\",  \"slot4\": \"\",  \"slot5\": \"\" }, \"pop_corns\": 1, \"QFS\": false, \"DES\": false, \"TLD\": false, \"CIA\": false, \"EPS\": false, \"IFS\": false}}");
}

if (!File.Exists("../memory/items.json"))
{
    File.WriteAllText("../memory/items.json", "{\"pop-corn-bucket\": {\"name\": \"Pop-Corn Bucket\",\"img\": \"./img/popcorn.png\",\"desc\": \"Ah, Pop-corns. Can't get enough of em. You get 25 Renown every time your favorite team wins a game.\",\"price\": 100},\"stadium-horn\":{\"name\":\"Staduim Horn\",\"img\": \"./img/stadium-horn.png\",\"desc\":\"Make some noise! Start a chant! We want a pitcher! Not a. . . well, you get the idea. Doubles the amount of renown you recieve.\",\"price\":500},\"soda-can\":{\"name\":\"Soda Can\",\"img\":\"./img/soda-can.png\",\"desc\":\"Sluurrrrp. How Refreshing! You get 12 Renown every time your favorite team Loses.\",\"price\": 50}}");
}

if (!File.Exists("../memory/teams.json"))
{
    File.WriteAllText("../memory/teams.json", "{\"Team 1\": {\"Wins\": 0,\"Losses\": 0,\"Ties\": 0,\"Player1\": {\"alias\": \"Fred Burgermiester 1\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player2\": {\"alias\": \"Fred Burgermiester 2\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player3\": {\"alias\": \"Fred Burgermiester 3\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player4\": {\"alias\": \"Fred Burgermiester 4\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player5\": {\"alias\": \"Fred Burgermiester 5\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player6\": {\"alias\": \"Fred Burgermiester 6\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player7\": {\"alias\": \"Fred Burgermiester 7\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player8\": {\"alias\": \"Fred Burgermiester 8\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player9\": {\"alias\": \"Fred Burgermiester 9\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player10\": {\"alias\": \"Fred Burgermiester 10\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player11\": {\"alias\": \"Fred Burgermiester 11\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1}},\"Team 2\": {\"Wins\": 0,\"Losses\": 0,\"Ties\": 0,\"Player1\": {\"alias\": \"John Burgermiester 1\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player2\": {\"alias\": \"John Burgermiester 2\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player3\": {\"alias\": \"John Burgermiester 3\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player4\": {\"alias\": \"John Burgermiester 4\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player5\": {\"alias\": \"John Burgermiester 5\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player6\": {\"alias\": \"John Burgermiester 6\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player7\": {\"alias\": \"John Burgermiester 7\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player8\": {\"alias\": \"John Burgermiester 8\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player9\": {\"alias\": \"John Burgermiester 9\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player10\": {\"alias\": \"John Burgermiester 10\",\"run_score\": 1,\"throw_score\": 1,\"bat_score\": 1},\"Player11\": { \"alias\": \"John Burgermiester 11\", \"run_score\": 1, \"throw_score\": 1, \"bat_score\": 1}}}");
}

// get stuff from database storage jsons
var items = JsonSerializer.Deserialize<Dictionary<string, itemJSON>>(File.ReadAllText("../memory/items.json"));

var clients = JsonSerializer.Deserialize<Dictionary<string, spectatorJSON>>(File.ReadAllText("../memory/clients.json"));

var observers = new List<string>();

var teams = JsonSerializer.Deserialize<Dictionary<string, Team>>(File.ReadAllText("../memory/teams.json"));

var game_state = JsonSerializer.Deserialize<gameState>(File.ReadAllText("../memory/gamestate.json"));

// sign up
app.MapPost("/newcontract", (contractEntry recieved) =>
{
    Console.WriteLine("New Contract!");
    //we should get the right thing in the right format, so here goes nothing?

    // update program's copy
    // clients = JsonObject.Parse(File.ReadAllText("../memory/clients.json"));


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

    clients[newToken] = newclient;

    //write!
    File.WriteAllText("../memory/clients.json", JsonSerializer.Serialize(clients));

    return new { token = newToken };
});

// returns player data
app.MapPost("/token", (tokenString recievedtoken) =>
{
    if (clients.ContainsKey(recievedtoken.token)){
        return JsonSerializer.Serialize(clients[recievedtoken.token]);
    }
    else return JsonSerializer.Serialize("Failed, no such token exists!");
});

// for the league page
app.MapGet("/teams", () =>
{
    return JsonSerializer.Serialize(teams);
});

// reports game data and also handles watching renown gain
app.MapPost("/game", (tokenString observer) =>
{
    // this is me keeping track of who's watching, this should get cleared by another function every so often (and award renown)
    if (!observers.Contains(observer.token))
    {
        observers.Add(observer.token);
    }
    return JsonSerializer.Serialize(game_state);
});

// returns the items
app.MapGet("/items", () =>
{
    return JsonSerializer.Serialize(items);
});

// inventory manipulation
app.MapPost("/swap", (SwapSlots swapping) =>
{
    if (swapping.fromslot == 1) {
        if (swapping.toslot == 1) {
            var temp = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = temp;
        }
        else if (swapping.toslot == 2){
            var temp = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = temp;
        }
        else if (swapping.toslot == 3){
            var temp = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = temp;
        }
        else if (swapping.toslot == 4){
            var temp = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = temp;
        }
        else if (swapping.toslot == 5){
            var temp = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = temp;
        }

    }
    else if (swapping.fromslot == 2) {
        if (swapping.toslot == 1) {
            var temp = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot1 = temp;
        }
        else if (swapping.toslot == 2){
            var temp = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = temp;
        }
        else if (swapping.toslot == 3){
            var temp = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = temp;
        }
        else if (swapping.toslot == 4){
            var temp = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = temp;
        }
        else if (swapping.toslot == 5){
            var temp = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = temp;
        }
    }
    else if (swapping.fromslot == 3){
        if (swapping.toslot == 1) {
            var temp = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = temp;
        }
        else if (swapping.toslot == 2){
            var temp = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = temp;
        }
        else if (swapping.toslot == 3){
            var temp = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = temp;
        }
        else if (swapping.toslot == 4){
            var temp = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = temp;
        }
        else if (swapping.toslot == 5){
            var temp = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = temp;
        }
    }
    else if (swapping.fromslot == 4){
        if (swapping.toslot == 1) {
            var temp = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = temp;
        }
        else if (swapping.toslot == 2){
            var temp = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = temp;
        }
        else if (swapping.toslot == 3){
            var temp = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = temp;
        }
        else if (swapping.toslot == 4){
            var temp = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = temp;
        }
        else if (swapping.toslot == 5){
            var temp = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = temp;
        }
    }
    else if (swapping.fromslot == 5){
        if (swapping.toslot == 1) {
            var temp = clients[swapping.token].inventory.slot1;

            clients[swapping.token].inventory.slot1 = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = temp;
        }
        else if (swapping.toslot == 2){
            var temp = clients[swapping.token].inventory.slot2;

            clients[swapping.token].inventory.slot2 = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = temp;
        }
        else if (swapping.toslot == 3){
            var temp = clients[swapping.token].inventory.slot3;

            clients[swapping.token].inventory.slot3 = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = temp;
        }
        else if (swapping.toslot == 4){
            var temp = clients[swapping.token].inventory.slot4;

            clients[swapping.token].inventory.slot4 = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = temp;
        }
        else if (swapping.toslot == 5){
            var temp = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = clients[swapping.token].inventory.slot5;

            clients[swapping.token].inventory.slot5 = temp;
        }
    }

    File.WriteAllText("../memory/clients.json", JsonSerializer.Serialize(clients));
    return true;
});

// buying items
app.MapPost("/buy", (BuyingJson buying) =>
{

    // make sure the item name is valid
    // Console.WriteLine(items[buying.itemname]);
    if (items[buying.itemname] == null)
    {
        return JsonSerializer.Serialize("invalid item-name!");
    }

    // if you have enough renown to buy this thing
    // Console.WriteLine(JsonSerializer.Deserialize<int>(clients[buying.token]["renown"]));
    // Console.WriteLine(JsonSerializer.Deserialize<int>(items[buying.itemname]["price"]));

    if (clients[buying.token].renown >= items[buying.itemname].price)
    {
        //find an empty spot
        if (clients[buying.token].inventory.slot1 == "")
        {
            clients[buying.token].inventory.slot1 = buying.itemname;
            clients[buying.token].renown = clients[buying.token].renown - items[buying.itemname].price;
        }
        else if (clients[buying.token].inventory.slot2 == "")
        {
            clients[buying.token].inventory.slot2 = buying.itemname;
            clients[buying.token].renown = clients[buying.token].renown - items[buying.itemname].price;

        }
        else if (clients[buying.token].inventory.slot3 == "")
        {
            clients[buying.token].inventory.slot3 = buying.itemname;
            clients[buying.token].renown = clients[buying.token].renown - items[buying.itemname].price;

        }
        else if (clients[buying.token].inventory.slot4 == "")
        {
            clients[buying.token].inventory.slot4 = buying.itemname;
            clients[buying.token].renown = clients[buying.token].renown - items[buying.itemname].price;

        }
        else if (clients[buying.token].inventory.slot5 == "")
        {
            clients[buying.token].inventory.slot5 = buying.itemname;
            clients[buying.token].renown = clients[buying.token].renown - items[buying.itemname].price;

        }
        else
        {
            return JsonSerializer.Serialize("All slots are full!");
        }


    }
    else
    {
        return JsonSerializer.Serialize("Not enough Renown!");
    }

    File.WriteAllText("../memory/clients.json", JsonSerializer.Serialize(clients));
    return JsonSerializer.Serialize($"success! enjoy the {items[buying.itemname].name}");
});

// trashing items
app.MapPost("/discard", (DiscardJson discarding) =>
{

    if (discarding.slot == 1)
    {
        clients[discarding.token].inventory.slot1 = "";
    }
    else if (discarding.slot == 2)
    {
        clients[discarding.token].inventory.slot2 = "";
    }
    else if (discarding.slot == 3)
    {
        clients[discarding.token].inventory.slot3 = "";
    }
    else if (discarding.slot == 4)
    {
        clients[discarding.token].inventory.slot4 = "";
    }
    else if (discarding.slot == 5)
    {
        clients[discarding.token].inventory.slot5 = "";
    }

    File.WriteAllText("../memory/clients.json", JsonSerializer.Serialize(clients));

    return;
});



// executes in the background
Task.Run(() => RunGame());

Task.Run(() =>
{
    while (true)
    {
        RewardObservers();
        Thread.Sleep(1 * 1000);
    }
});


app.Run();


// I think I intend to make a list with everyone's tokens who has favorite teams and do it that way, not implemented yet.
Task RewardTeamPicks(string winningteam, string losingteam)
{
    int numitems = 0;

    foreach (string key in clients.Keys)
    {
        // for every token, look at their favorite team.
        // if their favorite team won, look for popcorn in their slots + horns
        if (clients[key].favoriteteam == winningteam)
        {
            int numbuckets = 0;

            if (clients[key].inventory.slot1 == "pop-corn-bucket")
            {
                numbuckets += 25;
            }

            if (clients[key].inventory.slot2 == "pop-corn-bucket")
            {
                numbuckets += 25;
            }


            if (clients[key].inventory.slot3 == "pop-corn-bucket")
            {
                numbuckets += 25;
            }


            if (clients[key].inventory.slot4 == "pop-corn-bucket")
            {
                numbuckets += 25;
            }

            if (clients[key].inventory.slot5 == "pop-corn-bucket")
            {
                numbuckets += 25;
            }
            numitems = numbuckets;
        }
        // if their favorite team lost, look for soda in their slots + horns
        else if (clients[key].favoriteteam == losingteam)
        {
            int numsodas = 0;

            if (clients[key].inventory.slot1 == "soda-can")
            {
                numsodas += 12;
            }

            if (clients[key].inventory.slot2 == "soda-can")
            {
                numsodas += 12;
            }

            if (clients[key].inventory.slot3 == "soda-can")
            {
                numsodas += 12;
            }

            if (clients[key].inventory.slot4 == "soda-can")
            {
                numsodas += 12;
            }

            if (clients[key].inventory.slot5 == "soda-can")
            {
                numsodas += 12;
            }
            numitems = numsodas;
        }
        else
        {
            continue;
        }

        int numhorns = 0;

        if (clients[key].inventory.slot1 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[key].inventory.slot2 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[key].inventory.slot3 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[key].inventory.slot4 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[key].inventory.slot5 == "stadium-horn")
        {
            numhorns++;
        }

        // Console.WriteLine(Math.Pow(2,  numhorns));

        clients[key].renown = clients[key].renown + (int)(numitems * Math.Pow(2, numhorns));
    }

    File.WriteAllText("../memory/clients.json", JsonSerializer.Serialize(clients));

    return Task.CompletedTask;
}


// game loop needs to run async.
Task RunGame()
{

    // repeat this indefinately.
    while (true)
    {
        game("Team 1", "Team 2");

        game("Team 2", "Team 1");
    }

    return Task.CompletedTask;

    void game(string team1name, string team2name)
    {

        // reset the game state
        Console.WriteLine("Reseting the gamestate");
        File.Delete("../memory/gamestate.json");
        File.WriteAllText("../memory/gamestate.json", "{\"HomeTeam\": \"\", \"AwayTeam\": \"\" ,\"inning\": {\"orientation\": \"top\",\"count\": 0},\"HomeScore\": 0,\"AwayScore\": 0,\"outs\": 0,\"strikes\": 0,\"balls\": 0,\"batter\": \"\",\"onbase\": \"first\",\"firstbasepitcher\": \"\",\"secondbasepitcher\": \"\",\"play\": \"Looks Like the Field is empty\",\"throws\": 0}");
        game_state = JsonSerializer.Deserialize<gameState>(File.ReadAllText("../memory/gamestate.json"));

        Console.WriteLine("Game has started");

        game_state.HomeTeam = team1name;
        game_state.AwayTeam = team2name;

        var Team_1 = teams[team1name];
        var Team_2 = teams[team2name];

        LinkedList<Player> Team_1_Batters = new LinkedList<Player>();
        LinkedList<Player> Team_2_Batters = new LinkedList<Player>();
        LinkedList<Player> Team_1_Fielders = new LinkedList<Player>();
        LinkedList<Player> Team_2_Fielders = new LinkedList<Player>();

        
        Team_1_Batters.AddLast(Team_1.Player1);
        Team_1_Batters.AddLast(Team_1.Player2);
        Team_1_Batters.AddLast(Team_1.Player3);
        Team_1_Batters.AddLast(Team_1.Player4);
        Team_1_Batters.AddLast(Team_1.Player5);
        Team_1_Batters.AddLast(Team_1.Player6);
        Team_1_Batters.AddLast(Team_1.Player7);
        Team_1_Batters.AddLast(Team_1.Player8);
        Team_1_Batters.AddLast(Team_1.Player9);
        Team_1_Batters.AddLast(Team_1.Player10);
        Team_1_Batters.AddLast(Team_1.Player11);

        Team_1_Fielders.AddLast(Team_1.Player1);
        Team_1_Fielders.AddLast(Team_1.Player2);
        Team_1_Fielders.AddLast(Team_1.Player3);
        Team_1_Fielders.AddLast(Team_1.Player4);
        Team_1_Fielders.AddLast(Team_1.Player5);
        Team_1_Fielders.AddLast(Team_1.Player6);
        Team_1_Fielders.AddLast(Team_1.Player7);
        Team_1_Fielders.AddLast(Team_1.Player8);
        Team_1_Fielders.AddLast(Team_1.Player9);
        Team_1_Fielders.AddLast(Team_1.Player10);
        Team_1_Fielders.AddLast(Team_1.Player11);

        Team_2_Batters.AddLast(Team_2.Player1);
        Team_2_Batters.AddLast(Team_2.Player2);
        Team_2_Batters.AddLast(Team_2.Player3);
        Team_2_Batters.AddLast(Team_2.Player4);
        Team_2_Batters.AddLast(Team_2.Player5);
        Team_2_Batters.AddLast(Team_2.Player6);
        Team_2_Batters.AddLast(Team_2.Player7);
        Team_2_Batters.AddLast(Team_2.Player8);
        Team_2_Batters.AddLast(Team_2.Player9);
        Team_2_Batters.AddLast(Team_2.Player10);
        Team_2_Batters.AddLast(Team_2.Player11);

        Team_2_Fielders.AddLast(Team_2.Player1);
        Team_2_Fielders.AddLast(Team_2.Player2);
        Team_2_Fielders.AddLast(Team_2.Player3);
        Team_2_Fielders.AddLast(Team_2.Player4);
        Team_2_Fielders.AddLast(Team_2.Player5);
        Team_2_Fielders.AddLast(Team_2.Player6);
        Team_2_Fielders.AddLast(Team_2.Player7);
        Team_2_Fielders.AddLast(Team_2.Player8);
        Team_2_Fielders.AddLast(Team_2.Player9);
        Team_2_Fielders.AddLast(Team_2.Player10);
        Team_2_Fielders.AddLast(Team_2.Player11);
        

        // we do the last on batters so it goes over to the first on startup
        LinkedListNode<Player> Team_1_Batter_Up = Team_1_Batters.Last;
        LinkedListNode<Player> Team_1_Pitcher_Up = Team_1_Fielders.First;
        LinkedListNode<Player> Team_2_Batter_Up = Team_2_Batters.Last;
        LinkedListNode<Player> Team_2_Pitcher_Up = Team_2_Fielders.First;


        // 9 innings. count starts at 0.
        while (game_state.inning.count < 9)
        {
            game_state.inning.count++;
            game_state.inning.orientation = "top";
            // Console.WriteLine(Team_1_Batter_Up.Value);
            halfinning(team1name, ref Team_1_Batter_Up, team2name, ref Team_2_Pitcher_Up);
            // Console.WriteLine(Team_1_Batter_Up.Value);
            game_state.inning.orientation = "bottom";
            halfinning(team2name, ref Team_2_Batter_Up, team1name, ref Team_1_Pitcher_Up);
        }


        if (game_state.HomeScore > game_state.AwayScore)
        {
            write_to_play("Home Team " + game_state.HomeTeam + " Wins!");
            teams[team1name].Wins = Team_1.Wins + 1;
            teams[team2name].Losses = Team_2.Losses + 1;
            RewardTeamPicks(team1name, team2name);
            Thread.Sleep(15 * 1000);
        }
        else if (game_state.HomeScore < game_state.AwayScore)
        {
            write_to_play("Away Team " + game_state.AwayTeam + " Wins!");
            teams[team1name].Losses = Team_1.Losses + 1;
            teams[team2name].Wins = Team_2.Wins + 1;
            RewardTeamPicks(team2name, team1name);
            Thread.Sleep(15 * 1000);
        }
        else if (game_state.HomeScore == game_state.AwayScore)
        {
            write_to_play("It's a tie??????");
            teams[team1name].Ties = Team_1.Ties + 1;
            teams[team2name].Ties = Team_2.Ties + 1;
            Thread.Sleep(15 * 1000);
        }
        else
        {
            // ????????
        }

        save_teams();


        return; // end of game -------------



        void halfinning(string inteam, ref LinkedListNode<Player> batter, string outteam, ref LinkedListNode<Player> pitcher)
        {
            // outs reset
            game_state.outs = 0;
            game_state.throws = 0;

            File.WriteAllText("../memory/gamestate.json", JsonSerializer.Serialize(game_state));

            //while outs < 3
            while (true)
            {
                if (game_state.outs >= 3)
                {
                    write_to_play("Out 3! Changing sides. . .");
                    break;
                }
                round(ref batter, ref pitcher);
            }

            return;


            void round(ref LinkedListNode<Player> batter, ref LinkedListNode<Player> pitcher)
            {
                // strikes, balls reset
                game_state.strikes = 0;
                game_state.balls = 0;

                game_state.firstbasepitcher = pitcher.Value.alias;
                game_state.secondbasepitcher = pitcher.Next != null ? pitcher.Next.Value.alias : pitcher.List.First.Value.alias;


                batter = batter.Next != null ? batter.Next : batter.List.First;
                game_state.batter = batter.Value.alias;
                game_state.onbase = "first";

                pitch(ref batter, ref pitcher);
                return;


                void pitch(ref LinkedListNode<Player> batter, ref LinkedListNode<Player> pitcher)
                {
                    write_to_play(batter.Value.alias + " lines up to bat.");

                    // until 3 strikes, 4 balls, or a hit (break)

                    while (true)
                    {
                        if (game_state.throws >= 6)
                        {
                            // rotate pitchers
                            pitcher = pitcher.Next != null ? pitcher.Next : pitcher.List.First;
                            write_to_play("The pitchers are rotating.");
                            game_state.firstbasepitcher = pitcher.Value.alias;
                            game_state.secondbasepitcher = pitcher.Next != null ? pitcher.Next.Value.alias : pitcher.List.First.Value.alias;
                            game_state.throws = 0;
                        }

                        if (game_state.strikes >= 3)
                        {
                            // out
                            game_state.outs++;
                            write_to_play(batter.Value.alias + " struck out.");
                            // rotate batters, break
                            return;
                        }

                        if (game_state.balls >= 4)
                        {
                            // walk
                            write_to_play("Ball 4. " + batter.Value.alias + " walks.");
                            advance();
                            continue;
                        }

                        // pitch
                        if (game_state.onbase == "first")
                        {
                            // pitcher on second throws (pitcher.next)
                            LinkedListNode<Player> nextpitcher = pitcher.Next != null ? pitcher.Next : pitcher.List.First;
                            game_state.throws++;

                            // if hurl ball returns false, break. otherwise continue.

                            if (!hurl_ball(nextpitcher, batter))
                            {
                                break;
                            }
                            continue;

                        }

                        if (game_state.onbase == "second")
                        {
                            // pitcher on first throws (pitcher)
                            game_state.throws++;

                            // if hurl ball returns false, break. otherwise continue.

                            if (!hurl_ball(pitcher, batter))
                            {
                                break;
                            }
                            continue;


                        }

                        bool hurl_ball(LinkedListNode<Player> thepitcher, LinkedListNode<Player> thebatter)
                        {

                            write_to_play(thepitcher.Value.alias + " hurls the ball down the pitch. . .");

                            float pitchmod = gen_rand();

                            float pitchscore = (float)thepitcher.Value.throw_score;

                            if (pitchmod * pitchscore < 1)
                            {
                                // wild
                                game_state.balls++;
                                write_to_play(thepitcher.Value.alias + " throws wild. Ball.");

                                return true;
                            }

                            float batmod = gen_rand();

                            float batscore = (float)thebatter.Value.bat_score;

                            float hit_total = batmod * batscore;

                            Console.WriteLine($"Pitch:{pitchmod * pitchscore}\nBat:{hit_total}");

                            if (hit_total <= pitchmod * pitchscore)
                            {
                                // strike
                                game_state.strikes++;
                                write_to_play(thepitcher.Value.alias + " Throws a strike!");

                                return true;
                            }

                            if (hit_total > pitchmod * pitchscore)
                            {
                                //hit

                                int target = new Random().Next(thepitcher.List.Count);
                                Player feilder = thepitcher.List.ElementAt<Player>(target);

                                write_to_play("Ball hit towards " + feilder.alias + "!");

                                float runmod = gen_rand();

                                float runscore = (float)feilder.run_score;

                                float run_total = runmod * runscore;

                                if (hit_total <= run_total)
                                {
                                    //caught. // return false
                                    game_state.outs++;
                                    game_state.strikes = 0;
                                    write_to_play(feilder.alias + " caught the ball. " + thebatter.Value.alias + " is OUT!");
                                    return false;
                                }

                                if (hit_total > run_total)
                                {
                                    // not caught
                                    write_to_play(feilder.alias + " picks the ball up!");
                                    write_to_play(feilder.alias + " throws the ball to " + (game_state.onbase == "first" ? "second!" : "first!"));
                                    float throwmod = gen_rand();

                                    float throw_score = (float)feilder.throw_score;

                                    float slidemod = gen_rand();

                                    float slidescore = (float)thebatter.Value.run_score;

                                    if (throwmod * throw_score >= slidemod * slidescore * 2)
                                    {
                                        // out at the base // return false
                                        game_state.outs++;
                                        game_state.strikes = 0;
                                        write_to_play(thebatter.Value.alias + " out at " + (game_state.onbase == "first" ? "second." : "first."));
                                        return false;
                                    }
                                    else
                                    {
                                        // advanced.
                                        advance();
                                        write_to_play("Safe! " + thebatter.Value.alias + " now at " + game_state.onbase + ".");
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
                // 0 to 3
                string whole = (new Random().Next(4)).ToString();
                // 0 to 99
                string fraction = (new Random().Next(100)).ToString();
                // concat
                string random = whole + "." + fraction;
                // Console.WriteLine("Generated " + random);
                return float.Parse(random);
            }

        }

        void write_to_play(string message)
        {
            game_state.play = message;
            Console.WriteLine(message);
            File.WriteAllText("../memory/gamestate.json", JsonSerializer.Serialize(game_state).ToString());
            Thread.Sleep(2 * 1000); // I like 5 and 3 second intervals, but for testing purposes, 1 or less might be good
        }


        void save_teams()
        {
            // string writestring = "{" + "\"" + game_state.HomeTeam + "\":" + JsonSerializer.Serialize(home_team) + ",\"" + game_state.AwayTeam + "\":" + JsonSerializer.Serialize(away_team) + "}";
            File.WriteAllText("../memory/teams.json", JsonSerializer.Serialize(teams));
        }


    }



}


// rewards people currently watching on the watch page
Task RewardObservers()
{
    if (observers.Count == 0){
        return Task.CompletedTask;
    }

    foreach (string watcher in observers)
    {
        // Console.WriteLine(watcher + "is watching!");
        int numhorns = 0;

        // Console.WriteLine(clients[watcher].inventory.["slot1"]));

        // I tried to do this elegantly with a foreach loop but couldn't find a good json serialization option to make it happen :/

        if (clients[watcher].inventory.slot1 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[watcher].inventory.slot2 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[watcher].inventory.slot3 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[watcher].inventory.slot4 == "stadium-horn")
        {
            numhorns++;
        }

        if (clients[watcher].inventory.slot5 == "stadium-horn")
        {
            numhorns++;
        }

        // Console.WriteLine(Math.Pow(2,  numhorns));

        clients[watcher].renown = clients[watcher].renown + (int)(1 * Math.Pow(2, numhorns));

    }

    File.WriteAllText("../memory/clients.json", JsonSerializer.Serialize(clients));

    observers.Clear();

    return Task.CompletedTask;
}



// functions

string generate_random_token()
{

    string tokenCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    string returnme = "";

    for (int i = 0; i < 20; i++)
    {
        returnme += tokenCharacters[new Random().Next(tokenCharacters.Length)];
    }

    // if this token already exists try again
    if (clients.ContainsKey(returnme))
    {
        returnme = generate_random_token();
    }

    return returnme;
}




// Data Structures

public class tokenString
{
    public string token { get; set; }
}

public class itemString
{
    public string item { get; set; }
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
    public int price { get; set; }

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
    public int throws { get; set; }
}

public class currentinning
{
    public string orientation { get; set; }
    public int count { get; set; }
}

public class Team
{
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public Player Player3 { get; set; }
    public Player Player4 { get; set; }
    public Player Player5 { get; set; }
    public Player Player6 { get; set; }
    public Player Player7 { get; set; }
    public Player Player8 { get; set; }
    public Player Player9 { get; set; }
    public Player Player10 { get; set; }
    public Player Player11 { get; set; }
}

public class Player
{
    public string alias { get; set; }
    public float run_score { get; set; }
    public float throw_score { get; set; }
    public float bat_score { get; set; }
}


public class SwapSlots
{

    public string token { get; set; }
    public int fromslot { get; set; }
    public int toslot { get; set; }
}


public class BuyingJson
{
    public string token { get; set; }
    public string itemname { get; set; }
}


public class DiscardJson
{
    public string token { get; set; }
    public int slot { get; set; }
}




