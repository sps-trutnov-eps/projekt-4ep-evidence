let jedna = 0;
let array = [];
let iii = 0;

function veci(e, data) {
    let value = e.target.value;
    let tech = document.getElementsByClassName(value);
    let more = "";
    let vole = "";
    if (data == "tech" && array.includes(value)) {
    document.getElementById("technology").value = "";
    return;
    }
    try {
        vole = Array.from(document.getElementById(data).getElementsByTagName("option")).map(e=> e.innerText);
    }
    catch {}
    try {
        document.getElementById(data).remove();
    }
    catch {}
    if (data == "tech") {
        more += '<select name = "' + data + '"' + 'id = "' + data +  '" multiple size = ' + tech.length + ">";
        if (jedna != 0) {
            for (let i = 0; i < vole.length; i ++) {    
                more += '<option value = "' + vole[i] + '">' + vole[i] +'</option>'
            }
        }
        jedna += 1;
    }
    else {
        more += '<select name = "' + data + '"' + 'id = "' + data + '">';
    }
    for(let i = 0; i < tech.length; i++ ) {
        more += '<option value = "' + tech[i].innerHTML + '">' + tech[i].innerHTML +'</option>';
    }
    more += '</select>';
    $(e.target).after(more);
    if (data == "tech") {
        array[iii] = value;
        iii++;
    }
    
    document.getElementById("technology").value = "";
}


function removeFile(fileInput) {
    const elements = document.getElementsByClassName(fileInput);
    while (elements.length > 0) {
        elements[0].parentNode.removeChild(elements[0]);
    }
}

function addNewAssignee(e, attr) {
    var closeWithNewInput = `<span class="${attr}" onclick="removeAssignee(${attr})">Odebrat</span>`;
    closeWithNewInput += `<input name="assignees" class="${attr + 1}" type="text" placeholder="..." onchange="addNewAssignee(event,${attr + 1})" list="users" />`;
    $(e.target).after(closeWithNewInput);
}

function removeAssignee(attr) {
    removeFile(attr);
}
