$(document).ready(function () {
    PlynulyPrechodMeziStrankami();
});

function PlynulyPrechodMeziStrankami(){
    history.replaceState({"html":$("html").prop("outerHTML")}, "", $(location).attr("pathname"));

    $(document).on("click", ".odkaz", function () {
        let link = $(this).attr('href');

        $("main").empty();
        $("main").html("<div>načítání dat...</div>");
    
        $.ajax({
            type : "GET",
            url : link,
            dataType: "html",
            success : function(html){
                let stranka = $($.parseHTML(html));
                $("main").replaceWith(stranka.filter("main"));
                $("title").replaceWith(stranka.filter("title"));
                history.pushState({"html":html}, "", link);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("main").html(`<div>${jqXHR.status} ${errorThrown}</div>`);
            }
        });
    
        return false;
    });
}
window.onpopstate = function(e){
    if (e.state == null) return;
    let stranka = $($.parseHTML(e.state.html));
    $("main").replaceWith(stranka.filter("main"));
    $("title").replaceWith(stranka.filter("title"));
};
