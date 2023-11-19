export default {
    submit_contract,
    testapithing,
    get_player_data,
    get_game_state,
    get_teams_data
}

// IMPORTANT: CHANGE THIS IS YOU REDO THE WEB APP!
const API_PORT = "5184"

async function testapithing() {
    return await fetch(
        "http://localhost:" + API_PORT + "/random",
        {
            method: "GET"
        }
    )
}

async function submit_contract(requestObject) {

    return await fetch(
        "http://localhost:" + API_PORT + "/newcontract",
        {
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify(requestObject)
        }
    )
}

async function get_player_data(sendtoken) {
    return await fetch(
        "http://localhost:" + API_PORT + "/token",
        {
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify({
                "token": sendtoken
            })
        }
    )
}

async function get_game_state(){
    return await fetch(
        "http://localhost:" + API_PORT + "/game",
        {
            method: "GET"
        }
    )
}

async function get_teams_data(){
    return await fetch(
        "http://localhost:" + API_PORT + "/teams",
        {
            method: "GET"
        }
    )
}