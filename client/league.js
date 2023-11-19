import api_calls from "./svc/api_calls.js"

// redirect to contract if no token
if(localStorage.length < 1){
    window.location.replace("./contract.html")
}
else {
    const playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json())

    if (playerdata == null){
        window.location.replace("./contract.html")
    }

}


const teams = await (await api_calls.get_teams_data()).json()

// console.log(teams);


const all_teams = document.getElementById("teams")

all_teams.innerHTML = ''

// console.log(Object.entries(teams))

for (let team of Object.entries(teams)){
    const table = document.createElement("table")
    table.innerHTML = 
    `<tbody>
        <tr class="table-head">
            <th colspan="4">${team[0]}</th>
        </tr>
        <tr class="team-stats">
            <td colspan="4">
                <div class="flexdiv">
                    <p>Wins: ${team[1].Wins}</p>
                    <p>Ties: ${team[1].Ties}</p>
                    <p>Losses: ${team[1].Losses}</p>
                </div>
            </td>
        </tr>
        ${make_players_html(team[1])}
    </tbody>`

    all_teams.appendChild(table);
}

function make_players_html(team){
    let returnstring = "";
    // console.log(team)
    let players = Object.entries(team)
    for (let i = 3; i < 11; i++){
        // console.log(players[i])
        returnstring += 
        `<tr>
            <td>${players[i][1].alias}</td>
            <td>${players[i][1].run_score}</td>
            <td>${players[i][1].bat_score}</td>
            <td>${players[i][1].throw_score}</td>
        </tr>`
    }
    return returnstring;
}





/* <tr>
    <td>Player 1</td>
    <td>Run:</td>
    <td>Bat:</td>
    <td>Throw:</td>
</tr> */
   