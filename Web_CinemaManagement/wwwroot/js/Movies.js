//Khu vực biến

var buttons = document.querySelectorAll("p button");

var tables = document.querySelectorAll(".col-md-12")

console.log(tables);

//Khu vực hàm

buttons.forEach((btn, index) => {

    btn.addEventListener("click", () => {
        tables.forEach(t => t.style.display = "none");
        tables[index].style.display = "block"

    });

});

document.addEventListener("DOMContentLoaded", function () {

    tables.forEach(t => t.style.display = "none");

    tables[0].style.display = "block";

});