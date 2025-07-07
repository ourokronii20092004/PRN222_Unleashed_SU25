document.addEventListener('DOMContentLoaded', function () {
    const menuContainer = document.querySelector('.menu-container');
    if (!menuContainer) return;

    const toggleButton = menuContainer.querySelector('.menu-trigger-button');
    const dropdownMenu = menuContainer.querySelector('.menu-dropdown');

    toggleButton.addEventListener('click', function (event) {
        event.stopPropagation(); // Prevent the 'document' click from firing immediately
        dropdownMenu.classList.toggle('show');
    });

    // Add a global click listener to close the menu if the user clicks elsewhere
    document.addEventListener('click', function (event) {
        // If the dropdown is not visible, do nothing
        if (!dropdownMenu.classList.contains('show')) {
            return;
        }

        // If the click was inside the menu container, do nothing
        if (menuContainer.contains(event.target)) {
            return;
        }

        // Otherwise, close the menu
        dropdownMenu.classList.remove('show');
    });
});