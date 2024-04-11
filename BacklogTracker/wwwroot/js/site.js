function addToBacklog(element, email) {
    const apiUrl = '/api/backlog/add-game-to-backlog';

    const requestData = {
        Email: email,
        GameID: element.getAttribute("data-game-guid")
    };

    fetch(apiUrl, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestData)
    })
        .then(response => {
            if (response.ok)
                return response.json();
            else if (response.status === 409)
                return response.json().then(data => {
                    throw new Error(data.errorMessage);
                });
            else
                throw new Error('Network response was not ok.')
        })
        .then(data => {
            console.log('Data successfully sent to the API:', data);
        })
        .catch(error => {
            var errorMsg = document.getElementById("errorMessage");
            console.error('There was a problem sending data to the API:', error);
            errorMsg.innerHTML = error;
            errorMsg.style.visibility = 'visible';
        });        

}