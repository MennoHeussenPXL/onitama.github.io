var tableData;
var gameData;
var selectedCard = null;
var selectedPawn = null;

document.querySelector(".openChatBtn").addEventListener("click", openForm);
document.querySelector(".close").addEventListener("click", closeForm);
function openForm() {
    document.querySelector(".openChat").style.display = "block";
}
function closeForm() {
    document.querySelector(".openChat").style.display = "none";
}
function getTableData(tableId) {
    return fetch('https://localhost:5051/api/Tables/' + tableId, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem("token")
        }
    })
        .then(response => response.json())
        .then(data => {
            tableData = data;
        });
}
function getGameData(gameId) {
    return fetch('https://localhost:5051/api/Games/' + gameId, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem("token")
        }
    })
        .then(response => {
            return response.json();
        })
        .then(data => {
            GameData = data;
        });
}

function selectCard(cardId) {
    var currentPlayerId = sessionStorage.getItem("id");
    var activePlayerId = GameData.playerToPlayId;
    var cardElement = document.getElementById(cardId);

    if (currentPlayerId !== activePlayerId) {
        console.log("It's not your turn!");
        return;
    }

    if (selectedCard) {
        document.getElementById(selectedCard).parentNode.classList.remove('selected');
    }

    selectedCard = cardId;
    cardElement.parentNode.classList.add('selected');

    if (selectedPawn) {
        updatePossibleMoves();
    }
}

/*function selectPawn(pawnId) {
    if (selectedPawn) {
        document.getElementById(selectedPawn).classList.remove('selected');
    }
    selectedPawn = pawnId;
    document.getElementById(pawnId).classList.add('selected');
    if (selectedCard) {
        updatePossibleMoves();
    }
}

function updatePossibleMoves() {
    if (selectedCard && selectedPawn) {
        var possibleMoves = getPossibleMoves(selectedCard, selectedPawn); 
        highlightPossibleMoves(possibleMoves);
    }
}

function highlightPossibleMoves(moves) {
    document.querySelectorAll('.grid').forEach(cell => {
        cell.classList.remove('highlight');
    });

    moves.forEach(move => {
        var cell = document.getElementById(`cell-${move.x}-${move.y}`);
        cell.classList.add('highlight');
    });
}

function setupBoard() {
    var pawns = document.querySelectorAll('.pawnImg');
    pawns.forEach(pawn => {
        pawn.addEventListener('click', function () {
            selectPawn(pawn.id);
        });
    });
}*/

function rotate180(grid) {
    const rows = grid.length;
    const cols = grid[0].length;
    let newGrid = Array.from({ length: rows }, () => Array(cols).fill(0));

    for (let i = 0; i < rows; i++) {
        for (let j = 0; j < cols; j++) {
            newGrid[rows - 1 - i][cols - 1 - j] = grid[i][j];
        }
    }
    return newGrid;
}
async function fetchDataTable(tableId) {
    var player2 = document.getElementById("player2");
    var message = document.getElementById("message");
    var startGameButton = document.getElementById("startGame");
    var leaveGameButton = document.getElementById("leaveGame");
    leaveGameButton.style.display = "flex";
    stop = false;
    while (stop == false) {
        await getTableData(tableId);
        if (tableData.ownerPlayerId == sessionStorage.getItem("id")) {
            startGameButton.style.display = "flex";
        }
        if (tableData.hasAvailableSeat) {
            player2.textContent = "Waiting for Players...";
        } else {
            tableData.seatedPlayers.forEach(function (item) {
                if (item.id != sessionStorage.getItem("id")) {
                    player2.textContent = item.name;
                }
            });
            message.style.removeProperty("color");
            message.textContent = "Waiting for party leader to start game...";
        }
        if (tableData.gameId != "00000000-0000-0000-0000-000000000000") {
            sessionStorage.setItem("gameId", tableData.gameId);
            startGameButton.style.display = "none";
            leaveGameButton.style.display = "none";
            message.textContent = "";
            fetchDataGame(sessionStorage.getItem("gameId"));
            stop = true;
        }
        await new Promise(resolve => setTimeout(resolve, 1000));
    }
}
async function fetchDataGame(gameId) {
    while (true) {
        await getGameData(gameId);
        console.log(GameData);
        var board = document.getElementById("board");
        var nextCard = document.getElementById("next_card");
        var nextCardName = document.getElementById("nextCardName");
        var players = GameData.players;
        var playMatGrid = GameData.playMat.grid;
        nextCardName.textContent = GameData.extraMoveCard.name;
        let gridExtraMoveCard = GameData.extraMoveCard.grid;

        var player1 = document.getElementById("player1");
        var player2 = document.getElementById("player2");
        if (GameData.playerToPlayId === sessionStorage.getItem("id")) {
            player1.classList.add("activePlayer");
            player2.classList.remove("activePlayer");
        } else {
            player2.classList.add("activePlayer");
            player1.classList.remove("activePlayer");
        }
        if (GameData.playerToPlayId != sessionStorage.getItem("id")) {
            gridExtraMoveCard = rotate180(GameData.extraMoveCard.grid);
        } else {
            gridExtraMoveCard = GameData.extraMoveCard.grid;
        }
        for (var i = 0; i < gridExtraMoveCard.length; i++) {
            var row = gridExtraMoveCard[i];
            for (var j = 0; j < row.length; j++) {
                var item = row[j];
                var color;
                var cardCell = nextCard.rows[i].cells[j];
                if (item == 1) {
                    cardCell.style.backgroundColor = GameData.extraMoveCard.stampColor;
                } else if (item == 2) {
                    cardCell.style.backgroundColor = "gray";
                } else {
                    cardCell.style.backgroundColor = "";
                }
            }
        }
        for (var a = 0; a < players.length; a++) {
            var player = players[a];
            var playerColor = player.color;
            var playerMoveCards = player.moveCards;
            for (var i = 0; i < playerMoveCards.length; i++) {
                let moveCard;
                var playerNumber;
                if (player.id != sessionStorage.getItem("id")) {
                    moveCard = rotate180(playerMoveCards[i].grid);
                    playerNumber = "player2";
                } else {
                    moveCard = playerMoveCards[i].grid;
                    playerNumber = "player1";
                }
                for (var k = 0; k < moveCard.length; k++) {
                    var row = moveCard[k];
                    for (var j = 0; j < row.length; j++) {
                        var number = row[j];
                        if (i == 0) {
                            var playerCardName = document.getElementById(playerNumber + "CardName1");
                            var playerCard = document.getElementById(playerNumber + "Card1");

                        } else if (i == 1) {
                            var playerCardName = document.getElementById(playerNumber + "CardName2");
                            var playerCard = document.getElementById(playerNumber + "Card2");
                        }
                        playerCardName.textContent = playerMoveCards[i].name;
                        var playerCell = playerCard.rows[k].cells[j];
                        if (number == 1) {
                            playerCell.style.backgroundColor = playerColor;
                        } else if (number == 2) {
                            playerCell.style.backgroundColor = "gray";
                        } else {
                            playerCell.style.backgroundColor = "";
                        }
                    }
                }
            }
        }
        let grid;
        if (tableData.ownerPlayerId == sessionStorage.getItem("id")) {
            grid = rotate180(playMatGrid);
        } else {
            grid = playMatGrid;
        }
        for (var i = 0; i < grid.length; i++) {
            var row = grid[i];
            for (var j = 0; j < row.length; j++) {
                var item = row[j];
                var color;
                var cell = board.rows[i].cells[j];
                var img = cell.querySelector("img");
                if (img) {
                    cell.removeChild(img);
                }
                if (item != null) {
                    players.forEach(function (player) {
                        if (player.id == item.OwnerId) {
                            color = player.color;
                        }
                    });
                    var img = document.createElement("img");
                    img.classList.add("pawnImg");
                    if (item.Type == 1) {
                        img.src = "pawns/" + color + "Pawn.png";
                        cell.appendChild(img);
                    } else if (item.Type == 0) {
                        img.src = "pawns/" + color + "Master.png";
                        cell.appendChild(img);
                    }
                }/* else {
                    var img = cell.querySelector("img");
                    if (img) {
                        cell.removeChild(img);
                    }
                }*/
            }
        }
        await new Promise(resolve => setTimeout(resolve, 1000));
    }
}
document.addEventListener('DOMContentLoaded', async function () {
    var player1 = document.getElementById("player1");
    tableId = sessionStorage.getItem("tableId");
    warriorName = sessionStorage.getItem("warriorName");
    player1.textContent = warriorName;
    console.log(tableId);
    fetchDataTable(tableId);
    document.getElementById('leaveGame').addEventListener('click', function (event) {
        fetch('https://localhost:5051/api/Tables/' + tableId + '/leave', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            }
        })
            .then(response => {
                if (!response.ok) {
                    if (response.status == 401) {
                        window.location.href = 'index.html';
                    }
                    console.error(response.json())
                    return response.json()
                } else {
                    window.location.href = 'lobby.html';
                };
            }).then(data => {
                console.log(data.message);
            });
    });
    document.getElementById('startGame').addEventListener('click', function (event) {
        fetch('https://localhost:5051/api/Tables/' + tableId + '/start-game', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            }
        })
            .then(response => {
                if (!response.ok) {
                    if (response.status == 401) {
                        window.location.href = 'index.html';
                    }
                    return response.json();
                } else {
                    return;
                };
            }).then(data => {
                if (data != null) {
                    message.style.color = "red";
                    message.textContent = data.message;
                }
            });
    });
    /*const table = document.getElementById("board");
    let previousCell = null;
    table.addEventListener("click", function (event) {
        const target = event.target.closest("td");

        if (target) {
            if (previousCell) {
                previousCell.classList.remove("selectPawn");
            }
            target.classList.add("selectPawn");
            previousCell = target;
        }*/
    setupBoard();
});

function setupBoard() {
    var pawns = document.querySelectorAll('.pawnImg');
    pawns.forEach(pawn => {
        pawn.addEventListener('click', function () {
            selectPawn(pawn.id);
        });
    });
}



