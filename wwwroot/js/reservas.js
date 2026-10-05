let inquilinos = [];
let inmuebles = [];

const inputBusquedaInquilino = document.getElementById("inputBusquedaInquilino");
const resultadosInquilinos = document.getElementById("resultadosInquilinos");

const inputBusquedaInmueble = document.getElementById("inputBusquedaInmueble");
const resultadosInmuebles = document.getElementById("resultadosInmuebles");

// INQUILINOS

inputBusquedaInquilino.addEventListener("keyup", function () {
    const valor = inputBusquedaInquilino.value.toLowerCase().trim();

    if (valor === "") {
        resultadosInquilinos.innerHTML = "";
        return;
    }

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

    resultados.forEach(inquilino => {
        resultadosInquilinos.innerHTML += `
            <div class="card mb-2" style="cursor:pointer"
                 onclick="seleccionarInquilino(${inquilino.Id})">
                <div class="card-body">
                    <strong>${inquilino.Apellido} ${inquilino.Nombre}</strong>
                    <br>
                    <small>DNI: ${inquilino.Dni || ""}</small>
                </div>
            </div>
        `;
    });

    if (resultados.length === 0) {
        resultadosInquilinos.innerHTML = `
            <div class="alert alert-secondary">
                No se encontraron inquilinos.
            </div>
        `;
    }
}

function seleccionarInquilino(id) {
    const inquilino = inquilinos.find(x => x.Id === id);

    if (!inquilino) return;

    inputBusquedaInquilino.value =
        `${inquilino.Apellido} ${inquilino.Nombre} - DNI: ${inquilino.Dni || ""}`;

    document.getElementById("IdInquilino").value = inquilino.Id;
    resultadosInquilinos.innerHTML = "";
}
// INMUEBLES

inputBusquedaInmueble.addEventListener("keyup", function () {
    const valor = inputBusquedaInmueble.value.toLowerCase().trim();

    if (valor === "") {
        resultadosInmuebles.innerHTML = "";
        return;
    }

    const resultados = inmuebles.filter(inmueble => {
        const direccion = (inmueble.Direccion || "").toLowerCase();
        const nombreDuenio = (inmueble.Duenio?.Nombre || "").toLowerCase();
        const apellidoDuenio = (inmueble.Duenio?.Apellido || "").toLowerCase();

        return direccion.includes(valor) ||
               nombreDuenio.includes(valor) ||
               apellidoDuenio.includes(valor);
    });

    mostrarInmuebles(resultados);
});

function mostrarInmuebles(resultados) {
    resultadosInmuebles.innerHTML = "";

    resultados.forEach(inmueble => {
        const nombreDuenio = inmueble.Duenio?.Nombre || "";
        const apellidoDuenio = inmueble.Duenio?.Apellido || "";

        resultadosInmuebles.innerHTML += `
            <div class="card mb-2" style="cursor:pointer"
                 onclick="seleccionarInmueble(${inmueble.Id})">
                <div class="card-body">
                    <strong>${inmueble.Direccion}</strong>
                    <br>
                    <small>Dueño: ${apellidoDuenio} ${nombreDuenio}</small>
                </div>
            </div>
        `;
    });

    if (resultados.length === 0) {
        resultadosInmuebles.innerHTML = `
            <div class="alert alert-secondary">
                No se encontraron inmuebles.
            </div>
        `;
    }
}

function seleccionarInmueble(id) {
    const inmueble = inmuebles.find(x => x.Id === id);

    if (!inmueble) return;

    const nombreDuenio = inmueble.Duenio?.Nombre || "";
    const apellidoDuenio = inmueble.Duenio?.Apellido || "";

    inputBusquedaInmueble.value =
        `${inmueble.Direccion} - ${apellidoDuenio} ${nombreDuenio}`;

    document.getElementById("IdInmueble").value = inmueble.Id;
    resultadosInmuebles.innerHTML = "";
}