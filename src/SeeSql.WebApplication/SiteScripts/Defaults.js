var SeeSqlDefaults = (function () {
    return {
        DataTableOptions: {
            "pageLength": -1,
            "lengthMenu": [[15, 25, 50, -1], [15, 25, 50, "All"]],
            "order": [1, "asc"]
        },
        JobHistoryDataTableOptions: {
            "paging": false,
            "ordering": false,
            "searching": false
        },
        JobRollupDataTableOptions: {
            "pageLength": 7,
            "lengthMenu": [[7, 14, 31, -1], ["1 week", "2 weeks", "1 month", "All"]],
            "order": [2, "desc"]
        }
    };
})();