//Khu vực biến
var checkbox = document.getElementById("checkpass");

var pass = document.getElementById("Password");

var form = document.querySelector(".form-login");

var formLogin = document.querySelector("form");

var err = document.querySelector(".validation-summary-errors");

var text = document.querySelector("h4[id='start'] img")

let intervalId1 = null;
let intervalId2 = null;
let stopped = false;

console.log(text);

//Khu vực hàm, xử lý
checkbox.addEventListener("change", function (e) {
    e.preventDefault();

    if (this.checked) {
        pass.type = "text";
    }
    else {
        pass.type = "password";
    }

});

//

form.addEventListener("click", function () {
    this.classList.add("active");
});

let images = [
    "/wwwroot/Images/Protect.png",
    "/wwwroot/Images/Error.png"
];
let current = 0;

function startInterval() {
    if (intervalId1 !== null || stopped) return;

    intervalId1 = setInterval(() => {
        // fade out
        text.style.opacity = 0;

        setTimeout(() => {
            // đổi ảnh khi đã fade out
            current = (current + 1) % images.length;
            text.src = images[current];

            // fade in
            text.style.opacity = 1;
        }, 500); // 500ms trùng với CSS transition

        form.classList.toggle("MessageErr"); // hiệu ứng form
    }, 2000);
}


function startIntervalLogin() {

    if (intervalId2 !== null) return;


    intervalId2 = setInterval(() => {
        form.classList.toggle("MessageLogin");
    }, 2000);


};


function stopInterval() {
    clearInterval(intervalId1);
    clearInterval(intervalId2);
    intervalId1 = null;
    intervalId2 = null;
    form.classList.remove("MessageErr");
    form.classList.remove("MessageLogin");
    text.src = images[0];

    text.style.opacity = 1;
}

document.addEventListener("DOMContentLoaded", function () {
    if (err !== null) {
        startInterval();
    } else {
        startIntervalLogin();
    }
});


form.addEventListener("mouseover", function () {
    if (!stopped) stopInterval();
    form.classList.remove("MessageErr");
});

form.addEventListener("mouseout", function () {
    if (err !== null) {
        if (!stopped)
            startInterval();
    } else {
        if (!stopped)
            startIntervalLogin();
    }
});


form.onclick = () => {
    stopped = true;
    stopInterval();
    form.classList.remove("MessageErr");
    form.classList.remove("MessageLogin");

}