//Khu vực biến

var filter = document.getElementById("Filter");

var button = document.getElementById("submit");

console.log(button)
//Khu vực hàm
document.addEventListener("DOMContentLoaded", function () {

    fetch(`/api/tickets/getTickets`)
        .then(res => {
            if (!res.ok) {
                alert("Lỗi kết nối")
            };
            return res.json();
        })
        .then(data => {
            console.log(data)
            const tbody = document.querySelector("#ticketTable tbody");

            tbody.innerHTML = "";

            var index = 1;

            data.forEach(ticket => {
                const tr = document.createElement("tr");
                tr.innerHTML = `
                    <td>${index++}</td>
                    <td>${ticket.mave}</td>
                    <td>${ticket.masuat}</td>
                    <td>${ticket.malv}</td>
                    <td>${ticket.makh}</td>
                    <td>${ticket.manv}</td>
                    <td>${ticket.maghe}</td>
                    <td>${ticket.ngaybanve}</td>
                    <td><a href="/Employee/TicketManagement/Details?id=${ticket.mave}">Chi tiết</a></td>
        `;
                tbody.appendChild(tr);


            });

        })
        .catch(err => console.error("Lỗi " + err))

});

filter.addEventListener("change", function () {


    fetch(`/api/tickets/getIn?time=${this.value}`)
        .then(res => {
            if (!res.ok) {
                alert("Lỗi kết nối");
            }
            return res.json();
        })
        .then(data => {

            const tbody = document.querySelector("#ticketTable tbody");

            tbody.innerHTML = "";

            var index = 1;
            data.forEach(ticket => {

                const tr = document.createElement("tr");

                tr.innerHTML = `
                    <td>${index++}</td>
                    <td>${ticket.mave}</td>
                    <td>${ticket.masuat}</td>
                    <td>${ticket.malv}</td>
                    <td>${ticket.makh}</td>
                    <td>${ticket.manv}</td>
                    <td>${ticket.maghe}</td>
                    <td>${ticket.ngaybanve}</td>
                    <td><a href="/Employee/TicketManagement/Details?id=${ticket.mave}">Chi tiết</a></td>
        `;

                tbody.appendChild(tr);

            });

        })
        .catch(err => console.error("Lỗi " + err));

});

button.addEventListener("click", function () {

    var text = document.getElementById("Search").value;

    console.log(text)
    
    fetch(`/api/tickets/Search?id=${text}`)
        .then(res => {
            if (!res.ok) {
                alert("Lỗi kết nối");
            }
            return res.json();
        })
        .then(data => {

            const tbody = document.querySelector("#ticketTable tbody");

            tbody.innerHTML = "";

            var index = 1;
            data.forEach(ticket => {

                const tr = document.createElement("tr");

                tr.innerHTML = `
                    <td>${index++}</td>
                    <td>${ticket.mave}</td>
                    <td>${ticket.masuat}</td>
                    <td>${ticket.malv}</td>
                    <td>${ticket.makh}</td>
                    <td>${ticket.manv}</td>
                    <td>${ticket.maghe}</td>
                    <td>${ticket.ngaybanve}</td>
                    <td><a href="/Employee/TicketManagement/Details?id=${ticket.mave}">Chi tiết</a></td>
        `;

                tbody.appendChild(tr);

            });

        })
        .catch(err => console.error("Lỗi " + err));
});