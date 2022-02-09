NumberBoxOnChange = function (event, decimals) {
    // 48 - 57 (0-9)
    var str1 = event.srcElement.value;

    if (!str1) return;

    if (
        str1[str1.length - 1].charCodeAt() < 48 ||
            str1[str1.length - 1].charCodeAt() > 57
    ) {
        event.srcElement.value = str1.substring(0, str1.length - 1);
        return;
    }

    let str = event.srcElement.value.replace(/,/g, "");

    let value = +str;
    event.srcElement.value = value.toLocaleString('en-US', { maximumFractionDigits: decimals });
}
