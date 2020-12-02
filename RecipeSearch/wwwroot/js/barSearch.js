function LoadData(data) {
    console.log(data);
    $.get({
        url: "User/Home/Index/?search=" + data,
        success: function (data) {
            $("#data").html(data);
        }
    });
}
$(document).ready(function () {
    $('#barSearchForm').on('submit', function (e) {
        e.preventDefault();
        var data = $("#barSearch").val()
        LoadData(data);
    })
});