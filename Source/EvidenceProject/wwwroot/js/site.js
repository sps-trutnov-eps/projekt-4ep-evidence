console.log("workin");
const fileSelector = document.getElementById('photo');
//fileSelector.addEventListener('change', (event) => {
//    const fileList = event.target.files;
//    console.log(fileList);
//    document.getElementById("nazvy").innerHTML = "";
//    for (let i = 0; i < fileList.length; i++) {
//        /*console.log(fileList[i].name);*/
//        ted = document.getElementById("nazvy").innerText;
//        document.getElementById("nazvy").innerHTML = ted + " " + fileList[i].name;
﻿$(document).ready(function () {
    PlynulyPrechodMeziStrankami();
});

//    }
//});
function PlynulyPrechodMeziStrankami(){
    history.replaceState({"html":$("html").prop("outerHTML")}, "", $(location).attr("pathname"));

console.log("xd");
$(document).ready(function () {
    loginText();
    console.log('ready');
});

function loginText(e) {

    $('.myLogin').on('click', () => {
        $('.myLogin').after('<p>logging in...</p>');
    });
    console.log('logging text');
    e.preventDefault();
};
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
