PopoverSetPosition = function (wrapperId, targetId) {
    setTimeout(function () {
        var wrapperNode = document.getElementById(wrapperId);
        //var wrapperBb = wrapperNode.getBoundingClientRect();
        var targetNode = document.getElementById(targetId);
        var targetBb = targetNode.getBoundingClientRect();
        if (!wrapperNode) return;
        wrapperNode.style.left = targetBb.left + "px";
        wrapperNode.style.top = (targetBb.top + targetBb.height + 4) + "px";
        wrapperNode.style.visibility = "visible";
    }, 100);
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