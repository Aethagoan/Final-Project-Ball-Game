
// request something from localhost
let api_message = await fetch(
    "http://localhost:5184", 
    {
    method: "GET"
    }
)

// turn it into json
api_message = await api_message.json()

console.log(api_message)
// console.log(Object.entries(api_message))


// request something from localhost
api_message = await fetch(
    "http://localhost:5184", {
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({
            name: "first item",
            description: "desc"
        })
    }
)

// turn it into json
api_message = await api_message.json()

console.log(api_message)
// console.log(Object.entries(api_message))

// request something from localhost
api_message = await fetch(
    "http://localhost:5184", 
    {
    method: "GET"
    }
)

// turn it into json
api_message = await api_message.json()

console.log(api_message)
