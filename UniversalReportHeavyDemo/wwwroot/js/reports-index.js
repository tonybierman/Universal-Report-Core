document.addEventListener("DOMContentLoaded", function () {
    function manageAccordionState(accordionId, storageKey) {
        // Restore state
        const savedState = JSON.parse(localStorage.getItem(storageKey)) || {};
        const element = document.getElementById(accordionId);
        if (element && savedState[accordionId]) {
            new bootstrap.Collapse(element, { toggle: true });
        }

        // Track state changes
        if (element) {
            element.addEventListener("shown.bs.collapse", () => {
                const state = JSON.parse(localStorage.getItem(storageKey)) || {};
                state[accordionId] = true;
                localStorage.setItem(storageKey, JSON.stringify(state));
            });
            element.addEventListener("hidden.bs.collapse", () => {
                const state = JSON.parse(localStorage.getItem(storageKey)) || {};
                state[accordionId] = false;
                localStorage.setItem(storageKey, JSON.stringify(state));
            });
        }
    }

    // Apply to both filter and search
    manageAccordionState("collapseFilter", "accordionFilterState");
    //manageAccordionState("collapseSearch", "accordionSearchState");
});