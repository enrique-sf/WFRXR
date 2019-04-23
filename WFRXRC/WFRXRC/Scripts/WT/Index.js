$(document).ready(function () {
    $("#tabs").tabs();
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
});