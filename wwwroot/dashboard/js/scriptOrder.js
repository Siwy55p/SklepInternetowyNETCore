

$(document).ready(function () {
    $("#table_3").each(function (_, table) {
        $(table).DataTable({
            "scrollY": "300px",
            "paging": true,
            "scrollCollapse": true,
            pageLength: 10,
            order: [[1, 'desc']]
        });
    });
});
