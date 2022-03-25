NumberBoxOnFocusOut = function (event, decimals) {
    const str1 = event.srcElement.value;

    if (!str1) return;

    const str = event.srcElement.value.replace(/,/g, "");

    const value = +str; //converts to number

    if (isNaN(value)) return; //leave it to NumberBox to complain

    event.srcElement.value = value.toLocaleString('en-US', { maximumFractionDigits: decimals });
}
