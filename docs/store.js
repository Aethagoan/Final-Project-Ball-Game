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

let items = await ( await api_calls.get_items()).json()
// console.log(items);


load_player_data()

async function load_player_data(){
    playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json());

    let icon
    let discard

    document.getElementById("current-renown").innerText = `Your Renown: ${playerdata.renown}`
    if (playerdata.inventory.slot1 != "") {

        icon = document.createElement("img")
        icon.classList.add("icon")
        icon.setAttribute("src", items[playerdata.inventory.slot1].img)
        icon.setAttribute("draggable", "false")

        discard = document.createElement("img")
        discard.classList.add("cancel")
        discard.setAttribute("src", "./img/cancel.png")
        discard.setAttribute("draggable", "false")

        discard.addEventListener("click", async () => {
            if (confirm(`Do you want to discard ${items[playerdata.inventory.slot1].name}? (THIS CAN'T BE UNDONE!)`)){
                await api_calls.discard_item(1)
                load_player_data()
            }
        })

        document.getElementById("slot1").innerHTML = ``

        document.getElementById("slot1").appendChild(icon)
        document.getElementById("slot1").appendChild(discard)

    } 
    else {
        document.getElementById("slot1").innerHTML = `<p>slot 1</p>`
    }


    if (playerdata.inventory.slot2 != "") {
        
        
        icon = document.createElement("img")
        icon.classList.add("icon")
        icon.setAttribute("src", items[playerdata.inventory.slot2].img)
        icon.setAttribute("draggable", "false")

        discard = document.createElement("img")
        discard.classList.add("cancel")
        discard.setAttribute("src", "./img/cancel.png")
        discard.setAttribute("draggable", "false")

        discard.addEventListener("click", async () => {
            if (confirm(`Do you want to discard ${items[playerdata.inventory.slot2].name}? (THIS CAN'T BE UNDONE!)`)){
                await api_calls.discard_item(2)
                load_player_data()
            }
        })

        document.getElementById("slot2").innerHTML = ``

        document.getElementById("slot2").appendChild(icon)
        document.getElementById("slot2").appendChild(discard)

    
    }
    else {
        document.getElementById("slot2").innerHTML = `<p>slot 2</p>`
    }


    if (playerdata.inventory.slot3 != "") {
        
        
        icon = document.createElement("img")
        icon.classList.add("icon")
        icon.setAttribute("src", items[playerdata.inventory.slot3].img)
        icon.setAttribute("draggable", "false")

        discard = document.createElement("img")
        discard.classList.add("cancel")
        discard.setAttribute("src", "./img/cancel.png")
        discard.setAttribute("draggable", "false")

        discard.addEventListener("click", async () => {
            if (confirm(`Do you want to discard ${items[playerdata.inventory.slot3].name}? (THIS CAN'T BE UNDONE!)`)){
                await api_calls.discard_item(3)
                load_player_data()
            }
        })

        document.getElementById("slot3").innerHTML = ``

        document.getElementById("slot3").appendChild(icon)
        document.getElementById("slot3").appendChild(discard)


    }
    else {
        document.getElementById("slot3").innerHTML = `<p>slot 3</p>`
    }

    
    if (playerdata.inventory.slot4 != "") {
        
        
        icon = document.createElement("img")
        icon.classList.add("icon")
        icon.setAttribute("src", items[playerdata.inventory.slot4].img)
        icon.setAttribute("draggable", "false")

        discard = document.createElement("img")
        discard.classList.add("cancel")
        discard.setAttribute("src", "./img/cancel.png")
        discard.setAttribute("draggable", "false")

        discard.addEventListener("click", async () => {
            if (confirm(`Do you want to discard ${items[playerdata.inventory.slot4].name}? (THIS CAN'T BE UNDONE!)`)){
                await api_calls.discard_item(4)
                load_player_data()
            }
        })

        document.getElementById("slot4").innerHTML = ``

        document.getElementById("slot4").appendChild(icon)
        document.getElementById("slot4").appendChild(discard)


    }
    else {
        document.getElementById("slot4").innerHTML = `<p>slot 4</p>`
    }


    if (playerdata.inventory.slot5 != "") {
        
        
        icon = document.createElement("img")
        icon.classList.add("icon")
        icon.setAttribute("src", items[playerdata.inventory.slot5].img)
        icon.setAttribute("draggable", "false")

        discard = document.createElement("img")
        discard.classList.add("cancel")
        discard.setAttribute("src", "./img/cancel.png")
        discard.setAttribute("draggable", "false")

        discard.addEventListener("click", async () => {
            if (confirm(`Do you want to discard ${items[playerdata.inventory.slot5].name}? (THIS CAN'T BE UNDONE!)`)){
                await api_calls.discard_item(5)
                load_player_data()
            }
        })

        document.getElementById("slot5").innerHTML = ``

        document.getElementById("slot5").appendChild(icon)
        document.getElementById("slot5").appendChild(discard)


    }
    else {
        document.getElementById("slot5").innerHTML = `<p>slot 5</p>`
    }


}

render_store();


async function render_store(){
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
        
        store.appendChild(createfigure(item[0], item[1]));

    }

    function createfigure(itemidentifier, item) {
        // console.log(item)

        const figure = document.createElement("figure")
        const dl = document.createElement("dl")
        const dt = document.createElement("dt")
        dt.innerHTML = `${item["name"]}`

        const div = document.createElement("div")
        div.classList.add("midsection")

        const img = document.createElement("img")
        img.setAttribute("src", `${item["img"]}`)
        img.setAttribute("draggable", "false")
        
        const innerdd = document.createElement("dd")
        innerdd.classList.add("purchase")
        innerdd.innerHTML = `buy for ${item["price"]} renown`

        innerdd.addEventListener("click", async () => {
            if (confirm(`Are you sure you want to buy ${item.name}?`)){
                alert(await (await api_calls.buy_item(itemidentifier)).json())
                load_player_data()
            }
        })

        const outerdd = document.createElement("dd")
        outerdd.innerHTML = `${item["desc"]}`

        div.appendChild(img)
        div.appendChild(innerdd)

        dl.appendChild(dt)
        dl.appendChild(div)
        dl.appendChild(outerdd)

        figure.appendChild(dl)


        return figure
    }


}


// when you drag an inventory item around set the transfer to be the name of the slot it came from then you can do all the rest

// I made the images nondraggable and the elements themselves draggable.
slot1.setAttribute("draggable", "true")
slot2.setAttribute("draggable", "true")
slot3.setAttribute("draggable", "true")
slot4.setAttribute("draggable", "true")
slot5.setAttribute("draggable", "true")

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

/* I need to send the request to /inventory, and included I need 
    token
    slot to switch from
    slot to switch to
*/


slot1.addEventListener("dragstart", (event) => {
    // console.log(items[playerdata.inventory.slot1].name)
    // slot 1
    event.dataTransfer.setData("text", 1)
})

slot2.addEventListener("dragstart", (event) => {
    // console.log(items[playerdata.inventory.slot1].name)
    event.dataTransfer.setData("text", 2)
})

slot3.addEventListener("dragstart", (event) => {
    // console.log(items[playerdata.inventory.slot1].name)
    event.dataTransfer.setData("text", 3)
})

slot4.addEventListener("dragstart", (event) => {
    // console.log(items[playerdata.inventory.slot1].name)
    event.dataTransfer.setData("text", 4)
})

slot5.addEventListener("dragstart", (event) => {
    // console.log(items[playerdata.inventory.slot1].name)
    event.dataTransfer.setData("text", 5)
})



slot1.addEventListener("drop", async (event) =>{
    // console.log(event.dataTransfer.getData("text"), 1)
    await api_calls.swap_items(event.dataTransfer.getData("text"), 1)
    load_player_data()
})

slot2.addEventListener("drop", async (event) =>{
    // console.log(event.dataTransfer.getData("text"), 2)
    await api_calls.swap_items(event.dataTransfer.getData("text"), 2)
    load_player_data()
    
})

slot3.addEventListener("drop", async (event) =>{
    // console.log(event.dataTransfer.getData("text"), 3)
    await api_calls.swap_items(event.dataTransfer.getData("text"), 3)
    load_player_data()

})

slot4.addEventListener("drop", async (event) =>{
    // console.log(event.dataTransfer.getData("text"), 4)
    await api_calls.swap_items(event.dataTransfer.getData("text"), 4)
    load_player_data()

})

slot5.addEventListener("drop", async (event) =>{
    // console.log(event.dataTransfer.getData("text"), 5)
    await api_calls.swap_items(event.dataTransfer.getData("text"), 5)
    load_player_data()

})
