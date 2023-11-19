import api_calls from "./svc/api_calls.js"

// redirect to contract if no token
if (localStorage.length < 1) {
    window.location.replace("./contract.html")
}
else {
    const playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json())

    if (playerdata == null) {
        window.location.replace("./contract.html")
    }

}


const home_team = document.getElementById("home-team")
const away_team = document.getElementById("away-team")
const inning = document.getElementById("inning")
const home_score = document.getElementById("home-score")
const away_score = document.getElementById("away-score")
const outs = document.getElementById("outs")
const strikes = document.getElementById("strikes")
const balls = document.getElementById("balls")
const base1 = document.getElementById("base-1")
const base2 = document.getElementById("base-2")
const first_pitcher = document.getElementById("base-1-pitcher")
const second_pitcher = document.getElementById("base-2-pitcher")
const batter = document.getElementById("batter")
const play = document.getElementById("play")

while(true) {
    await new Promise(r => {
        setTimeout(r, 500);
    });
    const game_state = await (await api_calls.get_game_state()).json()
    // console.log(game_state)

    home_team.innerText = "Home Team: " + game_state.HomeTeam;
    away_team.innerText = "Away Team: " + game_state.AwayTeam;
    inning.innerText = "inning: " + game_state.inning.orientation + " of " + game_state.inning.count
    home_score.innerText = "Home Score: " + game_state.HomeScore
    away_score.innerText = "Away Score: " + game_state.AwayScore
    outs.innerText = "outs: " + game_state.outs
    strikes.innerText = "strikes: " + game_state.strikes
    balls.innerText = "balls: " + game_state.balls

    base1.classList.remove("on-base")
    base2.classList.remove("on-base")

    if (game_state.onbase == "first"){
        base1.classList.add("on-base")
    }

    if (game_state.onbase == "second"){
        base2.classList.add("on-base")
    }

    first_pitcher.innerText = "first base pitcher: " + game_state.firstbasepitcher
    second_pitcher.innerText = "second base pitcher: " + game_state.secondbasepitcher

    batter.innerText = "batter: " + game_state.batter

    play.innerText = game_state.play.replaceAll(`"`, '')

}