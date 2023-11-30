export default {
    submit_contract,
    testapithing,
    get_player_data,
    get_game_state,
    get_teams_data,
    get_items,
    swap_items,
    buy_item,
    discard_item
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

async function get_items(){
    return await fetch(
        "http://localhost:" + API_PORT + "/items",
        {
            method: "GET",
        }
    )
}

async function swap_items(from, to){
    return await fetch(
        "http://localhost:" + API_PORT + "/swap",
        {
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify({
                "token": localStorage.getItem("token"),
                "fromslot": from,
                "toslot": to
            })
        }
    )
}

async function buy_item(itemname) {
    /*
    itemname must be one of the following:
    "pop-corn-bucket"
    "stadium-horn"
    "soda-can"
    */
    return await fetch(
        "http://localhost:" + API_PORT + "/buy",
        {
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify({
                "token": localStorage.getItem("token"),
                "itemname": itemname
            })
        }
    )
}

async function discard_item(slot) {
    return await fetch(
        "http://localhost:" + API_PORT + "/discard",
        { 
            method: "POST",
            headers: { "Content-type": "application/json; charset=UTF-8" },
            body: JSON.stringify({
                "token": localStorage.getItem("token"),
                "slot": slot
            })
        }
    )
}