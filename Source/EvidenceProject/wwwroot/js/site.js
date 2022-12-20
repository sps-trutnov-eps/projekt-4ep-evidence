$(document).ready(function () {
    plynulyPrechodMeziStrankami();
    nastaveniStylu();
    search();
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
                for (const script of document.querySelectorAll(".module-script")) {
                    let el = document.createElement("script");
                    el.src = script.src;
                    let mod_name = new RegExp("\/js\/(.*?)\\?v=").exec(el.src)[1];
                    console.log(`Loading ${mod_name} module.`);
                    el.remove();
                    document.body.appendChild(el);
                }
                history.pushState({"html": text }, "", xhr.responseURL);
                search();
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

    search();
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
                    let projektyHtml = "";
                    if(typeof(data) == Array & data.length > 1){
                        for(let projekt of data)  {
                            let obrazek= projekt.files.find(s => s.mimeType.includes("image"))
                            let nahled = `<img style="height: 100%;object-fit: cover;width: 100%;" src="/file/${obrazek.generatedFileName}"/>`;
                            let nadpis = `<a href="/project/${projekt.id}" class="odkaz"><h4>${projekt.name}</h4></a>`;
                            let spravce = `<div>Správce: ${projekt.projectManager}</div>`;
                            let popis = `<div>Popis: ${projekt.projectDescription}</div>`;
                            let stav = `<div style="margin-right:5px">${projekt.projectState.name}</div>`;
                            let typ = `<div style="margin-right:5px">${projekt.projectType.name}</div>`;
                            let technologie = "";
                            for(let tech of projekt.projectTechnology){
                                technologie += `<div style="margin-right:5px">${tech.name}</div>`;
                            }
                            let projektHtml = `<div style="display:flex"><div style="height:100px;width:100px;margin:10px">${nahled}</div><div><div>${nadpis}${spravce}${popis}</div><div style="display:flex">${stav}${typ}${technologie}</div></div></div>`;
                            projektyHtml += projektHtml;
                        }
                    } else {
                        projektyHtml = `<p>Nebyly nalezeny žádné výsledky :(</p>`;
                    }
                    $("main").html(`<div><h2>Výsledky vyhledávání pro hledaný výraz: "${hledanyVyraz}"</h2><div id="vysledky">${projektyHtml}</div></div>`);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("main").html(`<div>${jqXHR.status} ${errorThrown}</div>`);
                }
            });
        }
    });
}


// Unused
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
//////
