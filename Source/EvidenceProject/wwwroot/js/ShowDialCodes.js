$("#Cat").change(() => {
    $("show-hidden").css("display", "none");

    let name = document.getElementById("#Cat").value;

    $(`show-${name}`).css("display", "block");
});