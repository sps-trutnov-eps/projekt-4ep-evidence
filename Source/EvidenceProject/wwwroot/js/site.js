$(document).ready(function () {
    plynulyPrechodMeziStrankami();
    nastaveniStylu();
    spustitScript();
});
function plynulyPrechodMeziStrankami(){
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
                $("header").replaceWith(stranka.filter("header"));
                $("main").replaceWith(stranka.filter("main"));
                $("title").replaceWith(stranka.filter("title"));
                history.pushState({"html":html}, "", link);
                spustitScript();
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
    spustitScript();
};

function spustitScript(){
    let lokace = $(location).attr("pathname");
    if(lokace == "/project/create") {
        nazvySouboru();
    } else if (lokace == "/users/login"){
        loginText();
    }
}

function nazvySouboru(){
    console.log("workin");
    const fileSelector = document.getElementById('photo');
    fileSelector.addEventListener('change', (event) => {
        const fileList = event.target.files;
        console.log(fileList);
        document.getElementById("nazvy").innerHTML = "";
        for (let i = 0; i < fileList.length; i++) {
            /*console.log(fileList[i].name);*/
            ted = document.getElementById("nazvy").innerText;
            document.getElementById("nazvy").innerHTML = ted + ", " + fileList[i].name;

        }
    });
}

function loginText() {
    console.log('logging text');
    $("#login form").submit(function(event) {
        event.preventDefault();
        let formular = $(this);
        $('#hlaska').remove();
        $('.myLogin').after('<p id="hlaska">logging in...</p>');
        $.ajax({
            type: formular.attr("method"),
            url: formular.attr("action"),
            data: formular.serialize(),
            success: function(data)
            {
                if(!data.includes("<!DOCTYPE html>")){
                    $('#hlaska').remove();
                    $('.myLogin').after(`<p id="hlaska">${data}</p>`);
                } else {
                    let stranka = $($.parseHTML(data));
                    $("header").replaceWith(stranka.filter("header"));
                    $("main").replaceWith(stranka.filter("main"));
                    $("title").replaceWith(stranka.filter("title"));
                    history.pushState({"html":data}, "", "/");
                    spustitScript();
                }
            }
        });
    });
}

function menitHeslo() {
    alert("zatím nejde");
}

$(document).on("click", ".mode", function(event){
    let style = event.target.id
    localStorage.setItem("mode", style);
    nastaveniStylu();
})

function nastaveniStylu() {
    let style = localStorage.getItem("mode")
    if (style == null) {
        document.getElementsByTagName('body')[0].innerHTML += '<link rel="stylesheet" href="/css/site.css" asp-append-version="true" />';
    }
    else {
        document.getElementsByTagName('body')[0].innerHTML += '<link rel="stylesheet" href="/css/' + style + '.css" asp-append-version="true" />';
    }
}