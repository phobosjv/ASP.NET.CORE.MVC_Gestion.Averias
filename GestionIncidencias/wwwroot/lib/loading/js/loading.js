var modal, loading;
function ShowProgress() {
    modal = document.createElement("DIV");
    modal.className = "modal";
    document.body.appendChild(modal);
    loading = document.getElementsByClassName("loading")[0];
    loading.style.display = "block";
    //var top = Math.max(window.innerHeight / 2 - loading.offsetHeight / 2, 0);
    //var left = Math.max(window.innerWidth / 2 - loading.offsetWidth / 2, 0);
    const urlParams = new URLSearchParams(window.location.search);
    const mensaje = urlParams.get("mensaje");
    if (mensaje !== null) {
        var top = Math.max(window.innerHeight - loading.offsetHeight - 100, 0);
    }
    else {
        var top = Math.max(window.innerHeight - loading.offsetHeight, 0);
    }
    var left = Math.max(window.innerWidth - loading.offsetWidth, 0);
    loading.style.top = top + "px";
    loading.style.left = left + "px";
};
ShowProgress();

window.onload = function () {
    setTimeout(function () {
        document.body.removeChild(modal);
        loading.style.display = "none";
    }, 100); //Poner a 1000 si se quiere aumentar el tiempo en un segundo.
};


