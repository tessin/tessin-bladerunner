ToasterSetPosition = function (wrapperId, timeout) {
    if (timeout > 0) {
        setTimeout(function () {
            var wrapperNode = document.getElementById(wrapperId);
            if (!wrapperNode) return;
            wrapperNode.classList.add("toaster-fadeout");
        }, timeout);
    }
}