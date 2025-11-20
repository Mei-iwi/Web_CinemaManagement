const API_URL = 'https://691f0a0abb52a1db22c02e89.mockapi.io/cinemas';
const listContainer = document.getElementById("cinema-list-container");
const FALLBACK_IMAGE = "https://cdn-icons-png.flaticon.com/512/2809/2809590.png";

if (listContainer) {
    fetch(API_URL)
        .then(res => res.json())
        .then(data => {
            listContainer.innerHTML = '';

            data.forEach(cinema => {
                const item = document.createElement("div");

                // --- THAY ĐỔI Ở ĐÂY: Thêm bg-dark, text-white ---
                item.className = "card mb-4 border-secondary bg-dark text-white shadow cinema-card";
                item.style.cursor = "pointer";
                item.style.transition = "transform 0.2s, background-color 0.2s";

                // Hover: Nổi lên và đổi màu nền sáng hơn chút xíu
                item.onmouseover = () => {
                    item.style.transform = "translateY(-4px)";
                    item.classList.remove("bg-dark");
                    item.style.backgroundColor = "#2c3034"; // Màu xám đậm sáng hơn đen
                    item.classList.add("shadow-lg");
                };
                item.onmouseout = () => {
                    item.style.transform = "translateY(0)";
                    item.style.backgroundColor = ""; // Trả về mặc định
                    item.classList.add("bg-dark");
                    item.classList.remove("shadow-lg");
                };

                item.onclick = () => {
                    if (cinema.url) window.open(cinema.url, '_blank');
                    else alert("Rạp này chưa cập nhật link!");
                };

                item.innerHTML = `
                    <div class="card-body p-4">
                        <div class="d-flex align-items-center">
                            
                            <div class="flex-shrink-0 me-4"> 
                                <img src="${cinema.image}" 
                                     onerror="this.src='${FALLBACK_IMAGE}'"
                                     alt="${cinema.name}"
                                     class="rounded-circle border border-secondary p-1"
                                     style="width: 80px; height: 80px; object-fit: contain; background: #fff;">
                            </div>

                            <div class="flex-grow-1">
                                <h4 class="fw-bold text-white mb-2">${cinema.name}</h4>
                                
                                <p class="text-white-50 mb-0" style="font-size: 1rem;">
                                    <i class="bi bi-geo-alt-fill text-danger me-2"></i>${cinema.address}
                                </p>
                            </div>
                            
                            <div class="text-white-50 opacity-50 ms-3">
                                <i class="bi bi-chevron-right fs-3"></i>
                            </div>
                        </div>
                    </div>
                `;
                listContainer.appendChild(item);
            });
        })
        .catch(err => {
            console.error(err);
            listContainer.innerHTML = `<div class="alert alert-danger text-center">Lỗi tải dữ liệu</div>`;
        });
}