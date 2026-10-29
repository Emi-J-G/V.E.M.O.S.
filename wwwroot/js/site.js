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

document.addEventListener('DOMContentLoaded', function () {
    const modalLogin = document.getElementById('login-modal');
    const modalRegister = document.getElementById('register-modal');
    const loginTriggers = document.querySelectorAll('[data-auth-modal="login-modal"]');
    const registerTriggers = document.querySelectorAll('[data-auth-modal="register-modal"]');
    const closeButtons = document.querySelectorAll('.auth-modal-close');
    const switchButtons = document.querySelectorAll('.auth-switch-trigger');

    function closeModal(modal) {
        if (!modal) return;
        modal.classList.remove('is-open');
        modal.setAttribute('aria-hidden', 'true');
    }

    function closeAllModals() {
        closeModal(modalLogin);
        closeModal(modalRegister);
    }

    function openModal(modal) {
        if (!modal) return;
        closeAllModals();
        modal.classList.add('is-open');
        modal.setAttribute('aria-hidden', 'false');
        const firstInput = modal.querySelector('input:not([type="hidden"]), select');
        if (firstInput) {
            setTimeout(() => firstInput.focus(), 40);
        }
    }

    loginTriggers.forEach(function (trigger) {
        trigger.addEventListener('click', function () {
            openModal(modalLogin);
        });
    });

    registerTriggers.forEach(function (trigger) {
        trigger.addEventListener('click', function () {
            openModal(modalRegister);
        });
    });

    closeButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            const modal = button.closest('.auth-modal-backdrop');
            closeModal(modal);
        });
    });

    switchButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            const target = button.getAttribute('data-auth-modal');
            if (target === 'login-modal') {
                openModal(modalLogin);
            }
            if (target === 'register-modal') {
                openModal(modalRegister);
            }
        });
    });

    [modalLogin, modalRegister].forEach(function (modal) {
        if (!modal) return;

        modal.addEventListener('click', function (event) {
            if (event.target === modal) {
                closeModal(modal);
            }
        });
    });

    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
            closeAllModals();
        }
    });

    const hasMessage = document.querySelector('.auth-banner-message');
    if (hasMessage) {
        openModal(modalLogin);
    }
});