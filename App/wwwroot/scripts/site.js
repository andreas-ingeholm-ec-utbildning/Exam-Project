//Registers the urlChanged event.
setTimeout(() => {  
        const urlChangedEvent = new Event("urlChanged");

        let previousUrl = '';
        const observer = new MutationObserver(function (mutations) {
            if (location.href !== previousUrl) {
                previousUrl = location.href;
                window.dispatchEvent(urlChangedEvent);
            }
        });

        const config = { subtree: true, childList: true };
        observer.observe(document, config);
}, 0);

/**
 * Moves an element depending on viewport width. Automatically re-applies on resize.
 */
function moveElementDependingOnViewportSize(element, width, smallViewportSelector, largeViewportSelector) {
    doMove();
    window.addEventListener('resize', doMove);

    function doMove() {
        if (window.innerWidth <= width) {
            document.querySelector(smallViewportSelector).appendChild(element);
        } else {
            document.querySelector(largeViewportSelector).appendChild(element);
        }
    }
}

/**
 * Sets up a 'popup' using a details tag.
 */
function setupPopup(detailsSelector) {
    var details = [...document.querySelectorAll(detailsSelector)];
    document.addEventListener('click', function (e) {
        if (!details.some(f => f.contains(e.target))) {
            details.forEach(f => f.removeAttribute('open'));
        } else {
            details.forEach(f => !f.contains(e.target) ? f.removeAttribute('open') : '');
        }
    })
}

/**
 * Gets a query parameter from the current url.
 */
function getQuery(key) {
    return new URLSearchParams(window.location.search).get(key);
}