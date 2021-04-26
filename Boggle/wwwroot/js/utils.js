var boggle = {
    size: 4,
    selected: []
};

function initGame(g) {
    $("#sc_start").hide();
    $("#sc_lobby").hide();
    $("#sc_game").fadeIn();
    $("#lbl_username").text(boggle.username);

    startGame(g);

    console.log("game state: ", g);
    fillBoard(g.board);

    refreshState(g.gameId, true);
}

function initLobby(g) {
    $("#lbl_gameid").text(g.gameId);
    $("#sc_start").hide();
    $("#sc_lobby").fadeIn();
}

function initStart() {
    $("#sc_lobby").hide();
    $("#sc_start").fadeIn();
}

function cell(i, j) {
    return $("#cell_" + i + j);
}

function cellClick(i, j) {
    var sel = boggle.selected;
    for (var l = 0; l < sel.length - 1; l++) {
        if (sel[l].i === i && sel[l].j === j) {
            return;
        }
    }
    if (sel.length > 0) {
        if (sel[sel.length - 1].i === i && sel[sel.length - 1].j === j) {
            //deselecting the last letter
            cell(i, j).css("background-color", "");
            sel.pop();
        } else {
            sel.push({ i: i, j: j });
            renderSelected();
        }
    } else {    //first one stored as it is
        sel.push({ i: i, j: j });
        renderSelected();
    }
}

function fillBoard(board) {
    for (var i = 0; i < boggle.size; i++) {
        for (var j = 0; j < boggle.size; j++) {
            (function (i, j) {
                cell(i, j)
                    .text(board[i][j])
                    .click(function () { cellClick(i, j); });
            })(i, j);
        }
    }
}

function renderSelected() {
    for (var i = 0; i < boggle.size; i++) {
        for (var j = 0; j < boggle.size; j++) {
            cell(i, j).css("background-color", "");
        }
    }
    var s = boggle.selected;
    for (var i = 0; i < s.length; i++) {
        cell(s[i].i, s[i].j).css("background-color", "lightgreen");
    }
}

function refreshState(gameid, auto) {
    if (boggle.gameId != gameid) return;

    getGameState().then(function (g) {
        console.log("= refresh: ", g);
        if (!g.ok) {
            alert("fail to refresh game state: " + g.msg);
            return;
        }

        var tbody = $("#tbl_scoreboard tbody");
        tbody.html("");
        for (var i = 0; i < g.users.length; i++) {
            var u = g.users[i];
            var tr = $("<tr/>");
            $("<td/>").text(u).appendTo(tr);
            $("<td/>").text(g.ended ? g.userScores[u] : "?").appendTo(tr);
            $("<td/>").text(g.userGuesses[u]).appendTo(tr);
            $("<td/>").text(g.userGuessesOk[u]).appendTo(tr);
            tbody.append(tr);
        }

        if (g.ended) {
            $("#lbl_time").html("<b>Game is ended</b>");
        } else {
            $("#lbl_time").text(g.remainingTime + " s");
        }
        if (auto && !g.ended) {
            setTimeout(function () {
                refreshState(gameid, true);
            }, 500);
        }
    })
}