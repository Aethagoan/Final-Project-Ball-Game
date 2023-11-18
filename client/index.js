// import api_call from "./svc/api_call.js";

// wait for the button to be on the DOM
while(!document.getElementsByClassName("button")) {
    await new Promise(r => {
        setTimeout(r, 50);
    });
}

const button = document.getElementsByClassName("button")[0]

button.addEventListener("mouseenter", () => {
    button.innerHTML = "yes. . . >:]"
})

button.addEventListener("mouseout", () => {
    button.innerHTML = "Enter"
})


// if there is local storage, remove the button and contract message elements.
