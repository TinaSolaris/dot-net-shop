var ajaxCallUrl = 'ListPartial',
    index = 0,
    count = 100,
    inCallback = false,
    isReachedScrollEnd = false;

var scrollHandler = function () {
    if (isReachedScrollEnd == false &&
        ($(document).scrollTop() <= $(document).height() - $(window).height())) {
        loadProducts(ajaxCallUrl);
    }
}
function loadProducts(ajaxCallUrl) {
    var categoryId = $('#CategoryId').val();
    console.debug("categoryId: " + categoryId);

    if (index > -1 && !inCallback) {
        inCallback = true;
        index = index + count;
        console.debug("index: " + index);

        $("div#loading").show();
        $.ajax({
            type: 'GET',
            url: ajaxCallUrl,
            data: "categoryId=" + categoryId + "&index=" + index,
            success: function (data, textstatus) {
                if (data != '' && data.length > 20) {
                    $("#articles > tbody").append(data);
                    console.debug("data: exists");
                }
                else {
                    console.debug("data: empty");
                    index = -1;
                    isReachedScrollEnd = true;
                }

                inCallback = false;
                $("div#loading").hide();
            },
            error: function (errorThrown) {
                alert(errorThrown);
            }
        });
    }
}