import api_calls from "./svc/api_calls.js";

// wait for the button to be on the DOM
while(!document.getElementsByClassName("button")) {
    await new Promise(r => {
        setTimeout(r, 50);
    });
}

const button = document.getElementsByClassName("button")[0]
const sign_message = document.getElementById("sign-message")

button.addEventListener("mouseenter", () => {
    button.innerHTML = "yes. . . >:]"
})

button.addEventListener("mouseout", () => {
    button.innerHTML = "Enter"
})


// if there is local storage, remove the button and contract message elements.
if (localStorage.length > 0){
    
    const playerdata = await ((await api_calls.get_player_data(localStorage.getItem("token"))).json())

    if (playerdata != null) {
        button.classList.add("hide")
        sign_message.classList.add("hide")
    }

    
}