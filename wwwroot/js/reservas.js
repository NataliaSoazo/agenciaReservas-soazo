const inquilinos = window.inquilinosData || [];
const inmuebles = window.inmueblesData || [];

const inputBusquedaInquilino =
    document.getElementById("inputBusquedaInquilino");

const resultadosInquilinos =
    document.getElementById("resultadosInquilinos");

const inputBusquedaInmueble =
    document.getElementById("inputBusquedaInmueble");

const resultadosInmuebles =
    document.getElementById("resultadosInmuebles");

const idInquilino = document.getElementById("IdInquilino");
const idInmueble = document.getElementById("IdInmueble");

const fechaDesde = document.getElementById("FechaDesde");
const fechaHasta = document.getElementById("FechaHasta");
const inputMonto = document.getElementById("Monto");

// INQUILINOS

inputBusquedaInquilino.addEventListener("input", function () {
    const valor = this.value.toLowerCase().trim();

    // Si se modifica la búsqueda, se debe volver a seleccionar.
    idInquilino.value = "";
    resultadosInquilinos.innerHTML = "";

    if (!valor) return;

    const resultados = inquilinos.filter(inquilino => {
        const nombre = (inquilino.Nombre || "").toLowerCase();
        const apellido = (inquilino.Apellido || "").toLowerCase();
        const dni = String(inquilino.Dni || "").toLowerCase();

        return nombre.includes(valor) ||
               apellido.includes(valor) ||
               dni.includes(valor);
    });

    mostrarInquilinos(resultados);
});

function mostrarInquilinos(resultados) {
    resultadosInquilinos.innerHTML = "";

    if (resultados.length === 0) {
        resultadosInquilinos.innerHTML = `
            <div class="alert alert-secondary">
                No se encontraron inquilinos.
            </div>`;
        return;
    }

    resultados.forEach(inquilino => {
        const tarjeta = document.createElement("div");
        tarjeta.className = "card mb-2";
        tarjeta.style.cursor = "pointer";

        tarjeta.innerHTML = `
            <div class="card-body">
                <strong></strong><br>
                <small></small>
            </div>`;

        tarjeta.querySelector("strong").textContent =
            `${inquilino.Apellido || ""} ${inquilino.Nombre || ""}`;

        tarjeta.querySelector("small").textContent =
            `DNI: ${inquilino.Dni || ""}`;

        tarjeta.addEventListener("click", () => {
            seleccionarInquilino(inquilino.Id);
        });

        resultadosInquilinos.appendChild(tarjeta);
    });
}

function seleccionarInquilino(id) {
    const inquilino = inquilinos.find(x => x.Id === id);
    if (!inquilino) return;

    inputBusquedaInquilino.value =
        `${inquilino.Apellido || ""} ${inquilino.Nombre || ""} - DNI: ${inquilino.Dni || ""}`;

    idInquilino.value = inquilino.Id;
    resultadosInquilinos.innerHTML = "";
}

// INMUEBLES

inputBusquedaInmueble.addEventListener("input", function () {
    const valor = this.value.toLowerCase().trim();

    idInmueble.value = "";
    resultadosInmuebles.innerHTML = "";
    inputMonto.value = "";

    if (!valor) return;

    const resultados = inmuebles.filter(inmueble => {
        const direccion = (inmueble.Direccion || "").toLowerCase();
        const nombreDuenio =
            (inmueble.Duenio?.Nombre || "").toLowerCase();
        const apellidoDuenio =
            (inmueble.Duenio?.Apellido || "").toLowerCase();

        return direccion.includes(valor) ||
               nombreDuenio.includes(valor) ||
               apellidoDuenio.includes(valor);
    });

    mostrarInmuebles(resultados);
});

function mostrarInmuebles(resultados) {
    resultadosInmuebles.innerHTML = "";

    if (resultados.length === 0) {
        resultadosInmuebles.innerHTML = `
            <div class="alert alert-secondary">
                No se encontraron inmuebles.
            </div>`;
        return;
    }

    resultados.forEach(inmueble => {
        const tarjeta = document.createElement("div");
        tarjeta.className = "card mb-2";
        tarjeta.style.cursor = "pointer";

        const nombreDuenio = inmueble.Duenio?.Nombre || "";
        const apellidoDuenio = inmueble.Duenio?.Apellido || "";

        tarjeta.innerHTML = `
            <div class="card-body">
                <strong></strong><br>
                <small></small><br>
                <small></small>
            </div>`;

        tarjeta.querySelector("strong").textContent =
            inmueble.Direccion || "";

        tarjeta.querySelectorAll("small")[0].textContent =
            `Dueño: ${apellidoDuenio} ${nombreDuenio}`;

        tarjeta.querySelectorAll("small")[1].textContent =
            `Precio por día: ${formatearPrecio(inmueble.Precio)}`;

        tarjeta.addEventListener("click", () => {
            seleccionarInmueble(inmueble.Id);
        });

        resultadosInmuebles.appendChild(tarjeta);
    });
}

function seleccionarInmueble(id) {
    const inmueble = inmuebles.find(x => x.Id === id);
    if (!inmueble) return;

    const nombreDuenio = inmueble.Duenio?.Nombre || "";
    const apellidoDuenio = inmueble.Duenio?.Apellido || "";

    inputBusquedaInmueble.value =
        `${inmueble.Direccion || ""} - ${apellidoDuenio} ${nombreDuenio}`;

    idInmueble.value = inmueble.Id;
    resultadosInmuebles.innerHTML = "";

    calcularMonto();
}

// CÁLCULO DEL MONTO

function calcularMonto() {
    const id = Number(idInmueble.value);
    const inmueble = inmuebles.find(x => x.Id === id);

    if (!inmueble || !fechaDesde.value || !fechaHasta.value) {
        inputMonto.value = "";
        return;
    }

    const desde = fechaDesde.value;
    const hasta = fechaHasta.value;

    // Las fechas deben formar un rango válido.
    if (desde >= hasta) {
        inputMonto.value = "";
        return;
    }

    // Al usar fechas ISO yyyy-MM-dd, la diferencia en UTC
    // evita problemas por cambios de horario de verano.
    const inicio = new Date(`${desde}T00:00:00Z`);
    const fin = new Date(`${hasta}T00:00:00Z`);

    const dias = (fin - inicio) / 86400000;
    const precioDiario = Number(inmueble.Precio);

    if (!Number.isFinite(precioDiario) ||
        !Number.isFinite(dias) ||
        dias <= 0) {
        inputMonto.value = "";
        return;
    }

    inputMonto.value = (precioDiario * dias).toFixed(2);
}

function formatearPrecio(valor) {
    const precio = Number(valor);

    if (!Number.isFinite(precio)) return "No disponible";

    return precio.toLocaleString("es-AR", {
        style: "currency",
        currency: "ARS"
    });
}

// Recalcular cuando se cambian las fechas.
fechaDesde.addEventListener("change", calcularMonto);
fechaHasta.addEventListener("change", calcularMonto);

// Inicialización para editar una reserva existente.
document.addEventListener("DOMContentLoaded", function () {
    const inquilino = inquilinos.find(
        x => String(x.Id) === String(idInquilino.value)
    );

    if (inquilino) {
        seleccionarInquilino(inquilino.Id);
    }

    const inmueble = inmuebles.find(
        x => String(x.Id) === String(idInmueble.value)
    );

    if (inmueble) {
        seleccionarInmueble(inmueble.Id);
    }
});