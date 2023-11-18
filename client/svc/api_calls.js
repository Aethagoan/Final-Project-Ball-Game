export default {
    submit_contract,
    testapithing,
    get_player_data
}

// IMPORTANT: CHANGE THIS IS YOU REDO THE WEB APP!
const API_PORT = "5184"


// // request something from localhost
// let api_message = await fetch(
//     "http://localhost:" + API_PORT, 
//     {
//     method: "GET"
//     }
// )

// // turn it into json
// api_message = await api_message.json()

// console.log(api_message)
// // console.log(Object.entries(api_message))


// // request something from localhost
// api_message = await fetch(
//     "http://localhost:" + API_PORT, {
//         method: "POST",
//         headers: {'Content-Type': 'application/json'},
//         body: JSON.stringify({
//             name: "first item",
//             description: "desc"
//         })
//     }
// )

// // turn it into json
// api_message = await api_message.json()

// console.log(api_message)
// // console.log(Object.entries(api_message))

// // request something from localhost
// api_message = await fetch(
//     "http://localhost:" + API_PORT, 
//     {
//     method: "GET"
//     }
// )

// // turn it into json
// api_message = await api_message.json()

// console.log(api_message)

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
