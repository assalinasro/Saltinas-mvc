// =========================================================
// SALTINAS — site.js
// Carrito de compras (se guarda en localStorage para que
// no se pierda al cambiar de página del sitio MVC)
// =========================================================
(() => {
    "use strict";

    const CLAVE_STORAGE = "saltinas_carrito";

    const elementoModal = document.getElementById("carritoModal");
    const listaCarrito = document.getElementById("lista-carrito");
    const totalCarrito = document.getElementById("total-carrito");
    const contadorCarrito = document.getElementById("contador-carrito");
    const btnPagar = document.getElementById("btn-pagar");
    const elementoToast = document.getElementById("toast-carrito");
    const textoToast = document.getElementById("toast-texto");

    if (!elementoModal || !listaCarrito) return;

    const NUMERO_WHATSAPP = elementoModal.dataset.whatsapp;
    const URL_CATALOGO = elementoModal.dataset.urlCatalogo;

    let carrito = cargarCarrito();

    // ---------- Almacenamiento ----------
    function cargarCarrito() {
        try {
            const datos = JSON.parse(localStorage.getItem(CLAVE_STORAGE));
            return Array.isArray(datos) ? datos : [];
        } catch {
            return [];
        }
    }

    function guardarCarrito() {
        try {
            localStorage.setItem(CLAVE_STORAGE, JSON.stringify(carrito));
        } catch {
            /* el navegador puede bloquear el almacenamiento: el carrito sigue funcionando en memoria */
        }
    }

    // ---------- Utilidades ----------
    function formatearPrecio(valor) {
        return "$" + Number(valor).toLocaleString("es-CO");
    }

    function escaparHtml(texto) {
        const div = document.createElement("div");
        div.textContent = texto;
        return div.innerHTML;
    }

    function calcularTotal() {
        return carrito.reduce((suma, item) => suma + item.precio * item.cantidad, 0);
    }

    function mostrarAviso(mensaje) {
        if (!elementoToast || !textoToast) return;
        textoToast.textContent = mensaje;
        bootstrap.Toast.getOrCreateInstance(elementoToast, { delay: 1800 }).show();
    }

    // ---------- Pintar el carrito ----------
    function actualizarCarrito() {
        const totalItems = carrito.reduce((suma, item) => suma + item.cantidad, 0);

        if (totalItems > 0) {
            contadorCarrito.textContent = totalItems;
            contadorCarrito.classList.remove("d-none");
        } else {
            contadorCarrito.classList.add("d-none");
        }

        if (carrito.length === 0) {
            listaCarrito.innerHTML = `
                <div class="carrito-vacio text-center">
                    <p class="text-muted mb-3">Tu carrito está vacío.</p>
                    <button type="button" class="btn btn-seguir-comprando">Seguir comprando 🛍️</button>
                </div>`;
            btnPagar.disabled = true;
        } else {
            listaCarrito.innerHTML = carrito.map(item => `
                <div class="item-carrito">
                    <span class="detalle-item-carrito">
                        <img src="${escaparHtml(item.imagen)}" alt="${escaparHtml(item.nombre)}" class="mini-foto-carrito">
                        <span>
                            ${escaparHtml(item.nombre)}
                            <span class="cantidad-control">
                                <button type="button" class="btn-cantidad" data-accion="menos" data-id="${item.id}" aria-label="Quitar una unidad">−</button>
                                <span class="cantidad-valor">${item.cantidad}</span>
                                <button type="button" class="btn-cantidad" data-accion="mas" data-id="${item.id}" aria-label="Agregar una unidad">+</button>
                            </span>
                        </span>
                    </span>
                    <span class="precio-item-carrito">
                        ${formatearPrecio(item.precio * item.cantidad)}
                        <button type="button" class="btn-quitar" data-accion="quitar" data-id="${item.id}" aria-label="Quitar ${escaparHtml(item.nombre)}">&times;</button>
                    </span>
                </div>`).join("");
            btnPagar.disabled = false;
        }

        totalCarrito.textContent = formatearPrecio(calcularTotal());
    }

    // ---------- Agregar desde las tarjetas del catálogo ----------
    document.addEventListener("click", (evento) => {
        const boton = evento.target.closest(".btn-agregar");
        if (!boton) return;

        const id = Number(boton.dataset.id);
        const existente = carrito.find(item => item.id === id);

        if (existente) {
            existente.cantidad += 1;
        } else {
            carrito.push({
                id,
                nombre: boton.dataset.nombre,
                precio: Number(boton.dataset.precio),
                imagen: boton.dataset.imagen,
                cantidad: 1
            });
        }

        guardarCarrito();
        actualizarCarrito();
        mostrarAviso(`${boton.dataset.nombre} agregada 💗`);
    });

    // ---------- Acciones dentro del modal ----------
    listaCarrito.addEventListener("click", (evento) => {
        if (evento.target.closest(".btn-seguir-comprando")) {
            elementoModal.addEventListener("hidden.bs.modal", () => {
                if (window.location.pathname.toLowerCase() === URL_CATALOGO.toLowerCase()) {
                    const grid = document.getElementById("grid-titulo");
                    if (grid) grid.scrollIntoView({ behavior: "smooth" });
                } else {
                    window.location.href = URL_CATALOGO;
                }
            }, { once: true });

            bootstrap.Modal.getOrCreateInstance(elementoModal).hide();
            return;
        }

        const boton = evento.target.closest("[data-accion]");
        if (!boton) return;

        const id = Number(boton.dataset.id);
        const item = carrito.find(i => i.id === id);
        if (!item) return;

        switch (boton.dataset.accion) {
            case "mas":
                item.cantidad += 1;
                break;
            case "menos":
                item.cantidad -= 1;
                if (item.cantidad <= 0) carrito = carrito.filter(i => i.id !== id);
                break;
            case "quitar":
                carrito = carrito.filter(i => i.id !== id);
                break;
        }

        guardarCarrito();
        actualizarCarrito();
    });

    // ---------- Pagar por WhatsApp ----------
    btnPagar.addEventListener("click", () => {
        if (carrito.length === 0) return;

        const detalle = carrito
            .map(item => `- ${item.nombre} (x${item.cantidad}): ${formatearPrecio(item.precio * item.cantidad)}`)
            .join("\n");

        const mensaje = `¡Hola! Quiero hacer este pedido en Saltinas 🛍️:\n\n${detalle}\n\nTotal: ${formatearPrecio(calcularTotal())}`;
        const enlace = `https://wa.me/${NUMERO_WHATSAPP}?text=${encodeURIComponent(mensaje)}`;
        window.open(enlace, "_blank", "noopener");
    });

    // ---------- Inicialización ----------
    actualizarCarrito();
})();
