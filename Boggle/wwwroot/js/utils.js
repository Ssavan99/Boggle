var boggle = {
    size: 4,
    selected: []
};

function backToStartScreen() {
    boggle.gameId = -1;
    $("#sc_lobby").hide();
    $("#sc_game").fadeOut();
    $("#sc_start").fadeIn();
}

function initGame(g) {
    $("#sc_start").hide();
    $("#sc_lobby").fadeIn();
    $("#sc_game").hide();
    $("#lbl_gameid").text(g.gameId);
    $("#lbl_username").text(boggle.username);

    console.log("game state: ", g);
    fillBoard(g.board);

    refreshState(g.gameId, true);
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
    if (boggle.gameId !== gameid) {
        console.log("stop refresh state");
        return;
    }

    getGameState().then(function (g) {
        if (boggle.gameId !== gameid) {
            console.log("stop refresh state");
            return;
        }

        console.log("= refresh: ", g);
        if (!g.ok) {
            alert("fail to refresh game state: " + g.msg);
            return;
        }
        ended = (g.state === 2);
        if (g.state === 0) { // Lobby
            $("#sc_lobby").show();
            $("#sc_game").hide();

            var str = "";
            for (var i = 0; i < g.users.length; i++) {
                if (i > 0)
                    str += ", ";
                str += g.users[i];
            }
            $("#lbl_members").text(str);
        } else { // Playing/Ended
            $("#sc_lobby").hide();
            $("#sc_game").show();

            var tbody = $("#tbl_scoreboard tbody");
            tbody.html("");
            for (var i = 0; i < g.users.length; i++) {
                var u = g.users[i];
                var tr = $("<tr/>");
                $("<td/>").text(u).appendTo(tr);
                $("<td/>").text(ended ? g.userScores[u] : "?").appendTo(tr);
                $("<td/>").text(g.userGuesses[u]).appendTo(tr);
                $("<td/>").text(g.userGuessesOk[u]).appendTo(tr);
                tbody.append(tr);
            }

            if (ended) {
                $("#lbl_time").html("<b>Game is ended</b>");
            } else {
                $("#lbl_time").text(g.remainingTime + " s");
            }
        }

        if (auto && !ended) {
            setTimeout(function () {
                refreshState(gameid, true);
            }, 500);
        }
    })
}