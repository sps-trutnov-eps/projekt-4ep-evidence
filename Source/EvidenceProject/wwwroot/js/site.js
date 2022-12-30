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
        if (hledanyVyraz) {
            $.ajax({
                type: formular.attr("method"),
                url: formular.attr("action"),
                data: formular.serialize(),
                success : function(data){
                    let projektyHtml = "";
                    if (typeof (data) == "object" & data.length >= 1) {
                        projektyHtml += `<div class="projects">`;

                        projektyHtml += `<div class="project"><p>Název</p ><p>Popis</p><p>Technologie</p><p>Stav</p><p>Typ</p><p>Úspěchy</p><p>Detail</p></div >`;

                        for (let i = 0; i < data.length; i++) {
                            let _project = data[i];

                            let _tech = "";
                            let _achiv = "";

                            for (let y = 0; y < _project.achiv.length; y++) {
                                _achiv += _project.achiv[y] + (y == _project.achiv.length - 1 ? "" : ", ");
                            }
                            for (let y = 0; y < _project.tech.name.length; y++) {
                                _tech += `<span class="tech" style="color: ${_project.tech.color[y]}">${_project.tech.name[y]}</span>`;
                            }

                            projektyHtml += `<div class="project">
                                 <p><b>${_project.name}</b></p>
                                    <p><span class="desc">Popis:</span> ${_project.desc}</p>
                                    <p><span class="desc">technologie:</span>
                                            ${_tech}
                                    </p>
                                    <p>
                                        <span class="desc">Stav:</span>                
                                        <span style="color: ${_project.state.color}">${_project.state.name}</span>
                                    </p>
                                     <p>
                                         <span class="desc">Typ:</span> 
                                         <span style="color: ${_project.type.color}">${_project.type.name}</span>
                                    </p>

                                    <p><span class="desc">Úspěchy:</span> ${_achiv}</p>
                                    <a href="/project/${_project.id}"><span class="desc">Detail</span><img class="detail-icon" src="${window.location.origin}/icons/detail.png" alt="Detail"></a>
                            </div>`;
                        }
                        projektyHtml += "</div>";
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



