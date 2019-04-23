$(document).ready(function () {
    $('.dropdown-trigger').dropdown();
    $('#btnCon').on("click", function (e) {
        //recupero el valor
        var Contacto = $('#contacto').val();
        if (Contacto !== "") {
            //lo agrego a un input hide para posteriormente mandarlo al controller
            $("#divContactos").append('<input id="contactos" name="contactos" value=' + Contacto + ' type="hidden"/>');
            //Reviso si tiene la clase sc, que significa que es el renglon para indicar que no tiene contactos
            $("#ulDdl > li").each(function () {
                var li = $(this).hasClass("sc");
                if (li) {
                    $(this).remove();
                }
            });
            //lo agrego a una lista para que el usuario vea los que tiene capturados
            $("#ulDdl").append('<li class=""><a href="#!">' + Contacto + '</a></li>');
        }
    });
});