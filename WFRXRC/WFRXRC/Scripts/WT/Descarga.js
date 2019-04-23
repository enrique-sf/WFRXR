$(document).ready(function () {
    //Inicializo los select
    $('select').formSelect();
    $('input[type=radio][name=group1]').change(function () {
        if (this.value == 'M') {
            $('#xMes').css("display", "block");
            $('#xPeriodo').css("display", "none");
        }
        else if (this.value == 'P') {
            $('#xMes').css("display", "none");
            $('#xPeriodo').css("display", "block");
        }
    });

    $("#xmesaño").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // No pasa nada
            return;
        }


        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $('#btnD').on("click", function (e) {
        var anho = $('#xmesaño').val();
        if (anho != "") {
            $('#anho').val(anho);
            var currYear = new Date().toString().match(/(\d{4})/)[1];
            if (anho >= 1500 && anho <= currYear) {
                $('#mesP').val($('#mes').val());
                $('#btnDReporte').trigger("click");
            } else {
                swal("¡Error!", "Año Invalido", "error", {
                    button: "OK",
                });
            }
        }
        else {
            swal("¡Atención!", "Ingresar Año", "warning", {
                button: "OK",
            });
        }
    });
});