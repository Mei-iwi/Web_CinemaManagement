//Khu vực biến

var info = $("#info");

var sendinfo = document.getElementById("sendinfo");

var passInput = $("#pass");

var regis = $("#regis");

var avatar = document.getElementById("avatar");

var avatarshow = document.querySelector(".chosseimage img")

var textbox = document.querySelectorAll(".form-group input[type='text']");

var mail = document.querySelectorAll(".form-group input[type='email']");

var date = document.querySelectorAll(".form-group input[type='date']");

var phone = document.querySelectorAll(".form-group input[type='tel']");

var gender = document.querySelector(".form-group select");

var checkpassNew = document.getElementById("checkpassNew");

var checkpassAgain = document.getElementById("checkpassAgain");

var pass = document.querySelectorAll(".form-group input[type='password']");

var user = document.getElementById("PHAI");


//Hàm xử lý


avatar.addEventListener("change", function () {
    if (this.files && this.files[0]) {
        const reader = new FileReader();
        reader.onload = e => {
            avatarshow.src = e.target.result;
        };
        reader.readAsDataURL(this.files[0]);
    }
});

user.addEventListener("change", function () {

    if (!avatar.files.length) {
        if (this.value === "Nam") {
            avatarshow.src = "/wwwroot/Images/DefautUserMale.png"
        }
        else {
            avatarshow.src = "/wwwroot/Images/DefautUserFemale.png"
        }
    }

});

function isValidEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

sendinfo.addEventListener("click", function () {

    var cnt = 0;

    mail[0].parentElement.querySelector("span").textContent = "";

    phone[0].parentElement.querySelector("span").textContent = "";

    date[0].parentElement.querySelector("span").textContent = "";


    for (var i = 0; i < 2; i++) {
        textbox[i].parentElement.querySelector("span").textContent = "";
    }

    if (!mail[0].value) {

        mail[0].parentElement.querySelector("span").textContent = "Enail không được để trống";

        cnt++;
    }
    else if (!isValidEmail(mail[0].value)) {
        mail[0].parentElement.querySelector("span").textContent = "Sai định dạng email";
        cnt++;

    }

    if (!date[0].value) {
        date[0].parentElement.querySelector("span").textContent = "Ngày sinh không được để trống";
        cnt++;
    }

    if (!phone[0].value) {

        phone[0].parentElement.querySelector("span").textContent = "Số điện thoại không được để trống";

        cnt++;
    }
    else if (phone[0].value.length < 10 || phone[0].value.length > 13) {
        phone[0].parentElement.querySelector("span").textContent = "Số điện thoại không hợp lệ";

        cnt++;
    }

    for (var i = 0; i < 2; i++) {

        if (!textbox[i].value) {
            textbox[i].parentElement.querySelector("span").textContent = "Trường này không được để trống";
        }
    }

    if (cnt == 0) {

        
        console.log(mail[0].value)

        $.ajax({
            url: "/Authentication/CodeGeneration",
            type: "GET",
            data: { name: textbox[0].value, date: date[0].value, gender: gender.value, email: mail[0].value, phone: phone[0].value, addr: textbox[1].value },
            dataType: 'json',
            success: function (data) {
                if (data.success) {

                    $("#UserName").val(data.newID);

                    setTimeout(() => {
                        info.fadeOut();
                    }, 1000)

                    setTimeout(function () {
                        passInput.fadeIn();

                        regis.css("display", "flex")

                        regis.fadeIn();


                    }, 1000);

                }
            },
            error: function () {
                console.log("Lỗi");
            }

        });

       
    }

    return;


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


function isValidPassword(password) {
    const regex = /^(?=.*[0-9])(?=.*[!@#$%^&*()_+\-=\[\]{}|;':",.<>\/?]).{8,}$/;
    return regex.test(password);
}


var form = document.querySelector("form");

var newPass = document.querySelector("input[name='NewPass']");

var newPassAgain = document.querySelector("input[name='NewPassAgain']");

form.addEventListener("submit", function (e) {
    $("#diff").text("");
    if (newPass.value !== newPassAgain.value) {
        e.preventDefault();
        $("#diff").text("Mật khẩu nhập lại không chính xác");
    }
    else if (!isValidPassword(newPass.value)) {
        e.preventDefault();
        $("#diff").text("Mật khẩu phải dài tối thiếu 8 kí tự, có chứa số và kí tự đặc biệt");
    }
});