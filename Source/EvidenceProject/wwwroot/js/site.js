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
        $("main").html("<div>načítám data...</div>");
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
    if (lokace == "/project/create") {
        nazvySouboru();
    } else if (lokace == "/users/login") {
        prihlaseniRegistraceText("#login form","/",'Přihlašování ...');
    } else if (lokace == "/users/register") {
        prihlaseniRegistraceText("#register form", "/users/login", 'Registrování ...');
    }
}

function nazvySouboru(){
    const fileSelector = document.getElementById('photo');
    fileSelector.addEventListener('change', (event) => {
        const fileList = event.target.files;
        document.getElementById("nazvy").innerHTML = "";
        for (let i = 0; i < fileList.length; i++) {
            ted = document.getElementById("nazvy").innerText;
            document.getElementById("nazvy").innerHTML = ted + ", " + fileList[i].name;

        }
    });
}

function prihlaseniRegistraceText(selektor, presmerovani, text) {
    $(selektor).submit(function(event) {
        event.preventDefault();
        let formular = $(this);
        $('#hlaska').remove();
        $(selektor).after(`<p id="hlaska">${text}</p>`);
        $.ajax({
            type: formular.attr("method"),
            url: formular.attr("action"),
            data: formular.serialize(),
            success: function(data)
            {
                if(!data.includes("<!DOCTYPE html>")){
                    $('#hlaska').remove();
                    $(selektor).after(`<p id="hlaska">${data}</p>`);
                } else {
                    let stranka = $($.parseHTML(data));
                    $("header").replaceWith(stranka.filter("header"));
                    $("main").replaceWith(stranka.filter("main"));
                    $("title").replaceWith(stranka.filter("title"));
                    history.pushState({"html":data}, "", presmerovani);
                    spustitScript();
                }
            }
        });
    });
}

function zmenitHeslo() {
    window.open('https://youtu.be/dQw4w9WgXcQ', '_blank');
    alert('Byl jsi napálen.');
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
        /*document.getElementsByTagName('body')[0].innerHTML += '<link rel="stylesheet" href="/css/site.css" asp-append-version="true" />';*/
        document.getElementsByTagName('body')[0].innerHTML += '<link rel="stylesheet" href="/css/' + style + '.css" asp-append-version="true" />';
    }
}

async function search(query) {
    if (query != ""){
        $.ajax({
            type : "POST",
            url : "search",
            data: JSON.stringify(query),
            headers: {
                "Content-Type": "application/json"
            },
            dataType: "json",
            success : function(data){
                $("main").html(`<div><h2>Výsledky vyhledávání pro hledaný výraz: "${query}"</h2><div id="vysledky">${data}</div></div>`);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("main").html(`<div>${jqXHR.status} ${errorThrown}</div>`);
            }
        });
    }
}

async function login() {
    let username = Document.getElementById("username")
    let password = Document.getElementById("password")
    let res = await fetch("/login", {
        body: JSON.stringify({
            username: username,
            password: password,
        }),
        headers: {
            'Accept': 'application/json',
            'Content-type': 'application/json',
        },
        method: "POST",
    })
    let data = await res.json();
}

function menitHeslo() {
    alert("zatím nejde");
}


