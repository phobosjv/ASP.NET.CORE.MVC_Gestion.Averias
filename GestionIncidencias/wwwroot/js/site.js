const mostrarEmergenteBtn = document.getElementById("mostrarEmergente");
const emergente = document.getElementById("emergente");
const aceptarBtn = document.getElementById("aceptar");
const cancelarBtn = document.getElementById("cancelar");

mostrarEmergenteBtn.addEventListener("click", () => {
    emergente.style.display = "block";
});

cancelarBtn.addEventListener("click", () => {
    emergente.style.display = "none"; // Oculta el emergente si se cancela
});
