$(document).ready(function () {
    $('select').formSelect();


    $('#btn_guardarh').on("click", function (e) {
        $('#Fecha_Inicio').val(formatoFecha($('#Fecha_Inicio').val()));
        validarCampos();
        $('#btn_guardar').trigger("click");
    });
});

function validarCampos() {
    //En esta funcion validaremos el llenado de los campos
}

function formatoFecha(fecha) {
    //var datearray = fecha.split("/");
    //var _fecha = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
    //return _fecha;
    return fecha;
}