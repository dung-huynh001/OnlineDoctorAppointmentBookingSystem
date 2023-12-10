function validateUsername(event) {
    var regex = /^[a-zA-Z0-9]+$/;
    var value = event.key;
    return regex.test(value);
}

function validatePassword(event) {
    var regex = /^[^\s\u00C0-\u017F]+$/;
    var value = event.key;
    return regex.test(value);
}

function validateUnicode(event) {
    var regex = /^[\p{L}\d\s]+$/u;
    var value = event.key;
    return regex.test(value);
}

function validateNumber(event) {
    var regex = /^[0-9]+$/;
    var value = event.key;
    return regex.test(value);
}

function validateEmail(event) {
    var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
    var value = event.key;
    return regex.test(value);
}