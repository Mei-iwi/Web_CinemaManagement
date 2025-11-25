const API_URL = "https://api.npoint.io/8572c299fc8a884cdb3b";
const container = document.getElementById("blog-container");

fetch(API_URL)
    .then(response => response.json())
    .then(data => {
        if (container) {
            container.innerHTML = "";
            data.forEach(item => {
                const html = `
                <div class="col-md-6 col-lg-3 d-flex align-items-stretch">
                    <div class="card w-100 shadow-sm movie-tip-card h-100" style="overflow: hidden;">
                        
                        <a href="${item.url}" target="_blank" class="tip-image-link overflow-hidden">
                            <img src="${item.image}" class="card-img-top" alt="${item.title}" 
                                 style="height: 200px; object-fit: cover; transition: transform 0.3s;">
                        </a>

                        <div class="card-body d-flex flex-column" style="color: #fff;">
                            <h5 class="card-title mb-3" style="font-size: 16px; line-height: 1.4;">
                                <a href="${item.url}" target="_blank" class="tip-title-link" style="text-decoration: none; transition: color 0.3s;">
                                    ${item.title}
                                </a>
                            </h5>
                            
                            <p class="small text-muted mb-2">
                                <i class="fa fa-user-circle-o me-1"></i> Nguồn: ${item.author}
                            </p>
                            
                            <p class="card-text mb-4" style="font-size: 14px; color: #ccc;">${item.desc}</p>
                            
                            <a href="${item.url}" target="_blank" class="btn btn-custom-green w-100 mt-auto fw-bold">
                                Xem chi tiết
                            </a>
                        </div>
                    </div>
                </div>
            `;
                container.insertAdjacentHTML('beforeend', html);
            });
        }
    })
    .catch(error => console.error("Lỗi:", error));