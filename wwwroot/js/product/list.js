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
