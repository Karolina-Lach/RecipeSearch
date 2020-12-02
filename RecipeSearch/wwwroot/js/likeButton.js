function Like(id) {
    var hostString = window.location.protocol + "//" + window.location.host + "/" + "User/Favourites/AddToFavourites";
    $.ajax({
        type: "post",
        url: hostString,
        data: { id: id },
        dataType: "text",
        success: function (isLiked) {
            console.log(isLiked);
            if (isLiked == 'true') {
                $(".likeButton[value=" + id + "]").html('<i class="fas fa-heart"></i>');
            }
            else {
                $(".likeButton[value=" + id + "]").html('<i class="far fa-heart"></i>');
            }
        },
        error: function (req, status, error) {
            console.log(error);
            if (req.status == 401) {
                swal({
                    text: "You need to be logged in to do that!",
                    icon: "warning",
                });
            }
            else {
                swal({
                    text: "Unexpected error has occured. Please try again later.",
                    icon: "error",
                });
            }
        }

    })
}
