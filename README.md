# Intro to Web Development Final project

<sub>By Aidan</sub>

**The Pitch** <sub>lol, pun</sub>  
Drawing heavy inspiration from the internet browser game <a href="https://en.wikipedia.org/wiki/Blaseball" rel="noopener noreferrer" target="_blank" >Blaseball</a> which recently <a href="https://www.blaseball.com/" rel="noopener noreferrer" target="_blank">died</a> (may it rest in violence), I want to make something that provides entertainment in the form of sitting back and watching AI/randomness bots battle it out in a game ~of *baseball* (?)~. Of course, as a final project, it's not enough to just ***watch*** the chaos unfold, but you need to be able ***add*** to it. So, there needs to be a game element to it in the form of some type of currency, some way to get it, and some way to spend it that affects your experience or the game itself.

**Pages**  
The landing page of this website will give a brief explanation of the game (and make a player/spectator give a name if they don't have a token). There will be a watch page where a game between two teams will unfold. There will be a league page which will show all of the teams (probably just two for this project) with their players and ratings + wins/losses. And last but not least a Store Page that will include a player's inventory (I will have to probably make a token system?) and items for sale.

Across the top I want a navigation bar that is slim and out of the way / not distracting.

**API/Database**
One of the main draws of blaseball was community. The ability to watch together. That makes the end goal of this project to try and make it somehow multiplayer (but in a very basic sense like having two instances of the website open that can talk to the same api, not fully implemented and hosted because yeesh security/stability problems hello).

To do this I will need to keep track of a player's entered-name, balance, favorite team, and items in the backend \- and use a token system which stores a token in the local browser storage to call the API (I know it's not secure).

While I'm at it, I might as well put the teams into the API.

And further, if I want it to work in multiple instances, I need the game in the API too. . . herm. . .

**Game Mechanics, Spectating and Internal**  
This is the hard part. If I take on a game that is too complex, I will end trying to do far too much in too little time. If I make it too simple, it won't be interesting or entertaining (I think?).

> [!NOTE]
> **Spectating**  
> 1. The currency is "Renown", earned by watching games and when your favorite team wins.
> 
> 2. Items you can buy will increase the amount of Renown you earn.
> 
> 3. In some way at some point, Renown will be used to affect the internal game at a very costly price. (maybe boosting a player's stat? costing someone their inventory as well?)
> 
>
. 
>[!NOTE]
> **The Game/Internal**  
> After some contemplation, I have decided to make a cricket-baseball hybrid. Why? Baseball is too complicated (think of how runners and bases work + stealing and where plays happen etc), and I couldn't think of a way to add visual elements of cricket to a digital looking panel, so I thought I'd combine the two.
> 
> 1. Each team has 11 players. One team bats, one team fields.
> 
> 2. There are two bases. Each base has a pitcher/baseman. 
> 
> 3. A batter starts on a base, the pitcher on the opposite base then bowls/pitches the ball to the batter.
> 
> 4. A pitcher can either make a throw or miss a throw due to a strikezone, and a batter can hit or not hit a ball. Three strikes and they're out. Four balls/Wild Throws/Misses and the batter gets to walk.
> 
> 5. When a batter hits a ball, they run to the opposing base. The ball can be caught, causing an out, or thrown to the base, causing an out. If the batter makes it, they score a point and get to bat again. There are no foul balls, there are no "home runs".
> 
> 6. Three outs changes the inning. There are 9 innings (18 rounds but teams swapped each round).
> 
> 7. Every six balls that are pitched/bowled by a team, a rotation happens. The pitcher on first goes to the outfield, the pitcher on second goes to the first base, and a player from the outfield goes to the second base.
> 
> 
> Thoughts and Reasons:  
> Two bases \- I need something visual to display. The bases are a nice touch, I can just light up whichever base the batter is on and not have to worry about stealing or multiple players on multiple bases. The rest is easy to display, strikes, outs, the names of the batters and pitchers.  
> Pitcher rotation \- something from cricket, but modified so I can use a circle-queue and a counter. There might be some weird interactions like pitchers switching in between strikes but I think that makes it interesting.
> 
> More details:  
> Each player will have a Throw score, a Bat score, and a Run score. These values act as multipliers to a random number generator for comparisons to see what happens. I have decided against a catch score, this is enough.  
> At bat: Rand\*Pitcher.Throw vs Rand\*Batter.Bat (on a good throw)  
> To Catch: Rand\*Batter.Bat(the prev) vs Rand\*Fielder.Run  
> For Run: Rand\*Fielder.Throw vs Rand\*Batter.Run  
> 

**Other notes**  
I want a dark theme, Here's my general guidelines: 80% black, 15% white, and 5% neon colors of all kinds.  
I haven't picked a name just quite yet, but the current competitors are Cricket + Baseball = Crackball or Baseball + Cricket = Brisket

> [!IMPORTANT]
> Make sure the app builds with the correct version of dotnet you have installed. If all else fails, remake the web app and paste in the program file from github, but before that try changing the dotnet target in the .csproj file in /api/core from net8.0 to net7.0 or vice versa. Good luck.  

> [!IMPORTANT]
> The html root needs to be the client folder. if you run an extension in vscode like Live Server, if the workspace folder that is open is not client when you do it, stuff is going to probably break.

---

## Phase 1 \- Nov 17

>[!NOTE]
>> - [x] ground work  
>> - [x] parts currently functioning
>> - [ ] total completion of parts to my satisfaction  
>
>   
> **Parts:**  
> all pages .html  
> - [x] Multiple Pages Fulfilled.  
>
> general pass at css (layout), each page has own specific css + main css  
> - [x] HTML Mastery, CSS Mastery Fulfilled.  
>
> nav bar installed  
> - [x] Shared Page Layout Fulfilled.  
> 
> - [x] api code started (c# is probably the easiest?)  



---

## Phase 2 \- Nov 24

>[!NOTE]
>>
>> - [x] ground work  
>> - [x] parts currently functioning  
>> - [ ] total completion of parts to my satisfaction  
> 
>   
> **Parts:**  
> game simulated in API, teams created.  
> watch page is populating with game data  
> - [x] Network call(s) to external API Fulfilled  
> 
> 
---

## Phase 3 \- Dec 1

>[!NOTE]
>>
>> - [x] ground work  
>> - [ ] parts currently functioning  
>> - [ ] total completion of parts to my satisfaction  
> 
>   
> **Parts:**  
> api data structures working + token system in effect  
> 
> - [x] Forms Fulfilled (spectator gives name)
> 
> store, inventory, items added (drag and drop items in inventory spaces)  
> 
> - [ ] Hard Things Fulfilled.  
> 
> - [ ] Interactivity Fulfilled (except query strings).  
> - [ ] Drag and Drop Fulfilled.  
> 
> filter search in league page for players  
> - [x] Filter Bar Fulfilled.  
> 
>
---

## Phase 4 \- Dec 8

> [!NOTE]
>>
>> - [ ] ground work  
>> - [ ] parts currently functioning  
>> - [ ] total completion of parts to my satisfaction  
> 
>   
> **Parts:**  
> pages finalized in look/feel  
> Store finished, renown can affect the game (form?)
> 
>

---

