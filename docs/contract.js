import api_calls from "./svc/api_calls.js";


// wait for form to exist
while(!document.getElementById("contract-form")) {
    await new Promise(r => {
        setTimeout(r, 50);
    });
}

// when they submit the form, call this function
document.getElementById("contract-form").addEventListener("submit", submission)
document.getElementById("contract-form").addEventListener("submit", (event) => {
    event.preventDefault()//stop refresh page, I'll do that when I'm ready.
})

async function submission(event) {
    console.clear()
   
    console.log(event)
    console.log(event.target[0].name,)
    console.log(event.target[1].name,event.target[1].value)
    console.log(event.target[2].name,event.target[2].checked)
    console.log(event.target[3].name,event.target[3].checked)
    console.log(event.target[4].name,event.target[4].checked)
    console.log(event.target[5].name,event.target[5].checked)
    console.log(event.target[6].name,event.target[6].checked)
    console.log(event.target[7].name,event.target[7].checked)
    
    
    const alias = event.target[0].value


    const pop_corns = event.target[1].value ? event.target[1].value : 0; // defaults to 0 if they didn't enter anything

    const QFS = event.target[2].checked
    const DES = event.target[3].checked
    const TLD = event.target[4].checked
    const CIA = event.target[5].checked
    const EPS = event.target[6].checked
    const IFS = event.target[7].checked

    const requestObject = {
        "alias":alias,
        "pop_corns":pop_corns,
        "QFS": QFS,
        "DES": DES,
        "TLD": TLD,
        "CIA": CIA,
        "EPS": EPS,
        "IFS": IFS
    }

    /* JSON for spectator
    "token" : {
        "alias": alias,
        "renown": 0,
        "favorite-team": "team-name",
        
        "inventory": {
            "slot1": "item-name",
            "slot2": "item-name",
            "slot3": "item-name",
            "slot4": "item-name",
            "slot5": "item-name"
        },

        "pop_corns": popcorns,
        "QFS": QFS,
        "DES": DES,
        "TLD": TLD,
        "CIA": CIA,
        "EPS": EPS,
        "IFS": IFS
    }
    */

    /* item json 

        "pop-corn-bucket": {
            "name": "name",
            "img": "path-from-/client/",
            "desc": "description"
        }
    */

    /* what the API is expecting from a new contract
    
        {
            "alias": "the thing they typed",
            "pop_corns": popcorns,
            "QFS": QFS,
            "DES": DES,
            "TLD": TLD,
            "CIA": CIA,
            "EPS": EPS,
            "IFS": IFS
        }    
    */
    
    // console.log(await (await api_calls.testapithing()).json())

    // subit the contract, get the token back, put it into local storage.
    localStorage.setItem("token", (await (await api_calls.submit_contract(requestObject)).json()).token)

    // console.log(await (await api_calls.get_player_data(localStorage.getItem("token"))).json())

    window.location.replace("./contract.html")
}


// if there is local storage, replace the form element with a "copy" of the contract they signed, without the form elements.
if (!(localStorage.length < 1)){
    const playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json())
    
    if (playerdata != null){
        
        document.getElementById("contract-form").removeEventListener("submit", submission)
        document.getElementById("submit-button").classList.add("hide")
        document.getElementById("reset-button").classList.add("hide")

        const signed_line = document.createElement("p")
        signed_line.id = "signed-line"
        signed_line.innerText = playerdata.alias

        const signed_pop_corns = document.createElement("p")
        signed_pop_corns.id = "signed-pop-corns"
        signed_pop_corns.innerText = playerdata.pop_corns

        const alias = document.getElementById("alias")
        const pop_corns = document.getElementById("pop-corns")

        alias.replaceWith(signed_line)
        pop_corns.replaceWith(signed_pop_corns)

        const QFS = document.getElementsByName("QFS")[0]
        const DES = document.getElementsByName("DES")[0]
        const TLD = document.getElementsByName("TLD")[0]
        const CIA = document.getElementsByName("CIA")[0]
        const EPS = document.getElementsByName("EPS")[0]
        const IFS = document.getElementsByName("IFS")[0]


        if (playerdata.QFS){
            QFS.setAttribute("checked", "")
        }
        else {
            QFS.removeAttribute("checked")
        }

        if (playerdata.DES){
            DES.setAttribute("checked", "")
        }
        else {
            DES.removeAttribute("checked")
        }

        if (playerdata.TLD){
            TLD.setAttribute("checked", "")
        }
        else {
            TLD.removeAttribute("checked")
        }

        if (playerdata.CIA){
            CIA.setAttribute("checked", "")
        }
        else {
            CIA.removeAttribute("checked")
        }

        if (playerdata.EPS){
            EPS.setAttribute("checked", "")
        }
        else {
            EPS.removeAttribute("checked")
        }

        if (playerdata.IFS){
            IFS.setAttribute("checked", "")
        }
        else {
            IFS.removeAttribute("checked")
        }

        QFS.setAttribute("onclick", "return false")
        DES.setAttribute("onclick", "return false")
        TLD.setAttribute("onclick", "return false")
        CIA.setAttribute("onclick", "return false")
        EPS.setAttribute("onclick", "return false")
        IFS.setAttribute("onclick", "return false")



    }
}