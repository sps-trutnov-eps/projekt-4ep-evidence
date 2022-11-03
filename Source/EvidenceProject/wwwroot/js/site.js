$(document).ready(function () {
    plynulyPrechodMeziStrankami();
    spustitScript();
    nastaveniStylu();
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
        loginText();
    }
    /*} else if (lokace == "/") {

    }*/
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

function loginText(e) {
    $('.myLogin').on('click', () => {
        $('.myLogin').after('<p>logging in...</p>');
    });
    console.log('logging text');
    /*e.preventDefault();*/
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

async function search(query) {
    let res = await fetch("/search", {
        body: JSON.stringify({
            text: query,
        }),
        headers: {
            'Accept': 'application/json',
            'Content-type': 'application/json',
        },
        method: "POST",
    })
    let data = await res.json();
}
