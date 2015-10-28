﻿$(document).ready(function () {
    console.log("inne i ready Home")
    getGamesToIndex();

    console.log("Klar med ready script")
});

function getGamesToIndex() {
    $.ajax({
        url: "../Home/GamesInSixDays",
        dataType: "html",
        success: function (result) {
            console.log("Success");
            $('#showOnHomeIndex').html(result);
        }
    });
}

function goToBettingSite() {
    window.location.href = "../Betting/Index";
}

//function goToBettingSiteWithId(id) {
//    window.location.href = "../Betting/Index";

//    getLeagueStandings(id)
//    getBettableGames(id)
//    getLaguePlayedGames(id)
//    getLeagueNextGames(id)
//}

//function getBettableGames(id) {
//    $.ajax({
//        url: "../Betting/GetBettableGames?id=" + id,
//        dataType: "html",
//        success: function (result) {
//            console.log("Success");
//            $('#gamesThatIsBettable').html(result);
//        }
//    });
//}

//function getLeagueStandings(id) {
//    $.ajax({
//        url: "../Betting/GetLeagueStandings?id=" + id,
//        dataType: "html",
//        success: function (result) {
//            console.log("Success");
//            $('#leagueStandingsDiv').html(result);
//        }
//    });
//}

//function getLaguePlayedGames(id) {
//    $.ajax({
//        url: "../Betting/GetPlayedGames?id=" + id,
//        dataType: "html",
//        success: function (result) {
//            console.log("Success");
//            $('#leaguesPlayedGamesDiv').html(result);
//        }
//    });
//}

//function getLeagueNextGames(id) {
//    $.ajax({
//        url: "../Betting/GetNextGames?id=" + id,
//        dataType: "html",
//        success: function (result) {
//            console.log("Success");
//            $('#nextGamesDiv').html(result);
//        }
//    });
//}

