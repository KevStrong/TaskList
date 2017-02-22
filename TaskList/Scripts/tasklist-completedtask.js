$(document).ready(function () {
    $("input[type=checkbox]").click(function () {
        var ElementId = $(this).attr("checkfor");
        var pretext = document.getElementById(ElementId);

        var attr = $(pretext).attr('class');
        if (typeof attr !== typeof undefined && attr !== false) {
            $(pretext).removeAttr("class"); 
        }
        else {
            $(pretext).attr("class", "strikeout");
        }
    });
});