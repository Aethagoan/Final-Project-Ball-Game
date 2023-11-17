// import api_call from "./svc/api_call.js";

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