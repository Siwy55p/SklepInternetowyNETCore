$(document).ready(function () {
    var table = $('#ProductTableList').DataTable({
        responsive: true,
        pageLength: 50,
        scrollY: "60vh",
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        dom: 'lBfrtip',
        select: true,
        buttons: [
            {
                text: 'Zaznacz/Odznacz wszystkie',
                action: function () {
                    table.rows({ page: 'current' }).select()

                    var count = table.rows({ selected: true }).count();

                    var data = table.rows({ selected: true }).data();

                    for (var i = 0; i < count; i++) {
                        var selectRow = table.row(':eq(' + i + ')').select();
                        var ProduktId = selectRow.id();

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
                            url: '@Url.Action("TakeProductId", "Product")',
                            data: { ProduktId: ProduktId },
                            success: function (result) {
                            },
                            error: function (abc) {

                            },
                        });
                    }
                }
            }
        ],
        lengthMenu: [
            [10, 25, 50, -1],
            [10, 25, 50, 'All'],
        ],
        drawCallback: function () {
            $("img.lazy").lazyload();
        },
        fixedColumns: true,
        "order": [[0, 'desc'], [3, 'desc']]
    });
});
