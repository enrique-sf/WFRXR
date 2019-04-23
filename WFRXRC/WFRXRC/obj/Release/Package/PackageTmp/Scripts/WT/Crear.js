$(document).ready(function () {
    $("#tabs").tabs();
    loadCustomers();//para cargar el dropdownlist   
    loadCustomersSA();//para cargar el dropdownlist   
    //cargaremos eventos si es que hay en la bd
    loadEvents();
    //
    loadProjects();

    $('#SelASAp').change(function () {
        var valor = $('#SelASAp').val();
        if (valor == 1) {//Si es 1 sifnifica con asginacion sap
            $("#SASAP").css("display", "none");
            $("#ASAP").css("display", "block");
            //
            var ell = document.getElementById("tabs");
            var instances = M.Tabs.getInstance(ell);
            instances.select('DLoad');
        }
        else if (valor == 2) {//Si es 2 sifnifica sin asginacion sap
            $("#SASAP").css("display", "block");
            $("#ASAP").css("display", "none");
            //
            var ell = document.getElementById("tabs");
            var instances = M.Tabs.getInstance(ell);
            instances.select('DLoad');
        }
        else {
            $("#SASAP").css("display", "none");
            $("#ASAP").css("display", "none");
        }
    });

    //Carga Horas
    cargarHoraI();
    cargarHoraF();
    cargarHoraI2();
    cargarHoraF2();

    //Inicializo los select
    $('select').formSelect();

    $("#sProy").change(function () {
        var opc = $('#sProy Option:Selected').val();

        if (opc === "0") {
            $('#idcarga').prop('disabled', true);
            $('#AsSAP').prop('disabled', true);
        } else {
            $('#idcarga').prop('disabled', false);
            $('#AsSAP').prop('disabled', false);
        }

    });

    $('#btnSaveSASAP').on("click", function (e) {
        var ba = validarCamposA();
        if (ba) {
            var jd = {
                data: []
            };
            //Recuperación de datos en el formulario
            var selectP = $("#sProy").val();
            var asign = selectP;
            jd["data"].push({ 'cliente': $('#sacliente').val(), 'idEmpleado': $('#idH').val(), 'fInicio': $('#FInicio').val(), 'fFin': $('#FTermino').val(), 'proyecto': $("#sProy").val(), 'hInicio': $('#timeI').val(), 'hFin': $('#timeF').val(), 'Asignacion': asign });
            var lm = JSON.stringify(jd);
            $.ajax({
                type: "POST",
                url: 'busquedaRegistroSas',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data === "M1") {
                        swal({
                            title: "Atención",
                            text: "Existe ya uno o más registros con parametros similares, ¿Deseas agregar otro?",
                            icon: "warning",
                            buttons: true,
                            dangerMode: true,
                        })
                            .then((willDelete) => {
                                if (willDelete) {
                                    $.ajax({
                                        type: "POST",
                                        url: 'guardadoSAsignacion',
                                        data: lm,
                                        contentType: "application/json; charset=utf-8",
                                        success: function (data) {
                                            if (data != 990004009) {
                                                id = data;
                                                swal("¡Exito!", "Evento Guardado Con Exito, Id No." + id, "success", {
                                                    button: "OK",
                                                });
                                            }
                                            else if (data == 990004009) {
                                                swal("¡Error!", "Error con el guardado de este evento!", "error", {
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
                                } else {
                                    swal("OK!");
                                }
                            });
                    }
                    else if (data === "0") {
                        $.ajax({
                            type: "POST",
                            url: 'guardadoSAsignacion',
                            data: lm,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data != 990004009) {
                                    id = data;
                                    swal("¡Exito!", "Evento Guardado Con Exito, Id No." + id, "success", {
                                        button: "OK",
                                    });
                                }
                                else if (data == 990004009) {
                                    swal("¡Error!", "Error con el guardado de este evento!", "error", {
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
                'idsap': 0, 'idEmpleado': $('#idH').val(), 'fInicio': $('#FInicio2').val(), 'fFin': $('#FTermino2').val(), 'proyecto': $("#ddlProy").val(), 'hInicio': $('#timeI2').val(), 'hFin': $('#timeF2').val(), 'servicio': $('#aservice').val(), 'cw': $('#acw').val(), 'ubicacion': $('#ubicacion').val(), 'crm': $('#descCRM').val(), 'cliente': $('#sCliente').val(), 'responsable': $('#resSap').val(), 'sOrder': $('#servOrd').val()
            });
            var lm = JSON.stringify(jd);
            //primera validacion para saber si ya hay en sistema un registro similar
            $.ajax({
                type: "POST",
                url: 'busquedaRegistroIns',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data === "M1") {
                        swal({
                            title: "Atención",
                            text: "Existe ya uno o más registros con parametros similares, ¿Deseas agregar otro?",
                            icon: "warning",
                            buttons: true,
                            dangerMode: true,
                        })
                            .then((willDelete) => {
                                if (willDelete) {
                                    $.ajax({
                                        type: "POST",
                                        url: 'guardadoCAsignacion',
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
                                } else {
                                    swal("OK!");
                                }
                            });
                    }
                    else if (data === "0") {
                        $.ajax({
                            type: "POST",
                            url: 'guardadoCAsignacion',
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
            var selectP = $("#sProy").val();
            var asign = selectP;
            var _F = $('#FInicio').val();
            jd["data"].push({ 'idEmpleado': $('#idH').val(), 'fInicio': $('#FInicio').val(), 'fFin': $('#FTermino').val(), 'proyecto': $("#sProy").val(), 'hInicio': $('#timeI').val(), 'hFin': $('#timeF').val(), 'Asignacion': asign });
            var lm = JSON.stringify(jd);

            //primera validacion para saber si ya hay en sistema un registro similar
            $.ajax({
                type: "POST",
                url: 'busquedaRegistroSas',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data === "M1") {
                        swal({
                            title: "Atención",
                            text: "Existe ya uno o más registros con parametros similares, ¿Deseas agregar otro?",
                            icon: "warning",
                            buttons: true,
                            dangerMode: true,
                        })
                            .then((willDelete) => {
                                if (willDelete) {
                                    $.ajax({
                                        type: "POST",
                                        url: 'terminadoSAsignacion',
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
                                } else {
                                    swal("OK!");
                                }
                            });
                    }
                    else if (data === "0") {
                        $.ajax({
                            type: "POST",
                            url: 'terminadoSAsignacion',
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
                'idsap': 0, 'idEmpleado': $('#idH').val(), 'fInicio': $('#FInicio2').val(), 'fFin': $('#FTermino2').val(), 'proyecto': $("#ddlProy").val(), 'hInicio': $('#timeI2').val(), 'hFin': $('#timeF2').val(), 'servicio': $('#aservice').val(), 'cw': $('#acw').val(), 'ubicacion': $('#ubicacion').val(), 'crm': $('#descCRM').val(), 'cliente': $('#sCliente').val(), 'responsable': $('#resSap').val(), 'sOrder': $('#servOrd').val()
            });
            var lm = JSON.stringify(jd);
            $.ajax({
                type: "POST",
                url: 'busquedaRegistroSas',
                data: lm,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data === "M1") {
                        swal({
                            title: "Atención",
                            text: "Existe ya uno o más registros con parametros similares, ¿Deseas agregar otro?",
                            icon: "warning",
                            buttons: true,
                            dangerMode: true,
                        })
                            .then((willDelete) => {
                                if (willDelete) {
                                    $.ajax({
                                        type: "POST",
                                        url: 'terminarCAsignacion',
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
                                } else {
                                    swal("OK!");
                                }
                            });
                    }
                    else if (data === "0") {
                        $.ajax({
                            type: "POST",
                            url: 'terminarCAsignacion',
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

function resetForm($dialogContent) {
    $dialogContent.find("input").val("");
    $dialogContent.find("textarea").val("");
}

function setupStartAndEndTimeFields($startTimeField, $endTimeField, calEvent, timeslotTimes) {

    for (var i = 0; i < timeslotTimes.length; i++) {
        var startTime = timeslotTimes[i].start;
        var endTime = timeslotTimes[i].end;
        var startSelected = "";
        if (startTime.getTime() === calEvent.start.getTime()) {
            startSelected = "selected=\"selected\"";
        }
        var endSelected = "";
        if (endTime.getTime() === calEvent.end.getTime()) {
            endSelected = "selected=\"selected\"";
        }
        $startTimeField.append("<option value=\"" + startTime + "\" " + startSelected + ">" + timeslotTimes[i].startFormatted + "</option>");
        $endTimeField.append("<option value=\"" + endTime + "\" " + endSelected + ">" + timeslotTimes[i].endFormatted + "</option>");

    }
    $endTimeOptions = $endTimeField.find("option");
    $startTimeField.trigger("change");
}

function saveEvent(id_empleado, inicio, fin, servicio, ubicacion, desc_crm, cliente, cw, responsable, serviceOr) {
    var id = 0;
    var jData = {
        data: []
    };
    jData["data"].push({ 'id_empleado': id_empleado, 'inicio': inicio, 'fin': fin, 'servicio': servicio, 'ubicacion': ubicacion, 'desc_crm': desc_crm, 'cliente': cliente, 'cw': cw, 'rsap': responsable, 'sorder': serviceOr });
    //jData["data"].push({ 'id': 0, 'start': inicio, 'end': fin, 'title': titulo, 'body': descripcion, 'proyecto': proyecto, 'empleado': 0 });
    var lm = JSON.stringify(jData);
    $.ajax({
        type: "POST",
        url: 'procesarJson',
        data: lm,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data > 0) {
                id = data;
                swal("¡Exito!", "Evento Guardado Con Exito!", "success", {
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
    return id;
}

function saveSAP(id_empleado, inicio, fin, servicio, ubicacion, desc_crm, cliente, cw, responsable, serviceOr) {
    var id = 0;
    var jData = {
        data: []
    };
    jData["data"].push({ 'id_empleado': id_empleado, 'inicio': inicio, 'fin': fin, 'servicio': servicio, 'ubicacion': ubicacion, 'desc_crm': desc_crm, 'cliente': cliente, 'cw': cw, 'rsap': responsable, 'sorder': serviceOr });
    //jData["data"].push({ 'id': 0, 'start': inicio, 'end': fin, 'title': titulo, 'body': descripcion, 'proyecto': proyecto, 'empleado': 0 });
    var lm = JSON.stringify(jData);
    $.ajax({
        type: "POST",
        url: 'procesarTablaSap',
        data: lm,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data > 0) {
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            swal("¡Error!", "Error con el guardado de este evento!", "error", {
                button: "OK",
            });
        }
    });
    return id;
}

function loadCustomers() {
    $.ajax({
        type: "POST",
        url: 'getClientes',
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (data !== null || data !== "") {
                var ddlI = $("#sCliente");
                $.each(data, function (i, dataj) {
                    ddlI.append($("<option />").val(dataj.Id_Cliente).text(dataj.RazonSocial));
                });
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
}

function loadCustomersSA() {
    $.ajax({
        type: "POST",
        url: 'getClientes',
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (data !== null || data !== "") {
                var ddlI = $("#sacliente");
                $.each(data, function (i, dataj) {
                    if (dataj.Id_Cliente === 3) {
                        ddlI.append($("<option selected />").val(dataj.Id_Cliente).text(dataj.RazonSocial));
                    }
                    else {
                        ddlI.append($("<option />").val(dataj.Id_Cliente).text(dataj.RazonSocial));
                    }
                });
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
}

function loadProjects() {
    $.ajax({
        type: "POST",
        url: 'getProyectos',
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (data !== null || data !== "") {
                var ddlI = $("#ddlProy");
                $.each(data, function (i, dataj) {
                    ddlI.append($("<option />").val(dataj.Id_Proyecto).text(dataj.Nombre_Proyecto));
                });
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
}

function loadEvents() {
    $.ajax({
        type: "POST",
        url: 'getEvents',
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (data !== null || data !== "") {
                var eventData = {
                    events: [
                    ]
                };
                $.each(data, function (i, dataj) {
                    var arrI = dataj.cargaxDiaI.split('/');
                    var arrF = dataj.cargaxDiaF.split('/');
                    var hi = dataj.horaI.split(':');
                    var mi = hi[1].split(' ');
                    var hF = dataj.horaF.split(':');
                    var mf = hF[1].split(' ');
                    //eventData["events"].push({ 'id': dataj.id_Dia, 'start': new Date(arrI[2], arrI[1] - 1, arrI[0], hi[0], mi[0]), 'end': new Date(arrF[2], arrF[1] - 1, arrF[0], hF[0], mf[0]), 'title': dataj.titulo, 'body': dataj.Explicacion.trim(), 'proyecto': dataj.idProyecto, 'empleado': 0 });//para la fecha en el mes se le pones -1 ya que es un arreglo y comienza en 0
                    eventData["events"].push({ 'id_empleado': dataj.idEmpleado, 'start': new Date(arrI[2], arrI[1] - 1, arrI[0], hi[0], mi[0]), 'end': new Date(arrF[2], arrF[1] - 1, arrF[0], hF[0], mf[0]), 'servicio': "Servicio", 'ubicacion': "Leon", 'desc_crm': "CRM", 'cliente': "1", 'cw': "CW34", 'rsap': "1", 'sorder': "22", 'title': dataj.Explicacion });//para la fecha en el mes se le pones -1 ya que es un arreglo y comienza en 0
                });
                calendario(eventData);
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
}

function calendario(datos) {
    var $calendar = $('#calendar');
    var id = 10;
    $('#calendar').weekCalendar({
        data: datos,
        timeslotsPerHour: 4,
        businessHours: { start: 1, end: 24, limitDisplay: true },//horas a mostrar
        allowCalEventOverlap: true,
        overlapEventsSeparate: true,//para que no se enpalmen eventos
        totalEventsWidthPercentInOneColumn: 95,//el porcentaje de ancho que ocupara el evento
        height: function ($calendar) {
            return $(window).height() - $('h1').outerHeight(true);
        },
        eventRender: function (calEvent, $event) {
            if (calEvent.end.getTime() < new Date().getTime()) {
                $event.css('backgroundColor', '#68a1e5');
                $event.find('.time').css({
                    backgroundColor: '#999',
                    border: '1px solid #888'
                });
            }
        },
        eventNew: function (calEvent, $event) {
            var $dialogContent = $("#event_edit_container");
            resetForm($dialogContent);
            var idField = $dialogContent.find("input[name='id_Dia']").val(calEvent.id);
            var startField = $dialogContent.find("input[name='FInicio']").val(calEvent.start);
            var endField = $dialogContent.find("input[name='FFin']").val(calEvent.end);
            var titleField = $dialogContent.find("input[name='title']");
            var ubicacion = $dialogContent.find("input[name='location']");
            var descripcion = $dialogContent.find("input[name='project']").val(calEvent.desc);
            var cliente = $dialogContent.find("select[name='cliente']").val(calEvent.cliente);
            var cw = $dialogContent.find("input[name='cw']").val(calEvent.cw);
            var rsap = $dialogContent.find("input[name='rsap']").val(calEvent.rsap);
            var sor = $dialogContent.find("input[name='sOr']").val(calEvent.sOr);

            $dialogContent.dialog({
                modal: true,
                title: "Nuevo Evento",
                close: function () {
                    $dialogContent.dialog("destroy");
                    $dialogContent.hide();
                    $('#calendar').weekCalendar("removeUnsavedEvents");
                },
                buttons: {
                    save: function () {
                        calEvent.id = id;
                        id++;
                        //primero se hara el guardado en la tabla SAP
                        saveSAP(0, new Date(startField.val()), new Date(endField.val()), titleField.val(), ubicacion.val(), descripcion.val(), cliente.val(), cw.val(), rsap.val(), sor.val());

                        //seguimos con el guardado por dia
                        var arr = getDates(startField.val(), endField.val());
                        for (var i = 0; i < arr.length; i++) {
                            $calendar.weekCalendar("removeUnsavedEvents");
                            var fi = formatoFecha(startField.val());
                            var inicio = sethoraI(arr[i]);
                            var fin = sethoraF(arr[i]);
                            calEvent.start = inicio;
                            calEvent.end = fin;
                            calEvent.title = titleField.val();//servicio
                            calEvent.ubicacion = ubicacion.val();
                            calEvent.desc = descripcion.val();
                            calEvent.cliente = cliente.val();
                            calEvent.cw = cw.val();
                            calEvent.rsap = rsap.val();// a quien se le asigna
                            calEvent.sOr = sor.val();
                            $calendar.weekCalendar("updateEvent", calEvent);
                            $dialogContent.dialog("close");
                            //funcion para guardar el evento creado
                            saveEvent(0, calEvent.start, calEvent.end, calEvent.title, calEvent.ubicacion, calEvent.desc, calEvent.cliente, calEvent.cw, calEvent.rsap, calEvent.sOr);
                        }
                        location.reload();
                    },
                    cancel: function () {
                        $dialogContent.dialog("close");
                    }
                }
            }).show();
            //setupStartAndEndTimeFields(startField, endField, calEvent, $calendar.weekCalendar("getTimeslotTimes", calEvent.start));
        },
        eventDrop: function (calEvent, $event) {
            //var $dialogContent = $("#event_edit_container");
            //var idField = $dialogContent.find("input[name='id_Dia']").val(calEvent.id);
            //var startField = $dialogContent.find("select[name='start']").val(calEvent.start);
            //var endField = $dialogContent.find("select[name='end']").val(calEvent.end);
            //var titleField = $dialogContent.find("input[name='title']").val(calEvent.title);
            //var bodyField = $dialogContent.find("textarea[name='body']");
            //var project = $dialogContent.find("select[name='project']").val(calEvent.project);
            //bodyField.val(calEvent.body);
            //idField.val(calEvent.id);
            //updateEvent(calEvent.id, calEvent.start, calEvent.end);

        },
        eventResize: function (calEvent, $event) {
            //var $dialogContent = $("#event_edit_container");
            //var idField = $dialogContent.find("input[name='id_Dia']").val(calEvent.id);
            //var startField = $dialogContent.find("select[name='start']").val(calEvent.start);
            //var endField = $dialogContent.find("select[name='end']").val(calEvent.end);
            //var titleField = $dialogContent.find("input[name='title']").val(calEvent.title);
            //var bodyField = $dialogContent.find("textarea[name='body']");
            //var project = $dialogContent.find("select[name='project']").val(calEvent.project);
            //bodyField.val(calEvent.body);
            //idField.val(calEvent.id);
            //updateEvent(calEvent.id, calEvent.start, calEvent.end);
        },
        eventClick: function (calEvent, $event) {

            if (calEvent.readOnly) {
                return;
            }

            var $dialogContent = $("#event_edit_container2");
            resetForm($dialogContent);
            var startField = $dialogContent.find("select[name='start']").val(calEvent.start);
            var endField = $dialogContent.find("select[name='end']").val(calEvent.end);
            var titleField = $dialogContent.find("input[name='title']").val(calEvent.title);
            var bodyField = $dialogContent.find("textarea[name='body']");
            bodyField.val(calEvent.body);

            $dialogContent.dialog({
                modal: true,
                title: "Edit - " + calEvent.title,
                close: function () {
                    $dialogContent.dialog("destroy");
                    $dialogContent.hide();
                    $('#calendar').weekCalendar("removeUnsavedEvents");
                },
                buttons: {
                    save: function () {

                        calEvent.start = new Date(startField.val());
                        calEvent.end = new Date(endField.val());
                        calEvent.title = titleField.val();
                        calEvent.body = bodyField.val();

                        $calendar.weekCalendar("updateEvent", calEvent);
                        $dialogContent.dialog("close");
                    },
                    "delete": function () {
                        $calendar.weekCalendar("removeEvent", calEvent.id);
                        $dialogContent.dialog("close");
                    },
                    cancel: function () {
                        $dialogContent.dialog("close");
                    }
                }
            }).show();

            var startField = $dialogContent.find("select[name='start']").val(calEvent.start);
            var endField = $dialogContent.find("select[name='end']").val(calEvent.end);
            $dialogContent.find(".date_holder").text($calendar.weekCalendar("formatDate", calEvent.start));
            setupStartAndEndTimeFields(startField, endField, calEvent, $calendar.weekCalendar("getTimeslotTimes", calEvent.start));
            $(window).resize().resize();
        },
        eventMouseover: function (calEvent, $event) {

        },
        eventMouseout: function (calEvent, $event) {

        },
        noEvents: function () {
            swal("¡Espera!", "Sin Registros!", "info", {
                button: "OK",
            });
        }
    });
}

function updateEvent(_id, inicio, fin) {
    var id = 0;
    var jData = {
        data: []
    };
    jData["data"].push({ 'id': _id, 'start': inicio, 'end': fin });
    var lm = JSON.stringify(jData);
    $.ajax({
        type: "POST",
        url: 'actualizarFecha',
        data: lm,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            swal("¡Exito!", "Movido Con Exito!", "success", {
                button: "OK",
            });
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            swal("¡Error!", "Error con la actualizacion de este evento!", "error", {
                button: "OK",
            });
        }
    });
    return id;
}

function updateEventC(_id, inicio, fin, titulo, descripcion, proyecto, empleado) {
    var id = 0;
    var jData = {
        data: []
    };
    jData["data"].push({ 'id': _id, 'start': inicio, 'end': fin, 'title': titulo, 'body': descripcion, 'proyecto': proyecto, 'empleado': 0 });
    var lm = JSON.stringify(jData);
    $.ajax({
        type: "POST",
        url: 'actualizarEvento',
        data: lm,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data > 0) {
                id = data;
                swal("¡Exito!", "Evento Guardado Con Exito!", "success", {
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
    return id;
}

function eliminarEvent(_id) {
    var _d = { id: _id };
    var r = 0;
    $.ajax({
        type: "POST",
        url: 'borrarEvento',
        data: JSON.stringify(_d),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            r = data;
            swal("¡Exito!", "Borrado Con Exito!", "success", {
                button: "OK",
            });
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            r = 404;
            swal("¡Error!", "Error con el borrado de este evento!", "error", {
                button: "OK",
            });
        }
    });
    return r;
}

function contarDias(fromDate, toDate, interval) {
    var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
    fromDate = new Date(fromDate);
    toDate = new Date(toDate);
    var timediff = toDate - fromDate;
    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "years": return toDate.getFullYear() - fromDate.getFullYear();
        case "months": return (
            (toDate.getFullYear() * 12 + toDate.getMonth())
            -
            (fromDate.getFullYear() * 12 + fromDate.getMonth())
        );
        case "weeks": return Math.floor(timediff / week);
        case "days": return Math.floor(timediff / day);
        case "hours": return Math.floor(timediff / hour);
        case "minutes": return Math.floor(timediff / minute);
        case "seconds": return Math.floor(timediff / second);
        default: return undefined;
    }
}

function formatoFecha(fecha) {
    var datearray = fecha.split("/");
    var _fecha = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
    return _fecha;
}

function cd(currentDate, days) {
    var date = new Date(currentDate);
    date.setDate(date.getDate() + days);
    return date;
}

function sethoraI(currentDate) {
    var date = new Date(currentDate);
    date.setHours(8);
    return date;
}

function sethoraF(currentDate) {
    var date = new Date(currentDate);
    date.setHours(19);
    return date;
}

function getDates(startDate, stopDate) {
    var dateArray = new Array();
    var currentDate = startDate;
    while (new Date(currentDate) <= new Date(stopDate)) {
        dateArray.push(new Date(currentDate));
        currentDate = cd(currentDate, 1);
    }
    return dateArray;
}


