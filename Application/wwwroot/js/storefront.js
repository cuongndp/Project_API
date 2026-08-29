const adressbtn=document.querySelector("#adres-form")
const adressclose=document.querySelector("#adress-close")
const bannerRightBtn = document.querySelector(".silder-content-left-top-btn .fa-chevron-right")
const bannerLeftBtn = document.querySelector(".silder-content-left-top-btn .fa-chevron-left")
const imgNumber=document.querySelectorAll(".silder-content-left-top img")
let index=0
if (adressbtn && adressclose) {
    adressbtn.addEventListener("click" ,function(){
        document.querySelector('.adres-form').style.display="flex"
    })
    adressclose.addEventListener("click" ,function(){
        document.querySelector('.adres-form').style.display="none"
    })
}
if (bannerRightBtn && bannerLeftBtn && imgNumber.length) bannerRightBtn.addEventListener("click",function(){
    index=index+1
    if(index>imgNumber.length-1){
        index=0
    }
    document.querySelector(".silder-content-left-top").style.right=index *100+"%"
})
if (bannerRightBtn && bannerLeftBtn && imgNumber.length) bannerLeftBtn.addEventListener("click",function(){
    index=index-1
    if(index<=0){
        index=imgNumber.length-1
    }
    document.querySelector(".silder-content-left-top").style.right=index *100+"%"
})
const imgactive=document.querySelectorAll(".active")

const imgNumberi=document.querySelectorAll(".silder-content-left-bottom li")
imgNumberi.forEach(function(image,index){
    image.addEventListener("click",function(){
        removeactive ()
        document.querySelector(".silder-content-left-top").style.right=index *100+"%"
        image.classList.add("active")
    })
})
function removeactive (){ 
    let imgactive=document.querySelector('.active')
    if (imgactive) imgactive.classList.remove("active")
}
function imgAuto () {
    index=index+1
    if(index>imgNumber.length-1){
        index=0
    }
    removeactive ()
    document.querySelector(".silder-content-left-top").style.right=index *100+"%"
    imgNumberi[index].classList.add("active")
}
if (imgNumber.length && imgNumberi.length) setInterval(imgAuto,2000)

const productLists = document.querySelectorAll("[data-product-list]")

function getProductValue(product, name) {
    return product[name] ?? product[name.charAt(0).toUpperCase() + name.slice(1)]
}

function formatPrice(value) {
    const price = Number(value)
    return Number.isFinite(price) ? `${price.toLocaleString("vi-VN")}đ` : "Liên hệ"
}

function createProductCard(product, cardClass) {
    const productName = getProductValue(product, "productName") || "Sản phẩm"
    const price = Number(getProductValue(product, "price"))
    const promotionPrice = Number(getProductValue(product, "promotionPrice"))
    const discount = price > promotionPrice && promotionPrice > 0
        ? `-${Math.round((1 - promotionPrice / price) * 100)}%`
        : ""
    const productId = getProductValue(product, "productID") ?? getProductValue(product, "productId") ?? 0

    const card = document.createElement("div")
    card.className = cardClass
    card.style.cursor = "pointer"
    card.addEventListener("click", () => {
        if (productId) window.location.href = `/Storefront/ProductDetails?id=${productId}`
    })

    const image = document.createElement("img")
    image.src = getProductValue(product, "mainImage") || "/images/branding/AnhDau.PNG"
    image.alt = productName
    card.appendChild(image)

    const text = document.createElement("div")
    text.className = `${cardClass}-text`
    const promotion = document.createElement("li")
    const promotionIcon = document.createElement("img")
    promotionIcon.src = "/images/icons/icon1-50x50.webp"
    promotionIcon.alt = ""
    promotion.appendChild(promotionIcon)
    const promotionText = document.createElement("p")
    promotionText.textContent = "Trợ giá mua sắm"
    promotion.appendChild(promotionText)
    text.appendChild(promotion)

    const name = document.createElement("li")
    name.textContent = productName
    text.appendChild(name)

    const category = document.createElement("li")
    category.textContent = getProductValue(product, "categoryName") || "Online giá tốt"
    text.appendChild(category)

    const oldPrice = document.createElement("li")
    const oldPriceLink = document.createElement("a")
    oldPriceLink.href = "#"
    oldPriceLink.textContent = formatPrice(price)
    oldPrice.appendChild(oldPriceLink)
    if (discount) {
        const discountText = document.createElement("span")
        discountText.textContent = discount
        oldPrice.appendChild(discountText)
    }
    text.appendChild(oldPrice)

    const currentPrice = document.createElement("li")
    currentPrice.textContent = formatPrice(promotionPrice)
    text.appendChild(currentPrice)

    const gift = document.createElement("li")
    gift.textContent = "Quà 500.000đ"
    text.appendChild(gift)

    const stars = document.createElement("li")
    for (let starIndex = 0; starIndex < 5; starIndex += 1) {
        const star = document.createElement("i")
        star.className = "fas fa-star"
        stars.appendChild(star)
    }
    text.appendChild(stars)
    card.appendChild(text)
    return card
}

function renderRelatedProducts(products) {
    const container = document.getElementById("related-products")
    if (!container) return

    const items = Array.isArray(products) ? products.slice(0, 4) : []
    if (!items.length) {
        container.innerHTML = "<br /><h3 style='color:#F00'>Các Dòng Sản Phẩm Khác</h3>"
        return
    }

    container.innerHTML = `
        <br />
        <h3 style='color:#F00'>Các Dòng Sản Phẩm Khác</h3>
        <table class='table'>
            <tr class='tr'>
                ${items.map(product => {
                    const productId = getProductValue(product, "productID") ?? getProductValue(product, "productId") ?? 0
                    const productName = getProductValue(product, "productName") || "Sản phẩm"
                    const price = Number(getProductValue(product, "promotionPrice") ?? getProductValue(product, "price") ?? 0)
                    const image = getProductValue(product, "mainImage") || "/images/branding/AnhDau.PNG"
                    return `
                        <td class='td'>
                            <div class='top'>
                                <a href='/Storefront/ProductDetails?id=${productId}'>
                                    <img src="${image}" class='img' alt="${productName}" />
                                </a><br />
                                <a href='/Storefront/ProductDetails?id=${productId}' class='buy'>Mua ngay</a>
                            </div>
                            <div class='info'>
                                <a href='/Storefront/ProductDetails?id=${productId}' class='text'>${productName}</a><br />
                                <h3>${formatPrice(price)}</h3>
                            </div>
                        </td>
                    `
                }).join("")}
            </tr>
        </table>
    `
}

function bindProductSliderArrows() {
    const productWrapper = document.querySelector(".slider-prodouct-one-content-contaner")
    if (!productWrapper) return

    const track = productWrapper.querySelector(".slider-prodouct-one-content-items")
    const leftArrow = productWrapper.querySelector(".slider-prodouct-one-content-btn .fa-chevron-left")
    const rightArrow = productWrapper.querySelector(".slider-prodouct-one-content-btn .fa-chevron-right")
    if (!track || !leftArrow || !rightArrow) return

    const step = Math.max(track.clientWidth * 0.8, 260)

    leftArrow.addEventListener("click", () => {
        track.scrollBy({ left: -step, behavior: "smooth" })
    })

    rightArrow.addEventListener("click", () => {
        track.scrollBy({ left: step, behavior: "smooth" })
    })
}

function renderProductDetail(product, products = []) {
    const breadcrumb = document.getElementById("detail-breadcrumb")
    const gallery = document.getElementById("product-gallery")
    const info = document.getElementById("product-detail-info")
    const description = document.getElementById("product-description")

    if (!product || !breadcrumb || !gallery || !info || !description) return

    const productId = getProductValue(product, "productID") ?? getProductValue(product, "productId") ?? 0
    const productName = getProductValue(product, "productName") || "Sản phẩm"
    const categoryName = getProductValue(product, "categoryName") || "Sản phẩm"
    const price = Number(getProductValue(product, "price") ?? 0)
    const promotionPrice = Number(getProductValue(product, "promotionPrice") ?? getProductValue(product, "price") ?? 0)
    const images = Array.isArray(product.images) ? product.images.filter(Boolean) : []
    const galleryImages = [getProductValue(product, "mainImage"), ...images].filter(Boolean).slice(0, 5)
    const attrs = Array.isArray(product.attributes) ? product.attributes : []
    const groupedAttrs = attrs.reduce((result, attr) => {
        const groupName = attr.groupName || "Thông số"
        if (!result[groupName]) result[groupName] = []
        result[groupName].push(attr)
        return result
    }, {})

    breadcrumb.textContent = productName

    gallery.innerHTML = `
        <div class='detail-gallery'>
            <div class='detail-gallery__main'>
                <img id='detail-main-image' src="${galleryImages[0] || "/images/branding/AnhDau.PNG"}" alt="${productName}" />
            </div>
            <div class='detail-gallery__thumbs'>
                ${galleryImages.map((image, index) => `
                    <button type='button' class='detail-gallery__thumb ${index === 0 ? "is-active" : ""}' data-detail-thumb="${index}" aria-label="Xem ảnh ${index + 1}">
                        <img src="${image}" alt="${productName} ${index + 1}" />
                    </button>
                `).join("")}
            </div>
        </div>
    `

    const mainImage = document.getElementById("detail-main-image")
    document.querySelectorAll("[data-detail-thumb]").forEach(button => {
        button.addEventListener("click", () => {
            const nextIndex = Number(button.dataset.detailThumb || 0)
            const selectedImage = galleryImages[nextIndex] || galleryImages[0]
            if (mainImage && selectedImage) mainImage.src = selectedImage
            document.querySelectorAll(".detail-gallery__thumb").forEach(item => item.classList.toggle("is-active", item === button))
        })
    })

    info.innerHTML = `
        <h2>${productName}</h2>
        <img src="/images/icons/5-stars.png" style='width:20%; float:left'/>
        <p style='color:#999;float:left'>(12 đánh giá)</p>
        <br /><br /><br />
        <h2 style='color:#900;float:left'>${formatPrice(promotionPrice || price)}</h2>
        ${price > promotionPrice ? `<span style="float:left; margin: 10px 0 0 10px; color:#888; text-decoration: line-through;">${formatPrice(price)}</span>` : ""}
        <br /><br /><br />
        <ul>
            <li class='li'>- Thương hiệu: ${getProductValue(product, "brandName") || "Đang cập nhật"}</li>
            <li class='li'>- Danh mục: ${categoryName}</li>
            <li class='li'>- Tình trạng: ${Number(getProductValue(product, "quantity") ?? 0) > 0 ? "Còn hàng" : "Hết hàng"}</li>
            <li class='li'>- Bảo hành: ${Number(getProductValue(product, "warranty") ?? 0)} tháng</li>
            ${attrs.slice(0, 5).map(attr => `<li class='li'>- ${attr.attributeName}: ${attr.attributeValue}</li>`).join("")}
        </ul>
        <br />
        <input type='submit' value='MUA NGAY' id='button' style="border-radius:5px; width: 200px" />
        <input type='submit' value='THÊM VÀO GIỎ HÀNG' id='button1' style="border-radius:5px; width: 200px"/>
        <br /><br /><br />
    `

    const detailHtml = [
        "<h2>MÔ TẢ SẢN PHẨM</h2>",
        `<p>${getProductValue(product, "description") || "Sản phẩm đang được cập nhật mô tả chi tiết."}</p>`,
        `<p>${getProductValue(product, "detail") || "Sản phẩm có chất lượng tốt, thiết kế hiện đại và phù hợp nhu cầu sử dụng."}</p>`
    ]

    if (Object.keys(groupedAttrs).length) {
        detailHtml.push("<br /><h3>Thông số kỹ thuật</h3><table class='table' style='width:100%; border-collapse:collapse;'><tbody>")
        Object.entries(groupedAttrs).forEach(([groupName, items]) => {
            detailHtml.push(`<tr><td colspan='2' style='background:#f3f3f3; font-weight:bold; padding:8px;'>${groupName}</td></tr>`)
            items.sort((a, b) => (a.sort ?? 0) - (b.sort ?? 0)).forEach(item => {
                detailHtml.push(`<tr><td style='padding:8px; border-bottom:1px solid #eee; width:40%;'>${item.attributeName}</td><td style='padding:8px; border-bottom:1px solid #eee;'>${item.attributeValue}</td></tr>`)
            })
        })
        detailHtml.push("</tbody></table>")
    }

    description.innerHTML = detailHtml.join("")
    renderRelatedProducts(products.filter(item => getProductValue(item, "productID") !== productId))
}

async function loadProductDetail() {
    const detailRoot = document.querySelector("[data-product-detail]")
    if (!detailRoot) return

    const params = new URLSearchParams(window.location.search)
    const productId = params.get("id") || "9"

    try {
        const [productResponse, listResponse] = await Promise.all([
            fetch(`/api/product/Get_Product_Detail?id=${productId}`),
            fetch("/api/product/Get_Product?id=0")
        ])

        if (!productResponse.ok) throw new Error(`Product detail API returned ${productResponse.status}`)
        const product = await productResponse.json()
        const products = listResponse.ok ? await listResponse.json() : []
        renderProductDetail(product, Array.isArray(products) ? products : [])
    } catch (error) {
        console.error("Không thể tải chi tiết sản phẩm:", error)
        const productName = "Sản phẩm"
        const breadcrumb = document.getElementById("detail-breadcrumb")
        if (breadcrumb) breadcrumb.textContent = productName
        const description = document.getElementById("product-description")
        if (description) description.innerHTML = "<p>Không thể tải thông tin sản phẩm. Vui lòng thử lại sau.</p>"
    }
}

async function loadHomepageProducts() {
    if (!productLists.length) return
    productLists.forEach(list => list.replaceChildren())

    try {
        const response = await fetch("/api/product/Get_Product?id=0")
        if (!response.ok) throw new Error(`Product API returned ${response.status}`)
        const products = await response.json()
        const hotProducts = Array.isArray(products)
            ? products.filter(product => getProductValue(product, "hot") === true || String(getProductValue(product, "hot")).toLowerCase() === "true")
            : []
        const isPhone = product => String(getProductValue(product, "categoryName") || "").trim().toLowerCase() === "điện thoại"
        const productsByList = {
            sale: hotProducts.filter(product => !isPhone(product)).slice(0, 10),
            phones: hotProducts.filter(isPhone).slice(0, 10)
        }

        productLists.forEach(list => {
            const listType = list.dataset.productList
            list.replaceChildren(...(productsByList[listType] || []).map(product => createProductCard(
                product,
                listType === "sale" ? "slider-prodouct-one-content-item" : "product-gallery-one-content-product-item"
            )))
        })

        bindProductSliderArrows()
    } catch (error) {
        console.error("Không thể tải danh sách sản phẩm:", error)
    }
}

loadHomepageProducts()
loadProductDetail()