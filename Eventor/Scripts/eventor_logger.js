
/*Logger class to do*/

var globalLog = [];
var globalLogCounter = 0;

function Logger() {
};

Logger.log = function (location, message) {
    globalLogCounter++;
    console.log(globalLogCounter + ':' + location + ':\n' + message);
    globalLog.push(location + ':' + message);
}

Logger.success = "Finished with success";
Logger.error = "Finished with error";
Logger.fail = "Finished with failure";
//to do
/*Logger.globLogShow = function () {
    var csvContent = "data:text/csv;charset=utf-8,";
    csvContent += "location, logmessage\n";
    var rowNo = 0;
    var row;
    globalLog.forEach(function (rawRow) {
        row = [rawRow[0], rawRow[1]];
        rowNo += 1;
        var dataString = row.join(",");
        csvContent += rowNo < globalLog.length ? dataString + "\n" : dataString;
    });

    var encodedUri = encodeURI(csvContent);
    var link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "profile.csv");
    link.click();
}*/