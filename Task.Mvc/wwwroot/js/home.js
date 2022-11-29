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

function checkConnectionInput(){
    if (connectionIdInput.value.length > 0) {
        connectButton.removeAttribute('disabled')
    }
    else{
        connectButton.setAttribute('disabled', 'disabled')
    }
}

connectionIdInput.oninput = () => checkConnectionInput();
