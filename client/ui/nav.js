// makes the nav (so I don't have to copy paste changes every time)
build_nav()

async function build_nav() {

    // wait until the nav exists on the DOM
    while(!document.querySelector("nav")) {
        await new Promise(r => {
            setTimeout(r, 50);
        });
    }


    const nav = document.getElementById("nav");

    const home = document.createElement("a");
    home.setAttribute("href","./")
    home.innerText = "HOME";

    const watch = document.createElement("a");
    watch.setAttribute("href","./watch.html")
    watch.innerText = "WATCH";

    const league = document.createElement("a");
    league.setAttribute("href","./league.html")
    league.innerText = "LEAGUE";

    const store = document.createElement("a");
    store.setAttribute("href","./store.html")
    store.innerText = "STORE";



    let currentlocation = location.href
    currentlocation = currentlocation.split("/")[3]
    if (currentlocation) {
        currentlocation = currentlocation.split(".")[0]
    }

    // console.log(currentlocation)

    if (currentlocation == "" || currentlocation == "index" ) {
        home.classList.add("current");
    }

    if (currentlocation == "watch") {
        watch.classList.add("current");
    }

    if (currentlocation == "league") {
        league.classList.add("current");
    }

    if (currentlocation == "store") {
        store.classList.add("current");
    }


    nav.appendChild(home);
    nav.appendChild(watch);
    nav.appendChild(league);
    nav.appendChild(store);

    //if localstorage exists, use the outerhtml tag to append <p>Welcome back, NAME</p>

}