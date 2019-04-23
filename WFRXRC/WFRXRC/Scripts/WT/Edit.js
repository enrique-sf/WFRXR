$(document).ready(function () {
    $("#tabs").tabs();

    //Carga Horas
    cargarHoraI();
    cargarHoraF();
    cargarHoraI2();
    cargarHoraF2();
    //Inicializo los select
    $('select').formSelect();

    $("#FInicio").on("change paste keyup", function () {
        $("#_fi").val(($(this).val()));
    });

    $("#FInicio").val("");
    $("#FInicio").val($("#_fi").val());

    $("#FInicio2").val("");
    $("#FInicio2").val($("#_fi2").val());

    $('#btnSaveSASAP').on("click", function (e) {
        var ba = validarCamposA();
        if (ba) {
            var jd = {
                data: []
            };
            //Recuperación de datos en el formulario
            var selectP = $("#proyecto").val();
            var s = 400;
            if (selectP === "No Asignado") {
                s = 0;
            }
            else if (selectP === "Vacaciones") {
                s = 1;
            }
            else if (selectP === "Incapacidad") {
                s = 2;
            }
            var asign = s;
            var _F = $('#FInicio').val();
            jd["data"].push({ 'idWT': $('#id_WT').val(), 'idEmpleado': $('#Id_Empleado').val(), 'fInicio': $('#FInicio').val(), 'fFin': $('#FTermino').val(), 'proyecto': $("#sProy").val(), 'hInicio': $('#timeI').val(), 'hFin': $('#timeF').val(), 'Asignacion': asign });
            var lm = JSON.stringify(jd);
            $.ajax({
                type: "POST",
                url: '../editarSAsignacion',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data > 0) {
                        id = data;
                        swal("¡Exito!", "Evento Guardado Con Exito, Id No." + id, "success", {
                            button: "OK",
                        });
                    }
                },
                error: function (xhr, httpStatusMessage, customErrorMessage) {
                    swal("¡Error!", "Error con el guardado de este evento!", "error", {
                        button: "OK",
                    });
                }
            });
        }
    });

    $('#btnSaveSASAP2').on("click", function (e) {
        var ba = validarCamposA2();
        if (ba) {
            var jd = {
                data: []
            };
            //Recuperación de datos en el formulario
            jd["data"].push({
                'idWT': $('#id_WT').val(), 'idsap': 0, 'idEmpleado': $('#Id_Empleado').val(), 'fInicio': $('#FInicio2').val(), 'fFin': $('#FTermino2').val(), 'proyecto': $("#ddlProy").val(), 'hInicio': $('#timeI2').val(), 'hFin': $('#timeF2').val(), 'servicio': $('#aservice').val(), 'cw': $('#acw').val(), 'ubicacion': $('#ubicacion').val(), 'crm': $('#descCRM').val(), 'cliente': $('#lstClientes').val(), 'responsable': $('#resSap').val(), 'sOrder': $('#servOrd').val()
            });
            var lm = JSON.stringify(jd);
            $.ajax({
                type: "POST",
                url: '../editarCAsignacion',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data== "E") {
                        id = data;
                        swal("¡Exito!", "Evento Guardado Con Exito", "success", {
                            button: "OK",
                        });
                    }
                },
                error: function (xhr, httpStatusMessage, customErrorMessage) {
                    swal("¡Error!", "Error con el guardado de este evento!", "error", {
                        button: "OK",
                    });
                }
            });
        }
    });
    
    $('#btnTerminarSASAP').on("click", function (e) {
        var ba = validarCamposA();
        if (ba) {
            var jd = {
                data: []
            };
            //Recuperación de datos en el formulario
            var selectP = $("#proyecto").val();
            var s = 400;
            if (selectP === "No Asignado") {
                s = 0;
            }
            else if (selectP === "Vacaciones") {
                s = 1;
            }
            else if (selectP === "Incapacidad") {
                s = 2;
            }
            var asign = s;
            var _F = $('#FInicio').val();
            jd["data"].push({ 'idWT': $('#id_WT').val(), 'idEmpleado': $('#Id_Empleado').val(), 'fInicio': $('#FInicio').val(), 'fFin': $('#FTermino').val(), 'proyecto': $("#sProy").val(), 'hInicio': $('#timeI').val(), 'hFin': $('#timeF').val(), 'Asignacion': asign });
            var lm = JSON.stringify(jd);
            $.ajax({
                type: "POST",
                url: '../editarTSAsignacion',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data != null) {
                        id = data;
                        swal("¡Exito!", "Evento Guardado Con Exito, Folio Carga" + id, "success", {
                            button: "OK",
                        });
                    }
                },
                error: function (xhr, httpStatusMessage, customErrorMessage) {
                    swal("¡Error!", "Error con el guardado de este evento!", "error", {
                        button: "OK",
                    });
                }
            });
        }
    });

    $('#btnTerminarSASAP2').on("click", function (e) {
        var ba = validarCamposA2();
        if (ba) {
            var jd = {
                data: []
            };
            //Recuperación de datos en el formulario
            jd["data"].push({
                'idWT': $('#id_WT').val(), 'idsap': 0, 'idEmpleado': $('#Id_Empleado').val(), 'fInicio': $('#FInicio2').val(), 'fFin': $('#FTermino2').val(), 'proyecto': $("#ddlProy").val(), 'hInicio': $('#timeI2').val(), 'hFin': $('#timeF2').val(), 'servicio': $('#aservice').val(), 'cw': $('#acw').val(), 'ubicacion': $('#ubicacion').val(), 'crm': $('#descCRM').val(), 'cliente': $('#lstClientes').val(), 'responsable': $('#resSap').val(), 'sOrder': $('#servOrd').val()
            });
            var lm = JSON.stringify(jd);
            $.ajax({
                type: "POST",
                url: '../editarTCAsignacion',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data != null) {
                        id = data;
                        swal("¡Exito!", "Evento Guardado Con Exito, Folio Carga: " + id, "success", {
                            button: "OK",
                        });
                    }
                },
                error: function (xhr, httpStatusMessage, customErrorMessage) {
                    swal("¡Error!", "Error con el guardado de este evento!", "error", {
                        button: "OK",
                    });
                }
            });
        }
    });
});

function cargarHoraI() {
    let $select = $("#timeI");
    for (let hr = 0; hr < 24; hr++) {

        let hrStr = hr.toString().padStart(2, "0") + ":";

        let val = hrStr + "00";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "15";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "30";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "45";
        $select.append('<option val="' + val + '">' + val + '</option>');
    }
}
function cargarHoraF() {
    let $select = $("#timeF");
    for (let hr = 0; hr < 24; hr++) {

        let hrStr = hr.toString().padStart(2, "0") + ":";

        let val = hrStr + "00";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "15";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "30";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "45";
        $select.append('<option val="' + val + '">' + val + '</option>');
    }
}

function cargarHoraI2() {
    let $select = $("#timeI2");
    for (let hr = 0; hr < 24; hr++) {

        let hrStr = hr.toString().padStart(2, "0") + ":";

        let val = hrStr + "00";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "15";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "30";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "45";
        $select.append('<option val="' + val + '">' + val + '</option>');
    }
}
function cargarHoraF2() {
    let $select = $("#timeF2");
    for (let hr = 0; hr < 24; hr++) {

        let hrStr = hr.toString().padStart(2, "0") + ":";

        let val = hrStr + "00";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "15";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "30";
        $select.append('<option val="' + val + '">' + val + '</option>');

        val = hrStr + "45";
        $select.append('<option val="' + val + '">' + val + '</option>');
    }
}

function validarCamposA() {
    var b = false;
    if ($('#FInicio').val() !== "") {
        if ($('#FTermino').val() !== "") {
            b = true;
        }
        else {
            swal("¡Error!", "Error con el guardado de este evento, revisar que exista una fecha de Termino", "error", {
                button: "OK",
            });
        }
    }
    else {
        swal("¡Error!", "Error con el guardado de este evento, revisar que exista una fecha de Inicio", "error", {
            button: "OK",
        });
    }
    return b;
}

function validarCamposA2() {
    var b = false;
    if ($('#FInicio2').val() !== "") {
        if ($('#FTermino2').val() !== "") {
            b = true;
        }
        else {
            swal("¡Error!", "Error con el guardado de este evento, revisar que exista una fecha de Termino", "error", {
                button: "OK",
            });
        }
    }
    else {
        swal("¡Error!", "Error con el guardado de este evento, revisar que exista una fecha de Inicio", "error", {
            button: "OK",
        });
    }
    return b;
}