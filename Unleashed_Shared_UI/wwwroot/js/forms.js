document.addEventListener('DOMContentLoaded', function () {
    // Helper function to set up a password toggle
    function setupPasswordToggle(buttonId, inputId, textId) {
        const toggleButton = document.getElementById(buttonId);
        if (!toggleButton) return;

        const passwordInput = document.getElementById(inputId);
        const toggleText = document.getElementById(textId);
        const toggleIcon = toggleButton.querySelector('i');

        toggleButton.addEventListener('click', function () {
            const isPassword = passwordInput.getAttribute('type') === 'password';
            if (isPassword) {
                passwordInput.setAttribute('type', 'text');
                toggleText.textContent = 'Hide';
                toggleIcon.classList.remove('fa-eye');
                toggleIcon.classList.add('fa-eye-slash');
            } else {
                passwordInput.setAttribute('type', 'password');
                toggleText.textContent = 'Show';
                toggleIcon.classList.remove('fa-eye-slash');
                toggleIcon.classList.add('fa-eye');
            }
        });
    }

    // Setup for the Login form password field
    setupPasswordToggle('togglePasswordBtn', 'Password', 'togglePasswordText');

    // Setup for the Register form's confirm password field
    setupPasswordToggle('toggleConfirmPasswordBtn', 'ConfirmPassword', 'toggleConfirmPasswordText');
});