NumberBoxOnFocusOut = function (event, decimals, min, max) {
    //debugger;
    
    const str1 = event.srcElement.value;

    if (!str1) return;

    const str = event.srcElement.value.replace(/,/g, "");

    let value = +str; //converts to number

    if (isNaN(value)) return; //leave it to NumberBox to complain

    let adjusted = Math.max(Math.min(max, value), min);
    
    event.srcElement.value = adjusted.toLocaleString('en-US', { maximumFractionDigits: decimals });
}
