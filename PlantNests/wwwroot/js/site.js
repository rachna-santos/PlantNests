
/*setquantity*/
            var QtyInput = (function () {
                var $qtyInputs = $(".qty-input");

                if (!$qtyInputs.length) {
                    return;
                }

                var $inputs = $qtyInputs.find(".product-qty");
                var $countBtn = $qtyInputs.find(".qty-count");
                var qtyMin = parseInt($inputs.attr("min"));
                var qtyMax = parseInt($inputs.attr("max"));

                $inputs.change(function () {
                    var $this = $(this);
                    var $minusBtn = $this.siblings(".qty-count--minus");
                    var $addBtn = $this.siblings(".qty-count--add");
                    var qty = parseInt($this.val());

                    if (isNaN(qty) || qty <= qtyMin) {
                        $this.val(qtyMin);
                        $minusBtn.attr("disabled", true);
                    } else {
                        $minusBtn.attr("disabled", false);

                        if (qty >= qtyMax) {
                            $this.val(qtyMax);
                            $addBtn.attr('disabled', true);
                        } else {
                            $this.val(qty);
                            $addBtn.attr('disabled', false);
                        }
                    }
                });

                $countBtn.click(function () {
                    var operator = this.dataset.action;
                    var $this = $(this);
                    var $input = $this.siblings(".product-qty");
                    var qty = parseInt($input.val());

                    if (operator == "add") {
                        qty += 1;
                        if (qty >= qtyMin + 1) {
                            $this.siblings(".qty-count--minus").attr("disabled", false);
                        }

                        if (qty >= qtyMax) {
                            $this.attr("disabled", true);
                        }
                    } else {
                        qty = qty <= qtyMin ? qtyMin : (qty -= 1);

                        if (qty == qtyMin) {
                            $this.attr("disabled", true);
                        }

                        if (qty < qtyMax) {
                            $this.siblings(".qty-count--add").attr("disabled", false);
                        }
                    }

                    $input.val(qty);
                });
            })();

            /*rating*/
            $(document).ready(function () {
                $(".ratingstart").hover(function () {
                    if (!$(this).hasClass("active")) {
                        $(".ratingstart").removeClass("fas").addClass("far");
                        $(this).addClass("fas").removeClass("far");
                        $(this).prevAll(".ratingstart").addClass("fas").removeClass("far");
                    }
                });

                $(".ratingstart").click(function () {
                    var startvalue = $(this).data("value");
                    var productId = $(this).data("productid");

                    $.ajax({
                        url: '/Client/Rating',
                        type: 'POST',
                        data: {
                            productid: productId,
                            rating: startvalue
                        },
                        success: function (response) {
                        },
                        error: function (xhr, status, error) {
                            alert('Error: ' + error);
                        }
                    });
                });
            });

      
