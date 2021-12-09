ScrollTo = function (id) {
    var node = document.getElementById(id);
    if (!!node) {
        console.log(node);
        setTimeout(function() {
                node.scrollIntoView();
            },
            1000);
    }
}