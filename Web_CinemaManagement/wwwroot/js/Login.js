//Khu vực biến
var checkbox = document.getElementById("checkpass");

var pass = document.getElementById("Password");

var form = document.querySelector(".form-login");

var formLogin = document.querySelector("form");

var err = document.querySelector(".validation-summary-errors");

var text = document.querySelector("h4[id='start']")

let intervalId = null;

console.log(form);

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

function startInterval() {

    if (intervalId !== null) return;


    intervalId = setInterval(() => {
        if (text.innerHTML === "ĐĂNG NHẬP TẠI ĐÂY") {
            text.innerHTML = "ĐĂNG NHẬP THẤT BẠI";
            text.classList.remove("text");
            text.style.color = "yellow"

        } else {
            text.classList.add("text");
            text.innerHTML = "ĐĂNG NHẬP TẠI ĐÂY";
            text.style.color = ""


        }

        form.classList.toggle("MessageErr");
    }, 1500);


};

function startIntervalLogin() {

    if (intervalId !== null) return;


    intervalId = setInterval(() => {
        form.classList.toggle("MessageLogin");
    }, 1500);


};


function stopInterval() {
    clearInterval(intervalId);
    intervalId = null;
    form.classList.remove("MessageErr");
    text.innerHTML = "ĐĂNG NHẬP TẠI ĐÂY";
}

document.addEventListener("DOMContentLoaded", function () {
    if (err !== null) {
        startInterval();
    } else {
        startIntervalLogin();
    }
});


form.addEventListener("mouseover", function () {
    stopInterval();
    form.classList.remove("MessageErr", "MessageErrHide");
});

form.addEventListener("mouseout", function () {
    if (err !== null) {
        startInterval();
    } else {
        startIntervalLogin();
    }
});


form.onclick = () => {
    stopInterval();
    form.classList.remove("MessageErr", "MessageErrHide");
}