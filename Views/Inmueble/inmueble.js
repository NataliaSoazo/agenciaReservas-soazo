function eliminar(id, nombre) {
            document.querySelector("#Domicilio_eliminar_id").value = id;
            document.querySelector("#Domicilio_eliminar_nombre").innerHTML = nombre;
            $("#modal_eliminar_inmueble").modal("show");
        }