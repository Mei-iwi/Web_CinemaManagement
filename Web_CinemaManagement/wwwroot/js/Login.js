//Khu vực biến
var checkbox = document.getElementById("checkpass");

var pass = document.getElementById("Password");

var form = document.querySelector(".form-login");

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

