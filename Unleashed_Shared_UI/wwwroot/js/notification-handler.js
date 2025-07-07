document.addEventListener('DOMContentLoaded', function () {
    const container = document.getElementById('notification-container');
    if (!container) return;

    const bellButton = document.getElementById('notification-bell-button');
    const closeButton = document.getElementById('notification-close-button');
    const popup = document.getElementById('notification-popup');
    const list = document.getElementById('notification-list');

    // --- Event Listeners ---
    bellButton.addEventListener('click', togglePopup);
    closeButton.addEventListener('click', () => popup.classList.remove('show'));

    // Handle outside clicks
    document.addEventListener('click', (e) => {
        if (!container.contains(e.target)) {
            popup.classList.remove('show');
        }
    });

    // Handle clicks on action buttons inside the list
    list.addEventListener('click', (e) => {
        const deleteButton = e.target.closest('[data-action="delete"]');
        if (deleteButton) {
            const item = deleteButton.closest('[data-notification-id]');
            const notificationId = item.dataset.notificationId;
            deleteNotification(notificationId);
        }
    });

    // --- Functions ---
    async function togglePopup() {
        const isOpening = !popup.classList.contains('show');
        popup.classList.toggle('show');

        if (isOpening && bellButton.classList.contains('has-unread')) {
            // Mark all as read when opening
            try {
                await fetch('/api/notifications/mark-all-as-read', { method: 'POST' });
                bellButton.classList.remove('has-unread');
            } catch (error) {
                console.error("Error marking all as read:", error);
            }
        }
    }

    async function deleteNotification(id) {
        try {
            const response = await fetch(`/api/notifications/${id}`, { method: 'DELETE' });
            if (response.ok) {
                // Optimistic UI update: remove the item from the DOM immediately
                const itemToRemove = list.querySelector(`[data-notification-id="${id}"]`);
                if (itemToRemove) {
                    itemToRemove.remove();
                }
            }
        } catch (error) {
            console.error(`Error deleting notification ${id}:`, error);
        }
    }
});