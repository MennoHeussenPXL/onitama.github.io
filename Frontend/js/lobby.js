var availableSeatsData;
var selectedElement = null;
function clearSelection() {
    if (selectedElement) {
        selectedElement.classList.remove("selected");
        selectedElement = null;
    }
}

function handleItemClick(event) {
    clearSelection();
    var clickedElement = event.target.closest('.list-item');
    if (clickedElement) {
        clickedElement.classList.add("selected");
        selectedElement = clickedElement;
    }
}

function getAvailableSeats() {
    return fetch('https://localhost:5051/api/Tables/with-available-seats', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem("token")
        }
    }).then(response => {
        if (!response.ok) {
            if (response.status == 401) {
                window.location.href = 'index.html';
            }
            console.error(response.json());
            return;
        } else {
            return response.json();
        }
    }).then(data => {
        availableSeatsData = data;
        return data;
    });
}

async function fetchAvailableSeats() {
    var scrollableList = document.getElementById("scrollable-list");
    var listItem = document.createElement("div");
    listItem.addEventListener("click", handleItemClick);
    while (true) {
        const data = await getAvailableSeats();
        console.log(data);
        scrollableList.innerHTML = "";
        data.forEach(function (item) {
            var ownerPlayer;
            var seatedPlayers = "";
            item.seatedPlayers.forEach(function (player) {
                seatedPlayers += " " + player.name;
                if (player.id == item.ownerPlayerId) {
                    ownerPlayer = player.name;
                }
            });
            listItem.textContent = "owner = " + ownerPlayer + ", players =" + seatedPlayers;
            listItem.classList.add("list-item");
            listItem.setAttribute("id", item.id);
            
            scrollableList.appendChild(listItem);
        });

        await new Promise(resolve => setTimeout(resolve, 1000));
    }
}

document.addEventListener('DOMContentLoaded', function () {
    fetchAvailableSeats();
    document.getElementById('button3').addEventListener('click', function (event) {
        const DataJSON = {
            "numberOfPlayers": 2,
            "playerMatSize": 5,
            "moveCardSet": 0
        };
        fetch('https://localhost:5051/api/Tables', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            },
            body: JSON.stringify(DataJSON),
        }).then(response => {
            if (!response.ok) {
                if (response.status == 401) {
                    window.location.href = 'index.html';
                }
                console.error(response.json());
            } else {
                return response.json();
            }
        }).then(data => {
            sessionStorage.setItem("tableId", data.id);
            window.location.href = 'game.html';
        });
    });

    document.getElementById('button4').addEventListener('click', function (event) {
        if (selectedElement) {
            var tableId = selectedElement.getAttribute("id");
            fetch('https://localhost:5051/api/Tables/' + tableId + '/join', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage.getItem("token")
                }
            }).then(response => {
                if (!response.ok) {
                    if (response.status == 401) {
                        window.location.href = 'index.html';
                    }
                    console.error(response.json());
                } else {
                    return response.json();
                }
            }).then(data => {
                sessionStorage.setItem("tableId", data.id);
                window.location.href = 'game.html';
            });
        } else {
            console.log("No element selected.");
        }
    });

    document.getElementById('button1').addEventListener('click', function (event) {
        window.location.href = 'regels.html';
    });
});