function submitSearchCriteria() {

    $('#submit').on('click', function (e) {
        e.preventDefault();

        var products = [];
        $("#products-container .item").each(function () {
            products.push($(this).attr('value'));
            //console.log($(this).attr('value'));
        })

        var properties = [];
        $('#propertiesSet input:checked').each(function () {
            properties.push($(this).attr('value'));
            // console.log($(this).attr('value'));
        });
        var selectedCuisines = [];
        $('#cuisinesSet input:checked').each(function () {
            selectedCuisines.push($(this).attr('value'));
            //  console.log($(this).attr('value'));
        });
        var selectedMeals = [];
        $('#mealsSet input:checked').each(function () {
            selectedMeals.push($(this).attr('value'));
            // console.log($(this).attr('value'));
        });

        var criteria = {};
        criteria.Properties = properties;
        criteria.SelectedCuisines = selectedCuisines;
        criteria.SelectedMeals = selectedMeals;
        criteria.Products = products;
        criteria.AreAllPropertiesMatched = $("#applyAll").is(':checked');
        criteria.PrepTime = $("#prepTime").val();

        //console.log($("#applyAll").is(':checked'));
        $("#filters").slideUp(function () {
            $("#toggleFilters").html('<i class="fas fa-arrow-down"></i>');
            $('#toggleFiltersText').html('<em>Filters...</em>');
        });
        //console.log(products);
        var hostString = window.location.protocol + "//" + window.location.host + "/";
        $.ajax({
            // type: "post",
            type: "get",
            url: hostString + "User/RecipeSearch/Search",
            cache: false,
            data: {
                products: JSON.stringify(products),
                properties: JSON.stringify(properties),
                matchAll: criteria.AreAllPropertiesMatched,
                cuisines: JSON.stringify(selectedCuisines),
                meals: JSON.stringify(selectedMeals),
                time: criteria.PrepTime
            }, //JSON.stringify(criteria),
            contentType: "application/json; charset=utf-8",
            //dataType: "json",
            beforeSend: function () {
                $('#buffer').show();
            },
            complete: function () {
                $('#buffer').hide();
            },
            success: function (msg) {

                $("#data").html(msg);
            },
            error: function (req, status, error) {
                console.log('error');
                swal({
                    text: "Oops, something went wrong. Please try again later.",
                    icon: "error"
                });
            }

        });
    });
}
function toggleSearchCriteria() {
    $("#toggleFilters").on("click", function () {
        console.log('click');
        var selectedEffect = 'blind';
        var options = {};

        // Run the effect
        $("#filters").toggle(selectedEffect, options, 500, function () {
            var elementVisible = $(this).is(':hidden');
            if (elementVisible) {
                $("#toggleFilters").html('<i class="fas fa-arrow-down"></i>');
                $('#toggleFiltersText').html('<em>Filters...</em>');
            }
            else {
                $('#toggleFiltersText').text('');
                $("#toggleFilters").html('<i class="fas fa-arrow-up"></i>');

            }
        });

    });
}

$(document).ready(function () {
    submitSearchCriteria();
    toggleSearchCriteria();
});