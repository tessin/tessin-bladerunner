BladesScrollTo = function (id) {
    var node = document.getElementById(id);
    if (!!node) {
        node.scrollIntoViewIfNeeded();
    }
}