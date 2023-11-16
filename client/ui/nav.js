


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
    home.setAttribute("href","/client/index.html")
    home.innerText = "HOME";

    const watch = document.createElement("a");
    watch.setAttribute("href","/client/watch.html")
    watch.innerText = "WATCH";

    const league = document.createElement("a");
    league.setAttribute("href","/client/league.html")
    league.innerText = "LEAGUE";

    const store = document.createElement("a");
    store.setAttribute("href","/client/store.html")
    store.innerText = "STORE";



    const currentlocation = location.href.split("/")[4].split(".")[0]
    console.log(currentlocation)

    if (currentlocation == "index") {
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

}