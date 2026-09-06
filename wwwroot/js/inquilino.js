function eliminar(id, nombre, apellido) {
            document.querySelector("#Inquilino_eliminar_id").value = id;
            document.querySelector("#Inquilino_eliminar_nombre").innerHTML = nombre +" "+ apellido;
            $("#modal_eliminar_Inquilino").modal("show");
        }