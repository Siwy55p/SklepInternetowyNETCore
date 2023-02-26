
    function ChangeCategory(ProduktId, CategoryId) {
            var d = document.getElementById("select_" + ProduktId).value;
    $.ajax({
        type: "POST",
    url: '@Url.Action("ChangeCategory", "Product")',
    data: {ProduktId: ProduktId, KategoriaID: d },
    success: function (result) {
    },
    error: function (abc) {
        alert(abc.statusText);
                },
            });
        };
    function ChangeCheck(ProduktId) {
            var check = document.getElementById('chkP_' + ProduktId);
    if (check.checked == true) {
        check.checked = false;
    check = false;
            } else {
        check.checked = true;
    check = true;
            }
    $.ajax({
        type: "POST",
    url: '@Url.Action("TakeProduct", "Product")',
    data: {ProduktId: ProduktId, check: check },
    success: function (result) {

    },
    error: function (abc) {
        alert(abc.statusText);
                },
            });
        };
    function ChangeCheckUkrytyA(ProduktId) {
            var checkUkryty = document.getElementById('chkUkryty_' + ProduktId);

    if (checkUkryty.checked == true) {
        checkUkryty.checked = false;
    checkUkryty = false;
            } else {
        checkUkryty.checked = true;
    checkUkryty = true;
            }
    $.ajax({
        type: "POST",
    url: '@Url.Action("ChangeCheckUkryty", "Product")',
    data: {ProduktId: ProduktId, checkUkryty: checkUkryty },
    success: function (result) {
    },
    error: function (abc) {
        alert(abc.statusText);
                },
            });
        };
    function ChangeIlosc(ProduktId) {
            var d = document.getElementById("ilosc_" + ProduktId).value;
    $.ajax({
        type: "POST",
    url: '@Url.Action("ChangeIlosc", "Product")',
    data: {ProduktId: ProduktId, Ilosc: d },
    success: function (result) {
                    if (document.getElementById('chkUkryty_' + ProduktId).checked) {
        checkUkryty.checked = false;
    checkUkryty = false;
                    } else {
        checkUkryty.checked = true;
    checkUkryty = true;
                    }
                },
    error: function (abc) {
        alert(abc.statusText);
                },
            });
        };
    $('#ProductTableList tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');

    var ProduktId = $(this).attr("id");

    var check = document.getElementById('chkP_' + ProduktId);

    if (check.checked == true) {
        check.checked = false;
    check = false;
            } else {
        check.checked = true;
    check = true;
            }
    $.ajax({
        type: "POST",
    url: '@Url.Action("TakeProduct", "Product")',
    data: {ProduktId: ProduktId, check: check },
    success: function (result) {
    },
    error: function (abc) {
        alert(abc.statusText);
                },
            });
    });
