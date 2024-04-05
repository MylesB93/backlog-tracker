function addToBacklog(element) {
    //console.log(element.getAttribute("data-game-guid"));

    const apiUrl = '/api/backlog/patch-user-backlog';

    // TODO: Define the data you want to send to the API endpoint
    const requestData = {
        data: '{Email:"mylesbroomes@hotmail.co.uk", GameID: "321"}'
    };

    fetch(apiUrl, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Data successfully sent to the API:', data);
        })
        .catch(error => {
            console.error('There was a problem sending data to the API:', error);
        });

}