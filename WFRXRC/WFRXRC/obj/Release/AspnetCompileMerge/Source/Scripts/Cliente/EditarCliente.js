$(document).ready(function () {
    $('select').formSelect();
    //Modificar
    $('#btnMod').on("click", function (e) {
        //recupero el valor
        var Contacto = $('#contacto').val();
        if (Contacto !== "") {
            if (!comprobar(Contacto)) {//si es falso, significa que no existe, entonces lo hara
                //recorro el div buscando un input que se modifica y le cambio el valor anitugo por el nuevo
                $("#divContactos  > input").each(function () {
                    var paux = $("#paux").val();//donde almacene temporalmente el valor a cambiar
                    var li = $(this).val();
                    if (li === paux) {
                        $(this).val(Contacto);
                        $('select option:contains("' + paux + '")').text(Contacto);
                        return;
                    }
                });
                //Reviso si tiene la clase sc, que significa que es el renglon para indicar que no tiene contactos
                $("#ctls > option").each(function () {
                    var li = $(this).hasClass("sc");
                    if (li) {
                        $(this).remove();
                    }
                });
                //modifico el valor antiguo en la lista para que el usuario vea los que tiene capturados
                $("#ctls > option").each(function () {
                    var paux = $("#paux").val();//donde almacene temporalmente el valor a cambiar
                    var li = $(this).val();
                    if (li === paux) {
                        $(this).val(Contacto);
                        return;
                    }
                });
            }
            else {
                //manda alerta
            }
        }
        $("#contacto").val("");
        $("#paux").val("");
    });

    //Agregar
    $('#btnAdd').on("click", function (e) {
        //recupero el valor
        var Contacto = $('#contacto').val();
        if (Contacto !== "") {
            if (!comprobar(Contacto)) {//si es falso, significa que no existe, entonces lo hara
                //lo agrego a un input hide para posteriormente mandarlo al controller
                $("#divContactos").append('<input id="contactos" name="contactos" value=' + Contacto + ' type="hidden"/>');
                //Reviso si tiene la clase sc, que significa que es el renglon para indicar que no tiene contactos
                $("#ctls > option").each(function () {
                    var li = $(this).hasClass("sc");
                    if (li) {
                        $(this).remove();
                    }
                });
                //lo agrego a una lista para que el usuario vea los que tiene capturados
                $("#ctls").append('<option value="' + Contacto + '">' + Contacto + '</option>');
            }
            else {
                //manda alerta de contacto existente
                swal("¡Espera!", "Contacto Existente!", "warning", {
                    button: "OK",
                });
            }
        }
        $("#contacto").val("");
        $("#paux").val("");
    });

    //
    $("#ctls").change(function () {
        var val = $(this).val();
        $("#contacto").val(val);
        $("#paux").val(val);
    });

    //Cargo en el div "divContactos los inputs ya en bd
    addinputs();
});

function addinputs() {
    $("#ctls > option").each(function () {
        var li = $(this).val();
        //lo agrego a una lista para que el usuario vea los que tiene capturados
        $("#divContactos").append('<input id="contactos" name="contactos" value=' + li + ' type="hidden"/>');
    });
}

//funcion para corrobar si el dato que escribe ya existe o no
function comprobar(valor) {
    var band = false;
    $("#ctls > option").each(function () {
        var li = $(this).val();
        //si el valor que captura ya esta en la lista manda true
        if (li === valor) {
            band = true;
        }
    });
    return band;
}