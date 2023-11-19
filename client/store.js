import api_calls from "./svc/api_calls.js"

// redirect to contract if no token
if(localStorage.length < 1){
    window.location.replace("./contract.html")
}
else {
    const playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json())
}

if (playerdata == null){
    window.location.replace("./contract.html")
}