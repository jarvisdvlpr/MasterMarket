 /**=====================
     Quantity 2 js
==========================**/
//var basketamount;
var minAmount;
var myElement;

$(document).ready(function () {
    // basketamount = parseInt($("#my-data").val());
    minAmount = parseInt($("#my-amount").val());
    myElement = $("#multimy").val();
   /* console.log(amount);*/
})
//amount eto minamount

 $(".addcart-button").click(function () {
     $(this).next().addClass("open");
     $(".add-to-cart-box .qty-input").val('1');
 });

 $('.add-to-cart-box').on('click', function () {
     var $qty = $(this).siblings(".qty-input");
     var currentVal = parseInt($qty.val());
     if (!isNaN(currentVal)) {
         $qty.val(currentVal + 1);
     }
 });

 $('.qty-left-minus').on('click', function () {
     var $qty = $(this).siblings(".qty-input");
     var _val = $($qty).val();
     if (_val == '1') {
         var _removeCls = $(this).parents('.cart_qty');
         $(_removeCls).removeClass("open");
     }
     var currentVal = parseInt($qty.val());
     if (!isNaN(currentVal) && currentVal > minAmount ) {
         $qty.val(currentVal - minAmount);
     }

     
 });

 $('.qty-right-plus').click(function () {
     //if ($(this).prev().val() < 100) {
     //    $(this).prev().val(+$(this).prev().val() + 1);
     //}

    
     
     if ($(this).prev().val() < 1000) {
         $(this).prev().val(+$(this).prev().val() + minAmount); 
     }

     //if (myElement == null) {
     //    console.log("working!");
     //}

     console.log("working")
     console.log(myElement)
 });

//$('.addtomycart-btn').click(function (e) {
//    e.preventDefault();

//    let url = $(this).attr("href");
//    fetch(url)
//        .then(function (response) {
//            if (response.ok) {
//                console.log("is working good")
//            }
//            else {
//                console.log("Error!");
//            }
//            return response.text();
//        }).then(data => {
//            $(".basketblock").html(data)
//        });
//    //tut dobavlenie v karzinu
//});