
const key_API = "c16f384e1284410b829060cfc08e0e4f";

var url = `https://newsapi.org/v2/everything?q=phim&language=vi&apiKey=${key_API}`;


//var url = `https://newsapi.org/v2/everything?q=tesla&from=2025-10-13&sortBy=publishedAt&apiKey=c16f384e1284410b829060cfc08e0e4f`;

const container = document.getElementById("newsContainer");

fetch(url)
    .then(response => response.json())
    .then(data => {
        const top20 = data.articles
            .filter(a => a.title.includes("phim") || a.description?.includes("phim"))
            .slice(0, 20);
        top20.forEach(article => {
            const div = document.createElement("div");
            div.innerHTML = `
                <h3><a href="${article.url}" target="_blank">${article.title}</a></h3>
                <p><strong>Tác giả:</strong> ${article.author || "Không rõ"}</p>
                <p>${article.description || ""}</p>
                <img src="${article.urlToImage || ""}" width="200">
                <hr>
            `;
            container.appendChild(div);
        });
    })
    .catch(error => console.error("Lỗi", error))

