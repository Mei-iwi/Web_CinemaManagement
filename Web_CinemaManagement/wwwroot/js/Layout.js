//Khu vực biến
var logout = document.getElementById("logout");

var Message = $(".message")


var button = document.querySelectorAll(".message .d-flex button")



//Hảm xủ lý
logout.addEventListener("click", function (e) {
    e.preventDefault();

    Message.fadeIn();

});


//
button[0].addEventListener("click", function (e) {
    e.preventDefault();

    Message.fadeOut();

});

button[1].addEventListener("click", function () {
    window.location.href = "/Authentication/Logout";
});