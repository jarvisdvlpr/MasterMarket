$(document).ready(function(){
    $(document).on("click", "#mymodalwindow", function(e){
        e.preventDefault();

        let url = $(this).attr("href");

        fetch(url)
            .then(response => response.text())
            .then(data=>{  

                $("#view .modal-dialog").html(data)
                $("#view").modal(true)
            });  
    })
})