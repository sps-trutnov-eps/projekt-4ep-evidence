$(document).ready(function () {
    plynulyPrechodMeziStrankami();
    nastaveniStylu();
    spustitScript();
})

const xhr = new XMLHttpRequest();
const domp = new DOMParser();
const xmls = new XMLSerializer();

function plynulyPrechodMeziStrankami(){
    history.replaceState({"html":xmls.serializeToString(document)}, "", location.href);
    
    $(document).on("click", ".odkaz", function () {
        let link = $(this).attr('href');

        $("main").html(`<div>Načítám data...</div>`);

        xhr.open('GET', link, true);
        xhr.responseType = "text";
        xhr.onload = () => {
            if (xhr.status >= 200 && xhr.status < 400) {
                const text = xhr.responseText
                const doc = domp.parseFromString(text, "text/html")

                document.querySelector('body').innerHTML = doc.querySelector('body').innerHTML;
                document.querySelector('title').innerHTML = doc.querySelector('title').innerHTML;

                history.pushState({"html": text }, "", xhr.responseURL);

                spustitScript();
            } else {
                // errror
                $("main").html(`<div>Chyba: ${xhr.status} ${xhr.statusText}</div>`);
            }
        };
        xhr.send();

        return false;
    }); 
}

window.onpopstate = function(e){
    if (e.state == null) return;
    const text = e.state.html
    const doc = domp.parseFromString(text, "text/html")

    document.querySelector('body').innerHTML = doc.querySelector('body').innerHTML;
    document.querySelector('title').innerHTML = doc.querySelector('title').innerHTML;

    spustitScript();
}

function spustitScript(){
    let lokace = $(location).attr("pathname");

    search();

    if (lokace == "/project/create") {
        nazvySouboru();
    } else if (lokace == "/users/login") {
        prihlaseniRegistraceFormular("#login form",'Přihlašování ...');
    } else if (lokace == "/users/register") {
        prihlaseniRegistraceFormular("#register form", 'Registrování ...');
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

function prihlaseniRegistraceFormular(selektor, text) {
    $(selektor).submit(function(event) {
        event.preventDefault();

        let formular = $(this);

        $("main").html(`<div>${text}</div>`);

        xhr.open("POST", formular.attr("action"), true);
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        xhr.responseType = "text";
        xhr.onload = () => {
            if (xhr.status === 200) {
                const text = xhr.responseText
                const doc = domp.parseFromString(text, "text/html")

                document.querySelector('body').innerHTML = doc.querySelector('body').innerHTML;
                document.querySelector('title').innerHTML = doc.querySelector('title').innerHTML;

                if (xhr.responseURL != location.href){
                    history.pushState({"html": text }, "", xhr.responseURL);
                }
                spustitScript();
            } else {
                // errror
                $("main").html(`<div>Chyba: ${xhr.status} ${xhr.statusText}</div>`);
            }
        };
        xhr.send(formular.serialize());
    });
}

$(document).on("click", ".mode", function(event){
    let style = event.target.id
    localStorage.setItem("mode", style);
    nastaveniStylu();
})

function nastaveniStylu() {
    let style = localStorage.getItem("mode")
    if (style == null) {
        document.getElementsByTagName('head')[0].innerHTML += '<link rel="stylesheet" href="/css/site.css" asp-append-version="true" />';
    }
    else {
        /*document.getElementsByTagName('body')[0].innerHTML += '<link rel="stylesheet" href="/css/site.css" asp-append-version="true" />';*/
        document.getElementsByTagName('head')[0].innerHTML += '<link rel="stylesheet" href="/css/' + style + '.css" asp-append-version="true" />';
    }
}

function search() {
    $('#searchform').submit(function(event) {
        event.preventDefault();
        let formular = $(this);
        let hledanyVyraz = formular.serializeArray()[0].value.trim();
        if (hledanyVyraz){
            $.ajax({
                type: formular.attr("method"),
                url: formular.attr("action"),
                data: formular.serialize(),
                success : function(data){
                    $("main").html(`<div><h2>Výsledky vyhledávání pro hledaný výraz: "${hledanyVyraz}"</h2><div id="vysledky">${data}</div></div>`);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("main").html(`<div>${jqXHR.status} ${errorThrown}</div>`);
                }
            });
        }
    });
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
let jedna = 0

function veci(e, cojeto) {
    let value = e.target.value;
    let tech = document.getElementsByClassName(value);
    let more = "";
    let vole = "";
    try {
        vole = Array.from(document.getElementById(cojeto).getElementsByTagName("option")).map(e=> e.innerText);
        console.log(vole);
    }
    catch {}
    try {
        document.getElementById(cojeto).remove();
    }
    catch {}
    if (cojeto == "tech") {
        more += '<select name = "' + cojeto + '"' + 'id = "' + cojeto +  '" multiple size = ' + tech.length + ">";
        if (jedna != 0) {
            for (let i = 0; i < vole.length; i ++) {    
                more += '<option value = "' + vole[i] + '">' + vole[i] +'</option>'
                console.log(vole[i] + "jjjjj");
            }
        }
        jedna += 1;
        document.getElementById("technology").value = "";
    }
    else {
        more += '<select name = "' + cojeto + '"' + 'id = "' + cojeto + '">';
    }
    for(let i = 0; i < tech.length; i++ ) {
        more += '<option value = "' + tech[i].innerHTML + '">' + tech[i].innerHTML +'</option>'
    }
    more += '</select>'
    $( e.target ).after( more );
}
