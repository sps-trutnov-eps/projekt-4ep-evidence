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

//    }
//});

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