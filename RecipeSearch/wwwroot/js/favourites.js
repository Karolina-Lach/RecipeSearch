/********** VALIDATIONS **************/
var isValidName = function (value) {

    var valid = new RegExp("^[A-Za-z0-9]{1,20}$");
    return (valid.test(value) && value != "");

}


/************************************/

function AddRecipeToList(recipeId, listId) {
    var hostString = window.location.protocol + "//" + window.location.host + "/";
    $.ajax({
        type: "post",
        url: hostString + "User/Favourites/AddRecipeToList",
        data: { recipeId: recipeId, listId: listId },
        dataType: "html",
        success: function (data) {
            $("#statusMessage").html(data);
        },
        error: function (req, status, error) {
            console.log(status);
            console.log(error);
            swal({
                text: "Oops, something went wrong. Please try again later.",
                icon: "error"
            });
        }

    });
}
function DragAndDrop() {
    $(".draggable").draggable({
        revert: true,
        revertDuration: 100,
        zIndex: 100
    });

    
    $(".droppable").droppable({
        tolerance: "pointer",
        drop: function (event, ui) {
            var dragId = ui.draggable.attr("value");
            var dropId = $(this).attr("value");
            console.log(dragId);
            console.log(dropId);
            AddRecipeToList(dragId, dropId);
        }
    });
}
function DeleteRecipeFromList(recipeId) {
    var hostString = window.location.protocol + "//" + window.location.host + "/";
    var listId = $("#listDropdown option:selected").val();
    
    $.ajax({
        type: "post",
        url: hostString + "User/Favourites/RemoveFromList",
        data: { recipeId: recipeId, listId: listId },
        success: function (data) {
            $("#statusMessage").html(data);
            $.get({
                url: hostString + "User/Favourites/SelectFavouritesList",
                data: { selectedList: listId },
                success: function (data) {
                    $("#recipesOnList").html(data);
                    DragAndDrop();
                }
            });
        },
        error: function (req, status, error) {

            console.log(status);
            console.log(error);
            swal({
                text: "Oops, something went wrong. Please try again later.",
                icon: "error"
            });
        }
    });
}
function DropdownChange() {
    var id = $("#listDropdown option:selected").val();
    var hostString = window.location.protocol + "//" + window.location.host + "/";
    $.ajax({
        type: "get",
        url: hostString + "User/Favourites/SelectFavouritesList",
        data: { selectedList: id },
        dataType: "html",
        success: function (data) {
            // populate recipes list
            $("#recipesOnList").html(data);
            DragAndDrop();
        },
        error: function (req, status, error) {
            console.log(status);
            console.log(error);
        }

    });
}
function ChangePage(currentList, pageNumber) {
    var hostString = window.location.protocol + "//" + window.location.host + "/";
    $.ajax({
        type: "get",
        url: hostString + "User/Favourites/SelectFavouritesList",
        data: { currentList: currentList, pageNumber: pageNumber },
        dataType: "html",
        success: function (data) {
            // populate recipes list
            $("#recipesOnList").html(data);
            DragAndDrop();
        },
        error: function (req, status, error) {
            console.log(status);
            console.log(error);
        }

    });
}
function DeleteList(id) {
    var hostString = window.location.protocol + "//" + window.location.host + "/";
    $.ajax({
        type: "delete",
        url: hostString + "User/Favourites/RemoveListJson/" + id,
        data: { id: id },
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                // populate new dropdown
                $.get(hostString + "User/Favourites/FavouritesDropDownView", function (data) {
                    $("#dropdown").html(data);
                });
                //populate new list
                $.get(hostString + "User/Favourites/ListOfFavouritesListsView", function (data) {
                    $("#listName").val("");
                    $("#listOfFavouritesLists").html(data);
                    DragAndDrop();
                });
            }
            else {
                swal({
                    text: "Oops, something went wrong. Please try again later.",
                    icon: "error"
                });
            }
        },
        error: function (req, status, error) {

            console.log(status);
            console.log(error);
            swal({
                text: "Oops, something went wrong. Please try again later.",
                icon: "error"
            });
        }

    });
    //});
}
function AddList() {
    var name = $("#listName").val();
    //console.log(name);
    if (!isValidName(name)) {
        swal({
            text: "Name must have between 1 and 20 characters.",
            icon: "error"
        })
    }
    else if (name == "All" || name == "all"){
        swal({
            text: "List cannot be named 'All'.",
            icon: "error"
        })
    }
    else {
        var hostString = window.location.protocol + "//" + window.location.host + "/";

        $.ajax({
            type: "post",
            url: hostString + "User/Favourites/AddNewListJson",
            cache: false,
            data: { name: name },
            dataType: "json",
            success: function (list) {
                //console.log(list);
                if (list == null) {
                    swal({
                        text: "Oops, something went wrong. Please try again later.",
                        icon: "error"
                    })
                }
                else {
                    // add new  element to dropdown
                    $("#listName").val("");
                    $("#listDropdown").append('<option value="' + list.id + '">' + list.name + '</option>');
                    $.get(hostString + "User/Favourites/ListOfFavouritesListsView", function (data) {
                        $("#listName").val("");
                        // populate list of lists
                        $("#listOfFavouritesLists").html(data);
                        DragAndDrop();
                    });
                }
            },
            error: function (req, status, error) {
                $("#listName").val("");
                console.log(status);
                console.log(error);
                swal("Oops, something went wrong.", "Please, try again later.", "error");
            }

        });
    }
}

$(document).ready(function () {
    DragAndDrop();
});

