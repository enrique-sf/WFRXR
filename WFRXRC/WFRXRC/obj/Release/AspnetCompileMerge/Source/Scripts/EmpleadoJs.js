$(document).ready(function () {
    //$('.datepicker').datepicker();
    $('select').formSelect();
    $('.tabs').tabs();

    var d = $('#FechaNacimiento').val();
    d = d.split(' ')[0];
    $('#FechaNacimiento').val(d);

    var ddd = $('#FechaIngreso').val();
    ddd = ddd.split(' ')[0];
    $('#FechaIngreso').val(ddd);

    //Para validar los decimales del costo unitario
    $('body').on('focusout', '.curp', function () {
        var curps = $(this).val();
        var curp = curps.toUpperCase();
        //Realizamos un Uppercasse para hacer las minusculas mayusculas
        $(this).val(curp);
    });

    $('#btn_guardarh').on("click", function (e) {
        $('#FechaNacimiento').val(formatoFecha($('#FechaNacimiento').val()));
        $('#FechaIngreso').val(formatoFecha($('#FechaIngreso').val()));
        validarCampos();
        //
        $('#Dias_Asignados').val('0');
        $('#Dias_SAginacion').val('0');
        $('#Dias_Vacaciones').val('0');
        var u = $('#Usuario').val();
        $('#btn_guardar').trigger("click");
    });

    $('#chbPwd').change(function () {
        var x = document.getElementById("Pwd");
        if ($(this).is(":checked")) {
            x.type = "text";
        } else {
            x.type = "password";
        }
    });

    $('body').on('focusout', '.dinero', function () {
        var xx = $(this).val().replace("$", "");
        var _miles = ',';
        var _decimales = '.';
        if (xx != '') {
            if (_decimales === '.') {
                //Hace la conversion a 2 decimales
                var _xv = xx.replace(',', '');
                xx = _xv;
                $(this).val("$" + parseFloat(xx).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            } else if (_decimales === ',') {
                var _xv = xx.replace('.', '');
                xx = _xv.replace(',', '.');
                var _xpf = parseFloat(xx.replace(',', '.')).toFixed(2);
                _xpf = _xpf.replace('.', ',');
                $(this).val("$" + _xpf.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."));
            }
        }
        else {
            $(this).val("$0.00");
        }
    });

    //Para que no ingrese letras ni signos extra
    $('body').on('keydown', '.dinero', function (e) {
        if (e.keyCode == 110 || e.keyCode == 190) {
            if ($(this).val().indexOf('.') != -1) {
                e.preventDefault();
            }
        }
        else {  // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything

                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        }
    });

    validarDecS();
    validarDecB();
});

function validarDecS() {
    validarDec('#Sueldo');
}
function validarDecB() {
    validarDec('#Bono_Monto');
}

function validarDec(campo) {
    var xx = $(campo).val().replace("$", "");
    var _miles = ',';
    var _decimales = '.';
    if (xx != '') {
        if (_decimales === '.') {
            //Hace la conversion a 2 decimales
            var _xv = xx.replace(',', '');
            xx = _xv;
            $(campo).val("$" + parseFloat(xx).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        } else if (_decimales === ',') {
            var _xv = xx.replace('.', '');
            xx = _xv.replace(',', '.');
            var _xpf = parseFloat(xx.replace(',', '.')).toFixed(2);
            _xpf = _xpf.replace('.', ',');
            $(campo).val("$" + _xpf.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."));
        }
    }
    else {
        $(campo).val("$0.00");
    }
}

function validarCampos() {
    //En esta funcion validaremos el llenado de los campos
}

function formatoFecha(fecha) {
    //el siguiente codigo aplica para fechas de tipo mm//dd/aaa
    //var datearray = fecha.split("/");
    //var _fecha = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
    //return _fecha;
    return fecha;
}