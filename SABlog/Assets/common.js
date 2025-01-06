function removeSpaces(element) {
    $(element).val($(element).val().replace(/\s+/g, ''));
}

function removeLeadingSpaces(element) {
    $(element).val($(element).val().replace(/^\s+/, ''));
}