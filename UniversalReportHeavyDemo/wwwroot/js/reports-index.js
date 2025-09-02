// wwwroot/js/reports-index.js
document.addEventListener("DOMContentLoaded", function () {
    function restoreAccordionState(accordionId, storageKey) {
        const savedState = JSON.parse(localStorage.getItem(storageKey)) || {};
        Object.keys(savedState).forEach(id => {
            const element = document.getElementById(id);
            if (element && savedState[id]) {
                new bootstrap.Collapse(element, { toggle: true });
            }
        });
    }

    function trackAccordionState(accordionId, storageKey) {
        const accordion = document.getElementById(accordionId);
        if (!accordion) return;

        accordion.addEventListener("shown.bs.collapse", event => {
            const savedState = JSON.parse(localStorage.getItem(storageKey)) || {};
            savedState[event.target.id] = true;
            localStorage.setItem(storageKey, JSON.stringify(savedState));
        });

        accordion.addEventListener("hidden.bs.collapse", event => {
            const savedState = JSON.parse(localStorage.getItem(storageKey)) || {};
            savedState[event.target.id] = false;
            localStorage.setItem(storageKey, JSON.stringify(savedState));
        });
    }

    // Restore states
    restoreAccordionState("collapseFilter", "accordionFilterState");

    // Track state changes
    trackAccordionState("collapseFilter", "accordionFilterState");
});