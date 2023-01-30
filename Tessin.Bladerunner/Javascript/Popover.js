PopoverSetPosition = function (wrapperId, targetId) {
    setTimeout(function () {
        const wrapperNode = document.getElementById(wrapperId);
        const wrapperBb = wrapperNode.getBoundingClientRect();
        const targetNode = document.getElementById(targetId);
        const targetBb = targetNode.getBoundingClientRect();
        if (!wrapperNode) return;
        wrapperNode.style.left = (targetBb.left + targetBb.width - wrapperBb.width) + "px";
        wrapperNode.style.top = (targetBb.top + targetBb.height + 4) + "px";
        wrapperNode.style.visibility = "visible";
    }, 100);
}

document.addEventListener('click', function (e) {
    const portals = document.getElementsByClassName("popover-wrapper");
    const n = portals.length;
    for (let i = 0; i < n; ++i) {
        const portal = portals[i];
        if (portal.style.visibility === 'visible' && !portal.contains(e.target)) {
            portal.style.visibility = 'hidden';
        }
    }
});