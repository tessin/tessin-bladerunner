function PopoverSetPosition(wrapperId, anchorId) {
    // the timeout is absolutly needed but code like this is abhorrent
    setTimeout(() => {
        const wrapper = document.getElementById(wrapperId)
        const target = document.getElementById(anchorId)
        const docEl = document.documentElement
        if ((wrapper && target && docEl)) {
            const wrapperRect = wrapper.getBoundingClientRect()
            const targetRect = target.getBoundingClientRect()
            const viewport = docEl.getBoundingClientRect()

            // it is possible that the popover is occluded by the viewport
            // if this happens we need to try a different layout

            wrapper.style.position = "fixed" // position relative to the viewport

            const wrapperWidth = wrapperRect.width
            const wrapperHeight = wrapperRect.height

            const targetLeft = targetRect.left
            const targetTop = targetRect.top
            const targetWidth = targetRect.width
            const targetHeight = targetRect.height

            let left = targetLeft + targetWidth
            if (left + wrapperWidth < viewport.width) {
                // the popover can flow left-to-right within the viewport
            } else {
                left = targetLeft - wrapperWidth
            }

            let top = targetTop + targetHeight / 2
            if (top + wrapperHeight < viewport.height) {
                // the popover can flow top-to-bottom within the viewport
            } else {
                top = targetTop + targetHeight / 2 - wrapperHeight
            }

            wrapper.style.left = `${left}px`;
            wrapper.style.top = `${top}px`;
            wrapper.style.visibility = "visible";
        }
    }, 100)
}

document.addEventListener('click', function (e) {
    var portals = document.getElementsByClassName("popover-wrapper");
    var n = portals.length;
    for (var i = 0; i < n; ++i) {
        var portal = portals[i];
        if (portal.style.visibility == 'visible' && !portal.contains(e.target)) {
            portal.style.visibility = 'hidden';
        }
    }
});