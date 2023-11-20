import api_calls from "./svc/api_calls.js"

let playerdata

// redirect to contract if no token
if (localStorage.length < 1) {
    window.location.replace("./contract.html")
}
else {
    playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json())

    if (playerdata == null) {
        window.location.replace("./contract.html")
    }

}

const items = await ( await api_calls.get_items()).json()

console.log(items)
// console.log(playerdata)



document.getElementById("current-renown").innerText = `Your Renown: ${playerdata.renown}`
if (playerdata.inventory.slot1 != "") {
    document.getElementById("slot1").innerHTML = `<img src="${items[playerdata.inventory.slot1].img}">`
}

if (playerdata.inventory.slot2 != "") {
    document.getElementById("slot2").innerHTML = `<img src="${items[playerdata.inventory.slot2].img}">`
}

if (playerdata.inventory.slot3 != "") {
    document.getElementById("slot3").innerHTML = `<img src="${items[playerdata.inventory.slot3].img}">`
}

if (playerdata.inventory.slot4 != "") {
    document.getElementById("slot4").innerHTML = `<img src="${items[playerdata.inventory.slot4].img}">`
}

if (playerdata.inventory.slot5 != "") {
    document.getElementById("slot5").innerHTML = `<img src="${items[playerdata.inventory.slot5].img}">`
}



document.getElementById("slot1").addEventListener("dragover", (event) => {
    event.preventDefault();
})
document.getElementById("slot2").addEventListener("dragover", (event) => {
    event.preventDefault();
})
document.getElementById("slot3").addEventListener("dragover", (event) => {
    event.preventDefault();
})
document.getElementById("slot4").addEventListener("dragover", (event) => {
    event.preventDefault();
})
document.getElementById("slot5").addEventListener("dragover", (event) => {
    event.preventDefault();
})

render_store()

function render_store(){
    const store = document.getElementById("store")

    /*
    <figure>
        <dl>
            <dt>Pop-corn bucket</dt>
            <div id="midsection">
                <img src="./img/popcorn.png">
                <dd class="purchase">buy for x renown</dd>
            </div>
            <dd>
                Ah, Pop-corns. Can't get enough of em. You get one more Renown every time your
                favorite team wins a game.
            </dd>
        </dl>
    </figure>
    */

    store.innerHTML = ""

    for (let item of Object.entries(items)){
        
        store.innerHTML += createfigure(item[1]);
    }


    function createfigure(item) {
        console.log(item)
        return `<figure>
        <dl>
            <dt>${item["name"]}</dt>
            <div class="midsection">
                <img src="${item["img"]}">
                <dd class="purchase">buy for ${item["price"]} renown</dd>
            </div>
            <dd>
                ${item["desc"]}
            </dd>
        </dl>
    </figure>`
    }


}