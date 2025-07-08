document.addEventListener('DOMContentLoaded', function () {
    // Function to open a modal
    function openModal(modal) {
        if (modal) {
            modal.classList.add('is-visible');
        }
    }

    // Function to close a modal
    function closeModal(modal) {
        if (modal) {
            modal.classList.remove('is-visible');
        }
    }

    // Event listener for all modal triggers
    document.querySelectorAll('[data-modal-trigger]').forEach(trigger => {
        trigger.addEventListener('click', () => {
            const modalId = trigger.getAttribute('data-modal-target');
            const modal = document.getElementById(modalId);
            openModal(modal);
        });
    });

    // Event listener for all modal close buttons
    document.querySelectorAll('[data-modal-close]').forEach(closer => {
        closer.addEventListener('click', () => {
            const modal = closer.closest('.modal-container');
            closeModal(modal);
        });
    });

    // Event listener to close modal by clicking the overlay
    document.querySelectorAll('.modal-overlay').forEach(overlay => {
        overlay.addEventListener('click', () => {
            const modal = overlay.closest('.modal-container');
            closeModal(modal);
        });
    });

    // Event listener for the 'Escape' key
    document.addEventListener('keydown', (event) => {
        if (event.key === 'Escape') {
            const visibleModal = document.querySelector('.modal-container.is-visible');
            closeModal(visibleModal);
        }
    });
});