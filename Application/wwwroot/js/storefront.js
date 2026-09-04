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

async function submitLogin(event) {
    event.preventDefault()
    const form = event.currentTarget
    const submitButton = form.querySelector("[data-login-submit]")
    const message = form.querySelector("[data-login-message]")
    const email = form.elements.email.value.trim()
    const password = form.elements.password.value

    if (!email || !password) return

    submitButton.disabled = true
    submitButton.value = "ĐANG ĐĂNG NHẬP..."
    message.textContent = ""

    try {
        const response = await fetch("/api/home/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ Email: email, Password: password })
        })
        const result = await response.json().catch(() => null)
        const success = result?.seccess ?? result?.Seccess

        if (!response.ok || !success || !(result?.accessToken || result?.AccessToken)) {
            throw new Error(result?.message || result?.Message || "Email hoặc mật khẩu không đúng.")
        }

        const accessToken = result.accessToken || result.AccessToken
        saveAccessToken(accessToken)
        saveUserName(result.userName || result.UserName)
        updateAuthUser()

        message.textContent = result.message || result.Message || "Đăng nhập thành công."
        window.location.href = document.referrer && new URL(document.referrer).origin === window.location.origin
            ? document.referrer
            : "/Storefront"
    } catch (error) {
        message.textContent = error.message || "Không thể đăng nhập. Vui lòng thử lại."
    } finally {
        submitButton.disabled = false
        submitButton.value = "Đăng Nhập"
    }
}

const loginForm = document.querySelector("[data-login-form]")
if (loginForm) loginForm.addEventListener("submit", submitLogin)

async function submitRegister(event) {
    event.preventDefault()
    const form = event.currentTarget
    const submitButton = form.querySelector("[data-register-submit]")
    const message = form.querySelector("[data-register-message]")
    const password = form.elements.password.value
    const confirmPassword = form.elements.confirmPassword.value

    if (password !== confirmPassword) {
        message.textContent = "Mật khẩu xác nhận không khớp."
        return
    }

    submitButton.disabled = true
    submitButton.value = "ĐANG TẠO TÀI KHOẢN..."
    message.textContent = ""

    try {
        const response = await fetch("/api/home/Register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                Email: form.elements.email.value.trim(),
                Password: password,
                Name: form.elements.name.value.trim(),
                FullName: form.elements.fullName.value.trim(),
                Address: form.elements.address.value.trim()
            })
        })
        const result = await response.json().catch(() => null)
        const success = result?.seccess ?? result?.Seccess
        const responseMessage = result?.message || result?.Message
        if (!response.ok || !success) throw new Error(responseMessage || "Không thể tạo tài khoản.")

        message.textContent = responseMessage || "Đăng ký thành công."
        form.reset()
        setTimeout(() => { window.location.href = "/Storefront/Login" }, 700)
    } catch (error) {
        message.textContent = error.message || "Không thể đăng ký. Vui lòng thử lại."
    } finally {
        submitButton.disabled = false
        submitButton.value = "Đăng Kí"
    }
}

const registerForm = document.querySelector("[data-register-form]")
if (registerForm) registerForm.addEventListener("submit", submitRegister)

function getProductValue(product, name) {
    return product[name] ?? product[name.charAt(0).toUpperCase() + name.slice(1)]
}

function formatPrice(value) {
    const price = Number(value)
    return Number.isFinite(price) ? `${price.toLocaleString("vi-VN")}đ` : "Liên hệ"
}

function getPriceAfterVat(product, price) {
    const vat = Number(getProductValue(product, "vat") ?? 0)
    const numericPrice = Number(price)
    return Number.isFinite(numericPrice) && Number.isFinite(vat)
    ? Math.max(numericPrice * (1 + vat / 100), 0)
        : numericPrice
}

function getVatLabel(product) {
    const vat = Number(getProductValue(product, "vat") ?? 0)
    return Number.isFinite(vat) && vat > 0 ? `VAT: ${vat}%` : ""
}

function getVatAmount(product, price) {
    const vat = Number(getProductValue(product, "vat") ?? 0)
    const numericPrice = Number(price)
    return Number.isFinite(numericPrice) && Number.isFinite(vat)
        ? numericPrice * vat / 100
        : 0
}

function createProductCard(product, cardClass) {
    const productName = getProductValue(product, "productName") || "Sản phẩm"
    const rawPrice = Number(getProductValue(product, "price"))
    const price = getPriceAfterVat(product, rawPrice)
    const rawPromotionPrice = Number(getProductValue(product, "promotionPrice"))
    const promotionPrice = getPriceAfterVat(product, rawPromotionPrice)
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
    text.appendChild(oldPrice)

    const currentPrice = document.createElement("li")
    currentPrice.textContent = formatPrice(promotionPrice)
    text.appendChild(currentPrice)

    const vat = document.createElement("li")
    vat.textContent = getVatLabel(product)
    if (vat.textContent) text.appendChild(vat)

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
                    const rawPrice = getProductValue(product, "promotionPrice") ?? getProductValue(product, "price") ?? 0
                    const price = getPriceAfterVat(product, rawPrice)
                    const image = getProductValue(product, "mainImage") || "/images/branding/AnhDau.PNG"
                    const vat = getVatLabel(product)
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
                                ${vat ? `<small>${vat}</small>` : ""}
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

function getAccessToken() {
    return localStorage.getItem("accessToken")
        || localStorage.getItem("AccessToken")
        || sessionStorage.getItem("accessToken")
        || sessionStorage.getItem("AccessToken")
}

function getUserNameFromToken(accessToken) {
    try {
        const payload = JSON.parse(atob(accessToken.split(".")[1].replace(/-/g, "+").replace(/_/g, "/")))
        return payload.name || payload.unique_name || payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
    } catch {
        return null
    }
}

function saveUserName(userName) {
    if (userName) localStorage.setItem("userName", userName)
}

function updateAuthUser() {
    const authUser = document.querySelector("[data-auth-user]")
    if (!authUser) return

    const userName = localStorage.getItem("userName") || getUserNameFromToken(getAccessToken() || "")
    if (userName) {
        authUser.textContent = `Xin chào, ${userName}`
        authUser.href = "/Storefront"
    } else {
        authUser.textContent = "ĐĂNG NHẬP"
    }
}

let refreshTokenRequest = null

function isAccessTokenExpired(accessToken) {
    try {
        const payload = JSON.parse(atob(accessToken.split(".")[1].replace(/-/g, "+").replace(/_/g, "/")))
        return !payload.exp || payload.exp * 1000 <= Date.now()
    } catch {
        return true
    }
}

function saveAccessToken(accessToken) {
    localStorage.setItem("accessToken", accessToken)
    localStorage.removeItem("AccessToken")
    sessionStorage.removeItem("accessToken")
    sessionStorage.removeItem("AccessToken")
}

function clearAccessTokenAndRedirect() {
    localStorage.removeItem("accessToken")
    localStorage.removeItem("AccessToken")
    sessionStorage.removeItem("accessToken")
    sessionStorage.removeItem("AccessToken")
    localStorage.removeItem("userName")
    window.location.replace("/Storefront/Login")
}

function isAuthenticationError(error) {
    return error?.isAuthenticationError === true
}

async function refreshAccessToken() {
    if (refreshTokenRequest) return refreshTokenRequest

    refreshTokenRequest = fetch("/api/home/Refresh_Token", {
        method: "POST",
        credentials: "include"
    }).then(async response => {
        const responseText = await response.text()
        let refreshedToken = responseText.trim()
        try {
            const jsonValue = JSON.parse(refreshedToken)
            if (typeof jsonValue === "string") refreshedToken = jsonValue
        } catch {
            // The refresh API currently returns the JWT as text/plain.
        }
        if (!response.ok || typeof refreshedToken !== "string" || !refreshedToken) {
            const error = new Error("Không thể cấp lại phiên đăng nhập.")
            error.isAuthenticationError = true
            throw error
        }

        saveAccessToken(refreshedToken)
        return refreshedToken
    }).finally(() => {
        refreshTokenRequest = null
    })

    return refreshTokenRequest
}

async function authenticatedFetch(url, options = {}) {
    const requestOptions = { ...options, credentials: "include" }
    const headers = new Headers(requestOptions.headers || {})
    let accessToken
    try {
        accessToken = await ensureAccessToken()
    } catch (error) {
        clearAccessTokenAndRedirect()
        throw error
    }
    headers.set("Authorization", `Bearer ${accessToken}`)
    requestOptions.headers = headers

    const response = await fetch(url, requestOptions)
    if (response.status !== 401) return response

    try {
        const refreshedToken = await refreshAccessToken()
        const retryHeaders = new Headers(requestOptions.headers)
        retryHeaders.set("Authorization", `Bearer ${refreshedToken}`)
        const retryResponse = await fetch(url, { ...requestOptions, headers: retryHeaders })
        if (retryResponse.status === 401) {
            const error = new Error("Phiên đăng nhập không hợp lệ.")
            error.isAuthenticationError = true
            clearAccessTokenAndRedirect()
            throw error
        }
        return retryResponse
    } catch (error) {
        clearAccessTokenAndRedirect()
        error.isAuthenticationError = true
        throw error
    }
}

async function addProductToCart(productId, button) {
    const quantityInput = document.getElementById("detail-quantity")
    const quantity = Number(quantityInput?.value)
    const maxQuantity = Number(quantityInput?.max || 0)
    if (!Number.isInteger(quantity) || quantity < 1 || (maxQuantity > 0 && quantity > maxQuantity)) {
        alert("Vui lòng chọn số lượng hợp lệ.")
        return
    }

    const originalText = button.value
    button.disabled = true
    button.value = "ĐANG THÊM..."

    try {
        const response = await authenticatedFetch("/api/product/Insert_CartItem", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                ProductID: Number(productId),
                Quantity: quantity
            })
        })
        const result = await response.json().catch(() => null)

        const success = result?.seccess ?? result?.Seccess
        const message = result?.message || result?.Message
        if (!response.ok || !success) {
            throw new Error(message || `Không thể thêm sản phẩm vào giỏ hàng (HTTP ${response.status}).`)
        }

        button.value = "ĐÃ THÊM VÀO GIỎ"
        loadCartCount()
        alert(message || "Sản phẩm đã được thêm vào giỏ hàng.")
    } catch (error) {
        if (isAuthenticationError(error)) return
        alert(error.message || "Không thể thêm sản phẩm vào giỏ hàng.")
        button.value = originalText
    } finally {
        button.disabled = false
    }
}

async function loadCartCount() {
    const countElements = document.querySelectorAll("[data-cart-count]")
    if (!countElements.length) return

    try {
        const response = await authenticatedFetch("/api/product/GetCartItem")
        if (!response.ok) return
        const items = await response.json()
        const count = Array.isArray(items) ? items.length : 0
        countElements.forEach(element => { element.textContent = count })
    } catch {
        // Cart count is optional on public pages.
    }
}

async function loadCartPage() {
    const cartRoot = document.querySelector("[data-cart-page]")
    if (!cartRoot) return

    const list = cartRoot.querySelector("[data-cart-list]")
    const totalElement = cartRoot.querySelector("[data-cart-total]")
    try {
        const response = await authenticatedFetch("/api/product/GetCartItem")
        if (!response.ok) throw new Error("Không thể tải giỏ hàng.")
        const items = await response.json()
        const cartItems = Array.isArray(items) ? items : []

        if (!cartItems.length) {
            list.innerHTML = "<p>Giỏ hàng đang trống.</p>"
            totalElement.textContent = formatPrice(0)
            return
        }

        list.innerHTML = cartItems.map(item => {
            const cartItemId = item.cartItemID ?? item.CartItemID
            const name = item.productName ?? item.ProductName ?? "Sản phẩm"
            const quantity = Number(item.quantity ?? item.Quantity ?? 0)
            const rawPrice = item.promotionPrice ?? item.PromotionPrice ?? item.price ?? item.Price ?? 0
            const price = getPriceAfterVat(item, rawPrice)
            const vatRate = Number(item.vat ?? item.VAT ?? 0)
            const vatAmount = getVatAmount(item, rawPrice)
            const vat = getVatLabel(item)
            const image = item.image ?? item.Image ?? "/images/branding/AnhDau.PNG"
            return `
                <article class="cart-item" data-cart-item data-price="${price}" data-vat="${vatAmount}" data-vat-rate="${vatRate}" data-quantity="${quantity}">
                    <input type="checkbox" class="cart-item__check" data-cart-check checked />
                    <img src="${image}" alt="${name}" />
                    <div class="cart-item__info">
                        <h2>${name}</h2>
                        <p>Đơn giá: ${formatPrice(price)}</p>
                        ${vat ? `<p data-cart-item-vat>${vat} (${formatPrice(vatAmount * quantity)})</p>` : ""}
                        <div class="cart-item__quantity">
                            <button type="button" data-quantity-change="-1">−</button>
                            <span data-cart-quantity>${quantity}</span>
                            <button type="button" data-quantity-change="1">+</button>
                        </div>
                    </div>
                    <strong data-cart-item-total>${formatPrice(price * quantity)}</strong>
                    <input type="hidden" data-cart-item-id value="${cartItemId}" />
                </article>
            `
        }).join("")
        bindCartControls(cartRoot)
        updateCartSummary(cartRoot)
    } catch (error) {
        list.innerHTML = `<p>${error.message || "Không thể tải giỏ hàng."}</p>`
    }
}

function updateCartSummary(cartRoot) {
    let total = 0
    let selectedCount = 0
    cartRoot.querySelectorAll("[data-cart-item]").forEach(item => {
        const quantity = Number(item.dataset.quantity || 0)
        const price = Number(item.dataset.price || 0)
        const selected = item.querySelector("[data-cart-check]").checked
        if (selected) {
            total += price * quantity
            selectedCount += quantity
        }
        item.querySelector("[data-cart-quantity]").textContent = quantity
        item.querySelector("[data-cart-item-total]").textContent = formatPrice(price * quantity)
        const vatElement = item.querySelector("[data-cart-item-vat]")
        if (vatElement) vatElement.textContent = `VAT: ${item.dataset.vatRate}% (${formatPrice(Number(item.dataset.vat || 0) * quantity)})`
    })
    cartRoot.querySelector("[data-cart-total]").textContent = formatPrice(total)
    cartRoot.querySelector("[data-cart-selected]").textContent = `${selectedCount} sản phẩm được chọn`
    cartRoot.querySelector("[data-cart-select-all]").checked = [...cartRoot.querySelectorAll("[data-cart-check]")].every(check => check.checked)
}

function bindCartControls(cartRoot) {
    cartRoot.addEventListener("click", event => {
        const changeButton = event.target.closest("[data-quantity-change]")
        if (!changeButton) return
        const item = changeButton.closest("[data-cart-item]")
        const nextQuantity = Number(item.dataset.quantity) + Number(changeButton.dataset.quantityChange)
        if (nextQuantity < 1) return
        item.dataset.quantity = nextQuantity
        updateCartSummary(cartRoot)
    })
    cartRoot.addEventListener("change", event => {
        if (event.target.matches("[data-cart-select-all]")) {
            cartRoot.querySelectorAll("[data-cart-check]").forEach(check => { check.checked = event.target.checked })
        }
        if (event.target.matches("[data-cart-check], [data-cart-select-all]")) updateCartSummary(cartRoot)
    })
    cartRoot.querySelector("[data-cart-checkout]").addEventListener("click", async event => {
        const checkoutButton = event.currentTarget
        const cartItemIds = [...cartRoot.querySelectorAll("[data-cart-item]")]
            .filter(item => item.querySelector("[data-cart-check]").checked)
            .map(item => Number(item.querySelector("[data-cart-item-id]").value))
            .filter(Number.isInteger)
        if (!cartItemIds.length) {
            alert("Vui lòng chọn ít nhất một sản phẩm.")
            return
        }

        checkoutButton.disabled = true
        checkoutButton.textContent = "ĐANG TẢI..."
        try {
            const response = await authenticatedFetch("/api/payment/CheckoutCart", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ CartItemIDs: cartItemIds })
            })
            const preview = await response.json().catch(() => null)
            if (!response.ok || !preview) {
                throw new Error(`Không thể chuẩn bị thanh toán (HTTP ${response.status}).`)
            }
            sessionStorage.setItem("checkoutPreview", JSON.stringify(preview))
            window.location.href = "/Storefront/Checkout"
        } catch (error) {
            if (isAuthenticationError(error)) return
            alert(error.message || "Không thể chuẩn bị thanh toán.")
            checkoutButton.disabled = false
            checkoutButton.textContent = "Mua hàng"
        }
    })
}

async function loadCheckoutPage() {
    const checkoutRoot = document.querySelector("[data-checkout-page]")
    if (!checkoutRoot) return

    const list = checkoutRoot.querySelector("[data-checkout-list]")
    const totalElement = checkoutRoot.querySelector("[data-checkout-total]")
    const checkoutPreview = JSON.parse(sessionStorage.getItem("checkoutPreview") || "null")
    initializeCheckoutAddresses(checkoutRoot)
    const previewItems = checkoutPreview?.items || checkoutPreview?.Items || []
    if (previewItems.length) {
        list.innerHTML = previewItems.map(item => `
            <article class="cart-item">
                <img src="${item.image ?? item.Image ?? "/images/branding/AnhDau.PNG"}" alt="${item.productName ?? item.ProductName ?? "Sản phẩm"}" />
                <div class="cart-item__info">
                    <h2>${item.productName ?? item.ProductName ?? "Sản phẩm"}</h2>
                    <p>Số lượng: ${item.quantity ?? item.Quantity ?? 0}</p>
                    <p>Đơn giá: ${formatPrice(item.unitPrice ?? item.UnitPrice ?? 0)}</p>
                </div>
                <strong>${formatPrice(item.totalPrice ?? item.TotalPrice ?? 0)}</strong>
            </article>
        `).join("")
        totalElement.textContent = formatPrice(checkoutPreview.totalAmount ?? checkoutPreview.TotalAmount ?? 0)
        checkoutRoot.querySelector("[data-place-order]").addEventListener("click", () => {
            alert("Chưa thể đặt hàng vì backend chưa có API tạo đơn hàng và thanh toán.")
        })
        return
    }
    if (!checkoutPreview || !previewItems.length) {
        list.innerHTML = "<p>Chưa có sản phẩm được chọn. <a href='/Storefront/Cart'>Quay lại giỏ hàng</a></p>"
        return
    }
}

function initializeCheckoutAddresses(checkoutRoot) {
    const modal = document.querySelector("[data-address-modal]")
    const list = modal?.querySelector("[data-address-list]")
    const selectedAddress = checkoutRoot.querySelector("[data-selected-address]")
    if (!modal || !list || !selectedAddress) return

    const message = modal.querySelector("[data-address-message]")
    const form = modal.querySelector("[data-address-form]")
    const showAddressModal = () => { modal.hidden = false }
    const hideAddressModal = () => { modal.hidden = true }
    const renderAddresses = addresses => {
        list.innerHTML = addresses.length ? addresses.map((address, index) => {
            const id = address.shippingAddressID ?? address.ShippingAddressID ?? index
            const name = address.recipientName ?? address.RecipientName ?? ""
            const phone = address.recipientPhone ?? address.RecipientPhone ?? ""
            const value = address.address ?? address.Address ?? ""
            return `<label class="address-option"><input type="radio" name="checkoutAddress" value="${id}" data-address-name="${name}" data-address-phone="${phone}" data-address-value="${value}" /><span><strong>${name} - ${phone}</strong><span>${value}</span></span></label>`
        }).join("") : ""
    }
    const loadAddresses = async () => {
        try {
            const response = await authenticatedFetch("/api/payment/Get_ShippingAddress")
            if (!response.ok) throw new Error("Không thể tải địa chỉ nhận hàng.")
            renderAddresses(await response.json())
        } catch (error) {
            list.innerHTML = `<p>${error.message || "Không thể tải địa chỉ."}</p>`
        }
    }

    checkoutRoot.querySelector("[data-open-addresses]").addEventListener("click", showAddressModal)
    modal.querySelectorAll("[data-close-addresses]").forEach(button => button.addEventListener("click", hideAddressModal))
    modal.querySelector("[data-toggle-address-form]").addEventListener("click", () => { form.hidden = !form.hidden })
    list.addEventListener("change", event => {
        const option = event.target.closest("input[name='checkoutAddress']")
        if (!option) return
        selectedAddress.textContent = `${option.dataset.addressName} - ${option.dataset.addressPhone}, ${option.dataset.addressValue}`
        hideAddressModal()
    })
    form.addEventListener("submit", async event => {
        event.preventDefault()
        message.textContent = ""
        const submitButton = form.querySelector("button[type='submit']")
        submitButton.disabled = true
        try {
            const response = await authenticatedFetch("/api/payment/Insert_ShippingAddress", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    RecipientName: form.elements.recipientName.value.trim(),
                    RecipientPhone: form.elements.recipientPhone.value.trim(),
                    Address: form.elements.address.value.trim()
                })
            })
            if (!response.ok) throw new Error("Không thể thêm địa chỉ.")
            form.reset()
            form.hidden = true
            await loadAddresses()
            message.textContent = "Thêm địa chỉ thành công."
        } catch (error) {
            if (isAuthenticationError(error)) return
            message.textContent = error.message || "Không thể thêm địa chỉ."
        } finally {
            submitButton.disabled = false
        }
    })

    loadAddresses()
}

async function ensureAccessToken() {
    const accessToken = getAccessToken()
    if (accessToken && !isAccessTokenExpired(accessToken)) return accessToken

    return refreshAccessToken()
}

async function initializeAuthentication() {
    const accessToken = getAccessToken()
    if (accessToken && !isAccessTokenExpired(accessToken)) {
        updateAuthUser()
        return
    }

    try {
        const refreshedToken = await refreshAccessToken()
        const userName = localStorage.getItem("userName") || getUserNameFromToken(refreshedToken)
        saveUserName(userName)
        updateAuthUser()
    } catch {
        // Trang public vẫn truy cập được khi chưa đăng nhập.
    }
}

initializeAuthentication()
loadCartCount()
loadCartPage()
loadCheckoutPage()

function renderProductDetail(product, products = []) {
    const breadcrumb = document.getElementById("detail-breadcrumb")
    const gallery = document.getElementById("product-gallery")
    const info = document.getElementById("product-detail-info")
    const description = document.getElementById("product-description")

    if (!product || !breadcrumb || !gallery || !info || !description) return

    const productId = getProductValue(product, "productID") ?? getProductValue(product, "productId") ?? 0
    const productName = getProductValue(product, "productName") || "Sản phẩm"
    const categoryName = getProductValue(product, "categoryName") || "Sản phẩm"
    const stockQuantity = Number(getProductValue(product, "quantity") ?? 0)
    const rawPrice = getProductValue(product, "price") ?? 0
    const price = getPriceAfterVat(product, rawPrice)
    const rawPromotionPrice = getProductValue(product, "promotionPrice") ?? getProductValue(product, "price") ?? 0
    const promotionPrice = getPriceAfterVat(product, rawPromotionPrice)
    const vatLabel = getVatLabel(product)
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
        ${vatLabel ? `<p style="clear:both; color:#777; margin:8px 0 0;">${vatLabel}</p>` : ""}
        <br /><br /><br />
        <ul>
            <li class='li'>- Thương hiệu: ${getProductValue(product, "brandName") || "Đang cập nhật"}</li>
            <li class='li'>- Danh mục: ${categoryName}</li>
            <li class='li'>- Tình trạng: ${Number(getProductValue(product, "quantity") ?? 0) > 0 ? "Còn hàng" : "Hết hàng"}</li>
            <li class='li'>- Bảo hành: ${Number(getProductValue(product, "warranty") ?? 0)} tháng</li>
            ${attrs.slice(0, 5).map(attr => `<li class='li'>- ${attr.attributeName}: ${attr.attributeValue}</li>`).join("")}
        </ul>
        <br />
        <label for='detail-quantity'>Số lượng:</label>
        <input type='number' id='detail-quantity' min='1' max='${Math.max(stockQuantity, 1)}' value='1' style='width:70px; margin:0 10px 0 5px;' ${stockQuantity <= 0 ? "disabled" : ""} />
        <br /><br />
        <input type='button' value='MUA NGAY' id='button' style="border-radius:5px; width: 200px" />
        <input type='button' value='THÊM VÀO GIỎ HÀNG' id='button1' style="border-radius:5px; width: 200px" ${stockQuantity <= 0 ? "disabled" : ""}/>
        <br /><br /><br />
    `

    const addToCartButton = document.getElementById("button1")
    if (addToCartButton && productId) {
        addToCartButton.type = "button"
        addToCartButton.addEventListener("click", () => addProductToCart(productId, addToCartButton))
    }

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