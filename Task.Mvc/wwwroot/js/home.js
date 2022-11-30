const hub = new signalR.HubConnectionBuilder()
    .withUrl("/lobby")
    .build();

window.onload = async () => {
    await hub.start()
}

hub.on("SendLobbyInfo", async () => {
    let container = document.getElementsByClassName('free-games-container')[0]
    container.innerHTML = ""
    let response = await fetch("/Home/LoadFreeLobby/", {
        method: 'POST',
        body: JSON.stringify(),
        headers: {
            "Content-Type": "application/json"
        }
    })

    addToHtml(response)
})

function addToHtml(response) {
    let container = document.getElementsByClassName('free-games-container')[0]
    let responseJson = response.json();

    responseJson.then(data => {
        data.forEach((item) => {
            let html = `
                <row class="free-game mb-2">
                    <div class="row fw-bolder">Сreator: ${item.creator}</div>
                    <div class="row fw-bolder">Connection id: ${item.connectionId}</div>
                </row>
            `
            console.log(item)
            container.innerHTML += html
        })
    })
}

let createBtn = document.querySelector('#create-btn')
createBtn.addEventListener("click", async function () {
    let playerName = document.querySelector('#player-name').value
    await fetch("/Home/CreateLobby/", {
        method: 'POST',
        body: JSON.stringify(playerName),
        headers: {
            "Content-Type": "application/json"
        }
    })
})

let connectionIdInput = document.getElementById('connect-id-input')
let connectButton = document.querySelector('#connect-btn')

function checkConnectionInput() {
    if (connectionIdInput.value.length > 0) {
        connectButton.removeAttribute('disabled')
    } else {
        connectButton.setAttribute('disabled', 'disabled')
    }
}

connectionIdInput.oninput = () => checkConnectionInput();
