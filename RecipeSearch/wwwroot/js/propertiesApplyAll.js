function applyAllProperties() {
    $('#propertiesSet').on('change', function () {
        var atLeastOneIsChecked = $('.propCheckbox:checked').length;

        if (atLeastOneIsChecked > 0) {

            $("#applyAll").attr("hidden", false);
            $("#applyAll").attr("disabled", false);

        } else {
            $("#applyAll").prop("checked", false);
            $("#applyAll").attr("disabled", true);

        }

    });
}



$(document).ready(function () {
    applyAllProperties();
});