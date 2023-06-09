//Sweetalert

async function SWAlert(titulo, texto, botonOk, iconoOK) {
    return new Promise(resolve => {
        Swal.fire({
            title: '<strong>' + titulo + '</strong>',
            html: texto,
            confirmButtonText:
                '<i class="' + iconoOK + '"></i> ' + botonOk,
            customClass: {
                container: 'red-bg'
            }
        }).then((result) => {
            resolve(true);
        });
    });
}
function SWAlertVoid(titulo, texto, botonOk, iconoOK) {
    Swal.fire({
        title: '<strong>' + titulo + '</strong>',
        html: texto,
        confirmButtonText:
            '<i class="' + iconoOK + '"></i> ' + botonOk,
    })
}

function OpenWindow(path) {
    window.open(path, "_blank");
    return true;
}

async function SWConfirm(titulo, texto, botonSi, botonNo) {
    return new Promise(resolve => {
        Swal.fire({
            title: titulo,
            text: texto,

            showCancelButton: true,
            confirmButtonColor: "#455a64",
            cancelButtonColor: '#d33',
            cancelButtonText: botonNo,
            //closeOnConfirm: false,
            confirmButtonText: botonSi,
        }).then((result) => {
            if (result.value === true)
                resolve(result.value);
            else
                resolve(false);
        })
    });
}