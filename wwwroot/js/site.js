function isValidName(value) {
    return /^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+$/.test(value);
}

function isValidEmail(value) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function validateRegisterForm() {
    const nombreUsuario = document.getElementById("nombreUsuario").value.trim();
    const nombre = document.getElementById("nombre").value.trim();
    const apellido = document.getElementById("apellido").value.trim();
    const contrasena = document.getElementById("contrasena").value.trim();
    const email = document.getElementById("email").value.trim();
    const tipo = document.getElementById("tipo").value;

    if (!nombreUsuario || !nombre || !apellido || !contrasena || !email || !tipo) {
        alert("Por favor, complete todos los campos.");
        return false;
    }

    if (nombreUsuario.length < 3 || nombreUsuario.length > 20) {
        alert("El nombre de usuario debe tener entre 3 y 20 caracteres.");
        return false;
    }

    if (nombre.length < 3 || nombre.length > 20) {
        alert("El nombre debe tener entre 3 y 20 caracteres.");
        return false;
    }

    if (!isValidName(nombre)) {
        alert("El nombre no puede contener espacios ni caracteres especiales.");
        return false;
    }

    if (apellido.length < 3 || apellido.length > 20) {
        alert("El apellido debe tener entre 3 y 20 caracteres.");
        return false;
    }

    if (!isValidName(apellido)) {
        alert("El apellido no puede contener espacios ni caracteres especiales.");
        return false;
    }

    if (contrasena.length < 5 || contrasena.length > 15) {
        alert("La contraseña debe tener entre 5 y 15 caracteres.");
        return false;
    }

    if (!isValidEmail(email)) {
        alert("Ingrese un mail válido.");
        return false;
    }

    return true;
}