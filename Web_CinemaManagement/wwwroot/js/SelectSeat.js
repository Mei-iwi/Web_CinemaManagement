const cinema = document.getElementById('cinema');
const selectedLabel = document.getElementById('selectedSeat');

// Cấu hình ghế
const seatsPerRow = [15, 10, 10, 10, 15]; // chỉ dùng để tính layout cong
const rowSpacing = 60;
const seatSize = 50;
const startY = 100;

const url = new URLSearchParams(window.location.search);


// Dữ liệu ghế
let seatData = [];

// Lấy ghế từ API
function FetchData(id = '') {
    fetch(`/api/seats/getseats?id=${id}`)
        .then(res => {
            if (!res.ok) return;
            return res.json();
        })
        .then(data => {
            seatData = []; // reset dữ liệu

            // Vẽ ghế theo dữ liệu API
            let apiIndex = 0;
            for (let row = 0; row < seatsPerRow.length; row++) {
                const seatsInRow = seatsPerRow[row];
                const totalWidth = seatsInRow * seatSize + (seatsInRow - 1) * 10;
                const centerX = cinema.offsetWidth / 2;
                const y = startY + row * rowSpacing;

                for (let i = 0; i < seatsInRow; i++) {
                    if (apiIndex >= data.length) break; // chỉ hiển thị số ghế API trả về

                    const rowOffset = (i - (seatsInRow - 1) / 2) * (seatSize + 10);
                    const curveOffset = Math.sin((i / seatsInRow - 0.5) * Math.PI) * 20; // cong nhẹ
                    const x = centerX + rowOffset + curveOffset;

                    const type = row === 1 ? 'vip' : row === 2 ? 'couple' : 'normal';
                    const booked = data[apiIndex].TRANGTHAI === "true";

                    seatData.push({
                        id: 'G' + data[apiIndex].MAGHE.substring(data[apiIndex].MAGHE.length - 2, data[apiIndex].MAGHE.length),
                        type,
                        x,
                        y,
                        booked
                    });

                    apiIndex++;
                }
            }

            renderSeats();
        })
        .catch(err => console.error(err));
}

// Render ghế
function renderSeats() {
    cinema.innerHTML = '';

    seatData.forEach(seat => {
        const div = document.createElement("div");
        div.style.position = "absolute";
        div.style.width = div.style.height = seatSize + "px";
        div.style.left = seat.x + "px";
        div.style.top = seat.y + "px";
        div.style.backgroundColor = seat.booked ? "gray" :
            seat.type === "vip" ? "gold" :
                seat.type === "couple" ? "pink" : "green";

        div.innerText = seat.id;

        // Class CSS
        div.classList.add("seat");
        if (seat.type !== "normal") div.classList.add(seat.type);
        if (seat.booked) div.classList.add("booked");

        // Click chọn ghế
        div.addEventListener('click', () => {
            if (seat.booked) return;
            document.querySelectorAll('.seat.selected').forEach(s => s.classList.remove('selected'));
            div.classList.add('selected');
            selectedLabel.textContent = seat.id;
            ghe = seat.id;

        });

        cinema.appendChild(div);
    });
}


var seat = document.getElementById("selectedSeat");


// Call API khi load

function getTime(id = '') {
    fetch(`/api/seats/getTime?id=${id}`)
        .then(res => {
            if (!res.ok) {
                return;
            }
            return res.json();
        })
        .then(data => {

            FetchData(data[0].MAPHONG);

            var chose = document.getElementById("chose");


            console.log(data)

            data.forEach(t => {
                const opt = document.createElement("option");
                opt.value = t.MAPHONG; // giá trị value
                opt.setAttribute("MS", t.MASUAT);
                opt.textContent = `${t.GIOBATDAU} - ${t.GIOKETTHUC} - ${t.NGAYCHIEU}`;
                chose.appendChild(opt);
            });

        })
        .catch(err => console.error(err));
}


document.addEventListener("DOMContentLoaded", function () {


    const id = url.get("id");

    console.log(id)

    getTime(id);

    Quatity();


});

var change = document.getElementById("chose");

change.addEventListener("change", function () {


    $("#rooms").text(this.value)

    FetchData(this.value);

});

var button = document.querySelectorAll(".p-3 button");

console.log(button)

button[0].addEventListener("click", function () {
    location.href = "/Home/Dashboard";
});

var slv = document.getElementById("lv");
function buyTicket() {

    if (change.value === 0) {
        alert("Vui lòng chọn khung giờ");
        return;
    }

    if (seat.textContent.substring(1) === "None") {
        alert("Vui lòng chọn ghế");
        return;
    }

    const selectedOption = change.options[change.selectedIndex]; // option đang chọn
    console.log(selectedOption.getAttribute("MS"));

    const ticket = {
        MASUAT: selectedOption.getAttribute("MS"),
        MAGHE: seat.textContent.substring(1),
        MAPHONG: change.value,
        LOAIVE: slv.value
    };

    console.log(ticket);

    fetch("/Seat/BuyTicket", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(ticket)
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                alert("Mua vé thành công! Mã vé: " + data.mave);
                selectedSeat = null;
                document.getElementById("selectedSeat").textContent = "None";
                location.reload()
            } else {
                alert("Lỗi: " + data.message);
            }
        })
        .catch(err => console.error("Lỗi fetch:", err));
}

button[1].addEventListener("click", () => {

    buyTicket();
})

function Quatity() {
    fetch("/api/seats/Quatity")
        .then(res => {
            if (!res.ok) {
                return;
            }
            return res.json();
        })
        .then(data => {
            const lv = document.getElementById("lv");

            data.forEach(t => {
                const opt = document.createElement("option");
                opt.value = t.MALV;
                opt.text = t.TENLV;
                lv.appendChild(opt);
            });

        })
        .catch(er => console.error(er));
}