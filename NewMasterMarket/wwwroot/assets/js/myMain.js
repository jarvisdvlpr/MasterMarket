$(document).ready(function () {
    $(document).on("click", ".add-to-basket", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");
        fetch(url)
            .then(function (response) {
                if (response.ok) {
                    console.log("is working good")
                }
                else {
                    console.log("Error!");
                }
                return response.text();
            }).then(data => {
                $(".basketblock").html(data)
            });
        //li change to div
    });

    //$(document).on("click", ".close_button", function (e) {
    //    e.preventDefault();

    //    let url = $(this).attr("href");

    //    fetch(url)
    //        .then(function (response) {
    //        if (response.ok) {
    //            console.log("deleted! ofc")
    //        }
    //        else {
    //            console.log("error!");
    //        }
    //        return response.text();
    //    }).then(data => {
    //        $(".basketblock").html(data)
    //    });
    //});


    $(document).on("click", ".close_button", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");
        fetch(url)
            .then(function (response) {
                if (response.ok) {
                    console.log("deleted")
                }
                else {
                    console.log("Error!");
                }
                return response.text();
            }).then(data => {
                $(".basketblock").html(data)
            });
        //li change to div
    });

    //$(document).on("click", ".koki", function (e) {
    //    e.preventDefault();

    //    document.cookie = "koki=true";
    //})
});