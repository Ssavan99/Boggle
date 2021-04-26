$(document).ready(function () {
    $("#sc_game").hide();
    $("#sc_lobby").hide();

    $("#btn_newgame").click(function () {
        boggle.username = $("#txt_username").val();
        newGame()
            .then(function (ngResp) {
                console.log("new game: ", ngResp);
                if (!ngResp.ok) {
                    return $.Deferred().reject(ngResp.msg);
                }
                boggle.gameId = ngResp.gameId;
                return login();
            })
            .then(function (loginResp) {
                console.log("login: ", loginResp);
                if (!loginResp.ok) {
                    return $.Deferred().reject(loginResp.msg);
                }
            })
            .then(getGameState)
            .then(function (stt) {
                initGame(stt);
            })
            .fail(function (err) {
                alert("Fail: " + err);
            });
    });

    $("#btn_startgame").click(function () {
        startGame();
    });

    $("#btn_leavelobby").click(function () {
        //CODE TO BE ADDED
        //remove player from game
        removePlayer();
    });

    $("#btn_joingame").click(function () {
        boggle.gameId = parseInt($("#txt_gameid").val());
        boggle.username = $("#txt_username").val();
        login()
            .then(function (loginResp) {
                console.log("login: ", loginResp);
                if (!loginResp.ok) {
                    return $.Deferred().reject(loginResp.msg);
                }
            })
            .then(getGameState)
            .then(function (stt) {
                initGame(stt);
            })
            .fail(function (err) {
                alert("Fail: " + err);
            });
    });

    $("#btn_guess").click(function () {
        str = "";
        var s = boggle.selected;
        for (var i = 0; i < s.length; i++) {
            str += s[i].i.toString() + s[i].j.toString() + " ";
        }
        str = str.trim();
        guess(str).then(function (r) {
            if (!r.ok) {
                alert(r.msg);
            }
            $("#btn_resetguess").click();
            refreshState(boggle.gameId);
        });
    });
    $("#btn_resetguess").click(function () {
        boggle.selected = [];
        renderSelected();
    });

    $("#btn_endgame").click(function () {
        endGame().then(function () {
            refreshState(boggle.gameId);
        })
    });
});