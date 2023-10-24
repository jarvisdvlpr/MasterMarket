$(document).on("click", "#deletexuynu", function(e){
    e.preventDefault();

    let url = $(this).attr("href");
    
    Swal.fire({
        title: 'Удаление',
        text: "Вы уверены что хотите удалить этот продкут?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Нет',
        confirmButtonText: 'Да, Удали!'
      }).then((result) => {
        if (result.isConfirmed) {
          Swal.fire({
            title: 'Удалено!',
            text:'Выбранный продукт удален!',
            icon:'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Ok'
        }).then( setTimeout(function(){
            fetch(url).then(location.reload())
        },2000))
        }
      })
})