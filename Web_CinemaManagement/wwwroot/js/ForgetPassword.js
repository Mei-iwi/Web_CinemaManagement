//Khu vực biến

var sendmail = document.getElementById("sendmail");

var code = document.querySelector(".code");

var value = document.querySelectorAll(".authenticate input");

var User = document.getElementById("User");

var send = document.querySelector(".code button");


var confirmcode = "";

var checkpassNew = document.getElementById("checkpassNew");

var checkpassAgain = document.getElementById("checkpassAgain");

var pass = document.querySelectorAll(".changePass input[type='password']");





//Khu vực hàm

sendmail.addEventListener("click", function () {

    var list = document.querySelectorAll(".error");
    for (var i = 0; i < list.length; i++) {
        list[i].innerHTML = "";

    }

    sendmail.setAttribute("disabled", "true");

    sendmail.innerText = "Thông tin của bạn đang được xác thực";

    $.ajax({
        url: "/Authentication/getCode",
        type: "GET",
        data: { username: value[0].value, email: value[1].value },
        dataType: 'json',
        success: function (data) {

            if (data.success) {

                sendmail.innerText = "Xác thực thành công, vui lòng nhập mã xác thực";


                confirmcode = String(data.code);

                code.style.display = "block";

                sendmail.setAttribute("disabled", "true");

                User.value = value[0].value;

            }
            else {

                sendmail.innerText = "Xác thực tài khoản thất bại, xác thực lại tại đây!";

                sendmail.removeAttribute("disabled");

                var list = document.querySelectorAll(".error");
                for (var i = 0; i < list.length; i++) {
                    list[i].innerHTML = data.message;

                }
            }
        },
        error: function () {
            sendmail.innerText = "Xác thực tài khoản thất bại, xác thực lại tại đây!";

            sendmail.removeAttribute("disabled");
        }
    });
});

//

send.addEventListener("click", function () {

    var codeValue = String(document.querySelector("#code").value);

    $("#fail").text("");


    if (confirmcode === codeValue) {

        var authe = $(".authenticate");

        authe.fadeOut();

        authe.removeClass("d-flex");


        setTimeout(function () {
            authe.css("display", "none");

            var changePass = $(".changePass");

            changePass.fadeIn();

            changePass.css("display", "flex");



        }, 1500);

    }
    else {
        $("#fail").text("Sai mã xác thực, vui lòng kiểm tra lại");
    }

});

checkpassNew.addEventListener("change", function () {
    if (this.checked) {
        pass[0].type = "text";
    }
    else {
        pass[0].type = "password";

    }
});

checkpassAgain.addEventListener("change", function () {
    if (this.checked) {
        pass[1].type = "text";
    }
    else {
        pass[1].type = "password";

    }
});

var form = document.querySelector(".changePass form");

var newPass = document.querySelector("input[name='NewPass']");
var newPassAgain = document.querySelector("input[name='NewPassAgain']");

form.addEventListener("submit", function (e) {
    $("#diff").text("");
    if (newPass.value !== newPassAgain.value) {
        e.preventDefault();
        $("#diff").text("Mật khẩu nhập lại không chính xác");
    }
});
