function ajaxReq(url, data) {
    return $.ajax({
        url: url,
        dataType: "json",
        type: "get",
        data: data,
    });
}

function newGame() {
    return ajaxReq("/Server/newGame");
}

function startGame() {
    return ajaxReq("/Server/startGame", {
        gameId: boggle.gameId
    });
}

function login() {
    return ajaxReq("/Server/login", {
        gameId: boggle.gameId,
        username: boggle.username
    });
}

function removePlayer(gameid) {
    return ajaxReq("/Server/removePlayer", {
        gameId: gameid,
        username: boggle.username
    });
}

function getGameState() {
    return ajaxReq("/Server/getGameState", {
        gameId: boggle.gameId,
        username: boggle.username
    });
}

function guess(coords) {
    return ajaxReq("/Server/guess", {
        gameId: boggle.gameId,
        username: boggle.username,
        strcoords: coords
    });
}

function endGame() {
    return ajaxReq("/Server/endGame", {
        gameId: boggle.gameId
    });
}

function resetGame() {
    return ajaxReq("/Server/resetGame", {
        gameId: boggle.gameId
    });
}

function wordScore(word) {
    return ajaxReq("/Server/wordScore", {
        gameId: boggle.gameId,
        username: boggle.username,
        word: word
    });
}

function getGameLog() {
    return ajaxReq("/Server/getGameLog", {
        gameId: boggle.gameId
    });
}
