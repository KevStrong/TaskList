$(document).ready(function () {

    // On click of checkbox, add or remove class to score out the task decription
    $("input[type=checkbox]").click(function () {
        var checkChecked = false;
        var ElementId = $(this).attr("checkfor");
        var pretext = document.getElementById(ElementId);

        var attr = $(pretext).attr('class');
        if (typeof attr !== typeof undefined && attr !== false) {
            $(pretext).removeAttr("class"); 
            checkChecked = false;

        }
        else {
            $(pretext).attr("class", "strikeout");
            checkChecked = true;

        }
        //  Call controlle to update static list values
        $.ajax({
            type: "POST",
            url: "/TaskList/Update",
            content: "application/json; charset=utf-8",
            dataType: "json",
            data: {id : ElementId, markedComplete : checkChecked},
            success: function (result) {
                if (result.success == true)
                    window.location = "Index.cshtml";
            }
        });
    });

    // when page is ready, check if any boxes are already checked and apply class for strikeout
    $('input[type=checkbox]').each(function () {
        var ElementId = $(this).attr("checkfor");
        var pretext = document.getElementById(ElementId);
        if (this.checked) {
            $(pretext).attr("class", "strikeout");
        }
    });
});