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

var lokace = $(location).attr("pathname");
if (lokace == "/users/login") {
    prihlaseniRegistraceFormular("#login form",'Přihlašování ...');
} else if (lokace == "/users/register") {
    prihlaseniRegistraceFormular("#register form", 'Registrování ...');
}