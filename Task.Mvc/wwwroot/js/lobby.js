const playerName = document.querySelector('#player-name').value
const connectionId = document.querySelector('#connection-id').innerHTML
let cells = document.getElementsByClassName("game-cell");
let stepSymbol;
let playerStep;
let steps = []
let lobbyResult;
let status;

const hub = new signalR.HubConnectionBuilder()
    .withUrl("/lobby")
    .build();

window.onload = async () => {
    await hub.start()
    
    hub.invoke("CheckPlayers", connectionId, playerName)
    hub.invoke("GetStepSymbol", playerName)
    hub.invoke("GetPlayerNameStep", connectionId)
    hub.invoke("GetField", connectionId, playerName)
    hub.invoke("GetLobbyStatus", connectionId)
}

hub.on("CheckPlayers", function (message) {
    if (message.playersCount === 2)
        removeWaitingPlayerBanner()
})

hub.on("GetStepSymbol", function (symbol) {
    stepSymbol = symbol
    console.log(stepSymbol)
})

hub.on("GetPlayerNameStep", function (player) {
    playerStep = player
    setPlayerNameStep(player)
})

hub.on("Step", function (cells) {
    console.log(cells)
    setCellsValue(cells)
})

hub.on("GetField", function (field) {
    setCellsValue(field)
})

hub.on("GetLobbyResult", function (result) {
    lobbyResult = result
    showLobbyResult()
})

hub.on("GetLobbyStatus", function (result) {
    status = result
})

hub.on("Restart", function (result) {
    hideLobbyResult()
    hub.invoke("GetStepSymbol", playerName)
})

function removeWaitingPlayerBanner() {
    let banner = document.querySelector('.wait-container')
    banner.remove()
}

function step(elem) {
    if (playerStep === playerName && elem.innerHTML === ""){
        elem.innerHTML = stepSymbol
        hub.invoke("Step", connectionId, playerName, elem.innerHTML, +elem.id - 1)
    }
}

function setCellsValue(array){
    for (let i = 0; i < array.length; i++) {
        cells[i].innerHTML = array[i]
    }
}

function setPlayerNameStep(player){
    let stepBanner = document.querySelector('#player-step')
    if (player === playerName){
        stepBanner.innerHTML = `Player turn: You`
    }
    else{
        stepBanner.innerHTML = `Player turn: ${player}`
    }
}

function showLobbyResult(){
    let text = ""
    
    if (lobbyResult === playerName)
        text = "You won"
    if (lobbyResult !== "" && lobbyResult !== playerName) 
        text = `Winner is ${lobbyResult}`
    if (lobbyResult === "Draw")
        text = "Draw"
    
    let resultContainer = document.querySelector('#result-container')
    let resultContainerText = document.querySelector('#result-container-text')
    
    if (text !== ""){
        resultContainerText.innerHTML = text
        resultContainer.style.display = 'flex'
    }
}     

function hideLobbyResult(){
    let resultContainer = document.querySelector('#result-container')
    resultContainer.style.display = 'none'
}

let restartBtn = document.querySelector('#restart-btn')
restartBtn.addEventListener('click', () => {
    lobbyResult = ""
    hub.invoke("Restart", connectionId)
})