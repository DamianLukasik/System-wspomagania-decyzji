$(document).ready(function () {
    var state = true;
    $("#button").click(function () {
        if (state) {
            $("#effect").animate({
                backgroundColor: "#aa0",
                color: "#0ff",
                width: 500
            }, 1800, "easeOutQuint");
        } else {
            $("#effect").animate({
                backgroundColor: "#a0a",
                color: "#0f0",
                width: 240
            }, 1900, "easeOutQuint");
        }
        state = !state;//negacja wartości
    });
    
});

setInterval("odświeżanie();",1000); 
function odświeżanie(){
    $('#czas').load(location.href + ' #czas');
}