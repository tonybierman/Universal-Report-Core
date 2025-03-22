// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("selectAll").addEventListener("change", function () {
        let checkboxes = document.querySelectorAll(".selectRow");
        checkboxes.forEach(cb => cb.checked = this.checked);
    });
});
document.addEventListener("DOMContentLoaded", function () {
    const accordion = document.getElementById("collapseFilter");

    // Restore state from localStorage
    const savedState = JSON.parse(localStorage.getItem("accordionState")) || {};
    Object.keys(savedState).forEach(id => {
        const element = document.getElementById(id);
        if (element && savedState[id]) {
            new bootstrap.Collapse(element, { toggle: true });
        }
    });

    // Save state when accordion is shown or hidden
    accordion.addEventListener("shown.bs.collapse", event => {
        const savedState = JSON.parse(localStorage.getItem("accordionState")) || {};
        savedState[event.target.id] = true;
        localStorage.setItem("accordionState", JSON.stringify(savedState));
    });

    accordion.addEventListener("hidden.bs.collapse", event => {
        const savedState = JSON.parse(localStorage.getItem("accordionState")) || {};
        savedState[event.target.id] = false;
        localStorage.setItem("accordionState", JSON.stringify(savedState));
    });
});
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
    restoreAccordionState("collapseChart", "accordionChartState");

    // Track state changes
    trackAccordionState("collapseFilter", "accordionFilterState");
    trackAccordionState("collapseChart", "accordionChartState");
});
