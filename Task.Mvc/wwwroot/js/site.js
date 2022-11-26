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
