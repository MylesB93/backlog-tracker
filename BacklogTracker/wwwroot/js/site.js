function addToBacklog(element, email) {
    const apiUrl = '/api/backlog/add-game-to-backlog';

    const requestData = {
        Email: email,
        GameID: element.getAttribute("data-game-id")
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
            else if (response.status == 409)
                throw new Error("Game already exists in backlog!");
            else
                throw new Error('Network response was not ok.')
        })
        .then(data => {
            console.log('Data successfully sent to the API:', data);
            alert(element.getAttribute("data-game-name") + " successfully added to backlog!");
        })
        .catch(error => {
            var errorMsg = document.getElementById("errorMessage");
            console.error('There was a problem sending data to the API:', error);
            errorMsg.innerHTML = error;
            errorMsg.style.visibility = 'visible';
        });

}

function removeFromBacklog(element, email) {
    const apiUrl = '/api/backlog/remove-game-from-backlog';

    const requestData = {
        Email: email,
        GameID: element.getAttribute("data-game-id")
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
            else
                throw new Error('Network response was not ok.')
        })
        .then(data => {
            console.log('Data successfully sent to the API:', data);
            location.reload();
        })
        .catch(error => {
            var errorMsg = document.getElementById("errorMessage");
            console.error('There was a problem sending data to the API:', error);
            errorMsg.innerHTML = error;
            errorMsg.style.visibility = 'visible';
        });
}

function addToCompleted(element, email) {
    const apiUrl = '/api/backlog/add-game-to-completed-games';

    const requestData = {
        Email: email,
        GameID: element.getAttribute("data-completed-game-id")
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
            else
                throw new Error('Network response was not ok.')
        })
        .then(data => {
            console.log('Data successfully sent to the API:', data);
            location.reload();
        })
        .catch(error => {
            var errorMsg = document.getElementById("errorMessage");
            console.error('There was a problem sending data to the API:', error);
            errorMsg.innerHTML = error;
            errorMsg.style.visibility = 'visible';
        });
}

function moveFromCompletedTobacklog(element, email) {
    const apiUrl = '/api/backlog/move-from-completed-games-to-backlog';

    const requestData = {
        Email: email,
        GameID: element.getAttribute("data-game-id")
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
            else
                throw new Error('Network response was not ok.')
        })
        .then(data => {
            console.log('Data successfully sent to the API:', data);
            location.reload();
        })
        .catch(error => {
            var errorMsg = document.getElementById("errorMessage");
            console.error('There was a problem sending data to the API:', error);
            errorMsg.innerHTML = error;
            errorMsg.style.visibility = 'visible';
        });
}