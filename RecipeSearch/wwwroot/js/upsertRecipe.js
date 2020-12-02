var hostString = window.location.protocol + "//" + window.location.host + "/"

/************* Amount and product validations *************************/
var isValidMeasurement = function (value) {
    var valid = new RegExp("^([0-9]+(?:[.,][0-9]{1,2})?)$");
    return (valid.test(value) && value != 0 && value != "");
}
$.validator.addMethod("isValidMeasurement", function (value, element) {
    if (!isValidMeasurement(value)) {
        jQuery.validator.format("This field contains invalid data.");
        $("#measurementInputValidation").text("Please, use the number between 0.01 and 99999.99 with maximum 2 decimal digits.");
        return false;
    }
    $("#measurementInputValidation").text("");
    return true;
});
var isValidProduct = function (value) {
    if (value == 0 || value == "") {
        return false;
    }
    return true;
}
$.validator.addMethod("isValidProduct", function (value, element) {
    if (!isValidProduct(value)) {
        jQuery.validator.format("This field contains invalid data.");
        $("#productInputValidation").text("Please, choose a product.");
        return false;
    }
    $("#productInputValidation").text("");
    return true;
});
var isValidProductName = function (value) {
    if (value == "") {
        return false;
    }
    return true;
}
$.validator.addMethod("isValidProductName", function (value, element) {
    if (!isValidProduct(value)) {
        jQuery.validator.format("This field contains invalid data.");
        $("#productNameValidation").text("Please, provide a product name.");
        return false;
    }
    $("#productNameValidation").text("");
    return true;
});
/***********************************************************************/

function AddIngredient() {
    $("#addIngredient").on('click', function (e) {
        e.preventDefault();
        var measurementInput = $("#measurementInput").val();
        var productInput = $("#productInput").val();
        var unitInput = $("#unitInput").val();
        var commentInput = $("#commentInput").val();
        if (!isValidMeasurement(measurementInput)) {
            $("#measurementInputValidation").text("Please, use the number between 0.01 and 99999.99 with maximum 2 decimal digits.");
        }
        else if (!isValidProduct(productInput)) {
            $("#productInputValidation").text("Please, choose a product.");
        }
        else {
            $.ajax({
                type: 'post',
                url: hostString + "User/Recipe/AddIngredients",
                data: {
                    "measurement": measurementInput, "productId": productInput, "unitId": unitInput, "comment": commentInput
                },
                dataType: 'json',
                success: function (data) {
                    if (data.success == true) {
                        $("#measurementInput").val("");
                        $("#productInput").val("");
                        $("#unitInput").val("");
                        $("#commentInput").val("");
                        var html = '<div class="item">\
                                    <div class="remove">X</div>' + data.message + '<input type="hidden" measurement="' + measurementInput + '" product="' + productInput + '" unit="' + unitInput + '" comment="' + commentInput + '" /></div> ';
                        $('#products-container').append(html);
                    }
                    else {
                        swal({
                            text: data.message,
                            icon: "warning"
                        });
                    }
                },
                error: function () {
                    swal({
                        text: "Oops, something went wrong. Please try again later.",
                        icon: "error"
                    });
                }
            });
        }

    });
}
function RemoveIngredient() {
    $('#products-container').on('click', '.remove', function () {
        var measurementInput = $(this).parent().find("input:hidden").attr("measurement");
        var productInput = $(this).parent().find("input:hidden").attr("product");
        var unitInput = $(this).parent().find("input:hidden").attr("unit");
        var commentInput = $(this).parent().find("input:hidden").attr("comment");

        $(this).parent().remove();
        $.ajax({
            type: 'post',
            url: hostString + "User/Recipe/RemoveIngredients",
            data: {
                "measurement": measurementInput, "productId": productInput, "unitId": unitInput, "comment": commentInput
            },
            dataType: 'json',
            success: function (data) {
                if (data.success == false) {
                    swal({
                        text: data.message,
                        icon: "warning"
                    });
                }
            },
            error: function () {
                swal({
                    text: "Oops, something went wrong. Please, try again later.",
                    icon: "warning"
                });
            }
        });
    });
}
function SubmitProductForm() {
    $("#addProduct").on('click', function (e) {
        var name = $("#productName").val()
        var pluralName = $("#productPluralName").val()
        if (!isValidProductName(name)) {
            e.preventDefault();
            $("#productNameValidation").text("Please, provide a product name.");
        }
        else {
            $("#productNameValidation").text("");
            var hostString = window.location.protocol + "//" + window.location.host + "/";
            $.ajax({
                type: "post",
                url: hostString + "User/Recipe/AddProduct",
                data: { name: name, pluralName: pluralName },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        $.get({
                            url: hostString + "User/Recipe/GetProductDropdownList",
                            success: function (view) {
                                $("#productDropdown").html(view);
                                $("#productInput").val(data.id).change();
                            }
                        });
                        $("#productName").val("")
                        $("#productPluralName").val("")
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    swal({
                        text: "Oops, something went wrong. Please try again later.",
                        icon: "error"
                    });
                }
            })
        }
    })
};
function AddStep() {
    $("#addStep").on('click', function (e) {
        e.preventDefault();
        $("#stepsList").append('<div class="input-group"> \
                                             <input type = "text" name = "steps" class= "form-control" /> \
                                            <div class="input-group-append remove"> \
                                              <span class="input-group-text " style="padding-right:2em"> X </span> \
                                              </div> \
                                              </div >');
    });
}
function RemoveStep() {
    $('#stepsList').on('click', '.remove', function () {
        if ($("#stepsList").find('input').length > 1) {
            $(this).parent().remove();
        }
    });
}
function SubmitForm() {
    $("#submit").on('click', function (e) {
        var selectedMeals = 0;
        $('#mealsSet input:checked').each(function () {
            selectedMeals = selectedMeals + 1;
            // console.log($(this).attr('value'));
        });
        if (selectedMeals < 1) {
            e.preventDefault();
            swal({
                text: "You have to choose at least 1 type of meal!",
                icon: "warning"
            });
        }

        var filledSteps = 0;
        $("input[name='steps']").each(function () {
            var trimmed = $(this).val().trim();
            if (trimmed != "") {
                filledSteps = filledSteps + 1;
            }
        })
        if (filledSteps < 1) {
            e.preventDefault();
            swal({
                text: "You have to fill at least one step!",
                icon: "warning"
            });
        }
        if ($('#products-container').children().length < 1) {
            e.preventDefault();
            swal({
                text: "You have to use at least one ingredient!",
                icon: "warning"
            });
        }
    })
}
$(document).ready(function () {
    AddIngredient();
    RemoveIngredient();
    AddStep();
    RemoveStep();
    SubmitForm();
    SubmitProductForm();
});