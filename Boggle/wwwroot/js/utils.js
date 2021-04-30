var boggle = {
    size: 4,
    selected: [],
    gameId: -1,
    refreshGameId: -1,
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
    $(".cls_gameid").text(g.gameId);
    $("#lbl_username").text(boggle.username);

    console.log("game state: ", g);
    fillBoard(g.board);

    if (boggle.refreshGameId === -1) {
        refreshState(g.gameId, true);
    } else {
        console.log("already refreshing")
    }
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
                    .off('click')
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
        boggle.refreshGameId = -1;
        console.log("stop refresh state");
        return;
    }
    if (auto) {
        boggle.refreshGameId = gameid;
    }

    getGameState().then(function (g) {
        if (boggle.gameId !== gameid) {
            boggle.refreshGameId = -1;
            console.log("stop refresh state");
            return;
        }

        console.log("= refresh: ", g);
        if (!g.ok) {
            boggle.refreshGameId = -1;
            alert("fail to refresh game state: " + g.msg);
            return;
        }
        fillBoard(g.board);
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
            var u = boggle.username;
            // update guess table
            g.userGuesses[u].forEach(function (word) {
                var tr = $("<tr/>");
                $("<td/>").text(word).appendTo(tr);
                if (ended) {
                    let found = false;
                    // words in userGuessesOk are unique and verified and counted in user score
                    g.userGuessesOk[u].forEach(function (guess) {
                        if (guess == word) {
                            found = true;
                            var n = word.length;
                            if (n < 5)
                                $("<td/>").text("1").appendTo(tr);
                            if (n == 5)
                                $("<td/>").text("2").appendTo(tr);
                            if (n == 6)
                                $("<td/>").text("3").appendTo(tr);
                            if (n == 7)
                                $("<td/>").text("5").appendTo(tr);
                            if (n >= 8)
                                $("<td/>").text("11").appendTo(tr);
                        }
                    });
                    if (!found) {
                        $("<td/>").text("0").appendTo(tr);
                    }
                    /*wordScore(word).then(function (score) {
                        if (!score.ok) {
                            boggle.refreshGameId = -1;
                            alert("fail to get score: ");
                            return;
                        }
                        $("<td/>").text(score.score).appendTo(tr);
                    });*/
                } else {
                    $("<td/>").text("?").appendTo(tr);
                }
                tbody.append(tr);
            });

            var tr = $("<tr/>");
            $("<td/>").text("Total").appendTo(tr);
            $("<td/>").text(ended ? g.userScores[u] : "?").appendTo(tr);
            tbody.append(tr);


            if (ended) {
                $("#lbl_time").html("<b>Game is ended</b>");
            } else {
                $("#lbl_time").text(g.remainingTime + " s");
            }
        }

        if (auto) {
            setTimeout(function () {
                refreshState(gameid, true);
            }, 500);
        }
    })
}