 function eliminar(id, nombre, apellido){
            document.querySelector("#propietario_eliminar_id").value = id;
            document.querySelector("#propietario_eliminar_nombre").innerHTML = nombre +" "+ apellido;
            $("#modal_eliminar_propietario").modal("show");
        }